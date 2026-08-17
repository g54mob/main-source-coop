using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace kcp2k
{
	public abstract class KcpPeer
	{
		internal Kcp kcp;

		internal uint cookie;

		protected KcpState state;

		public const int DEFAULT_TIMEOUT = 10000;

		public int timeout;

		private uint lastReceiveTime;

		private readonly Stopwatch watch = new Stopwatch();

		private readonly byte[] kcpMessageBuffer;

		private readonly byte[] kcpSendBuffer;

		private readonly byte[] rawSendBuffer;

		public const int PING_INTERVAL = 1000;

		private uint lastPingTime;

		internal const int QueueDisconnectThreshold = 10000;

		public const int CHANNEL_HEADER_SIZE = 1;

		public const int COOKIE_HEADER_SIZE = 4;

		public const int METADATA_SIZE = 5;

		public readonly int unreliableMax;

		public readonly int reliableMax;

		public int SendQueueCount => kcp.snd_queue.Count;

		public int ReceiveQueueCount => kcp.rcv_queue.Count;

		public int SendBufferCount => kcp.snd_buf.Count;

		public int ReceiveBufferCount => kcp.rcv_buf.Count;

		public uint MaxSendRate => kcp.snd_wnd * kcp.mtu * 1000 / kcp.interval;

		public uint MaxReceiveRate => kcp.rcv_wnd * kcp.mtu * 1000 / kcp.interval;

		private static int ReliableMaxMessageSize_Unconstrained(int mtu, uint rcv_wnd)
		{
			return (mtu - 24 - 5) * (int)(rcv_wnd - 1) - 1;
		}

		public static int ReliableMaxMessageSize(int mtu, uint rcv_wnd)
		{
			return ReliableMaxMessageSize_Unconstrained(mtu, Math.Min(rcv_wnd, 255u));
		}

		public static int UnreliableMaxMessageSize(int mtu)
		{
			return mtu - 5 - 1;
		}

		protected KcpPeer(KcpConfig config, uint cookie)
		{
			Reset(config);
			this.cookie = cookie;
			Log.Info($"[KCP] {GetType()}: created with cookie={cookie}");
			rawSendBuffer = new byte[config.Mtu];
			unreliableMax = UnreliableMaxMessageSize(config.Mtu);
			reliableMax = ReliableMaxMessageSize(config.Mtu, config.ReceiveWindowSize);
			kcpMessageBuffer = new byte[1 + reliableMax];
			kcpSendBuffer = new byte[1 + reliableMax];
		}

		protected void Reset(KcpConfig config)
		{
			cookie = 0u;
			state = KcpState.Connected;
			lastReceiveTime = 0u;
			lastPingTime = 0u;
			watch.Restart();
			kcp = new Kcp(0u, RawSendReliable);
			kcp.SetNoDelay(config.NoDelay ? 1u : 0u, config.Interval, config.FastResend, !config.CongestionWindow);
			kcp.SetWindowSize(config.SendWindowSize, config.ReceiveWindowSize);
			kcp.SetMtu((uint)(config.Mtu - 5));
			kcp.dead_link = config.MaxRetransmits;
			timeout = config.Timeout;
		}

		protected abstract void OnAuthenticated();

		protected abstract void OnData(ArraySegment<byte> message, KcpChannel channel);

		protected abstract void OnDisconnected();

		protected abstract void OnError(ErrorCode error, string message);

		protected abstract void RawSend(ArraySegment<byte> data);

		private void HandleTimeout(uint time)
		{
			if (time >= lastReceiveTime + timeout)
			{
				OnError(ErrorCode.Timeout, $"{GetType()}: Connection timed out after not receiving any message for {timeout}ms. Disconnecting.");
				Disconnect();
			}
		}

		private void HandleDeadLink()
		{
			if (kcp.state == -1)
			{
				OnError(ErrorCode.Timeout, $"{GetType()}: dead_link detected: a message was retransmitted {kcp.dead_link} times without ack. Disconnecting.");
				Disconnect();
			}
		}

		private void HandlePing(uint time)
		{
			if (time >= lastPingTime + 1000)
			{
				SendPing();
				lastPingTime = time;
			}
		}

		private void HandleChoked()
		{
			int num = kcp.rcv_queue.Count + kcp.snd_queue.Count + kcp.rcv_buf.Count + kcp.snd_buf.Count;
			if (num >= 10000)
			{
				OnError(ErrorCode.Congestion, $"{GetType()}: disconnecting connection because it can't process data fast enough.\n" + $"Queue total {num}>{10000}. rcv_queue={kcp.rcv_queue.Count} snd_queue={kcp.snd_queue.Count} rcv_buf={kcp.rcv_buf.Count} snd_buf={kcp.snd_buf.Count}\n" + "* Try to Enable NoDelay, decrease INTERVAL, disable Congestion Window (= enable NOCWND!), increase SEND/RECV WINDOW or compress data.\n* Or perhaps the network is simply too slow on our end, or on the other end.");
				kcp.snd_queue.Clear();
				Disconnect();
			}
		}

		private bool ReceiveNextReliable(out KcpHeaderReliable header, out ArraySegment<byte> message)
		{
			message = default(ArraySegment<byte>);
			header = KcpHeaderReliable.Ping;
			int num = kcp.PeekSize();
			if (num <= 0)
			{
				return false;
			}
			if (num > kcpMessageBuffer.Length)
			{
				OnError(ErrorCode.InvalidReceive, $"{GetType()}: possible allocation attack for msgSize {num} > buffer {kcpMessageBuffer.Length}. Disconnecting the connection.");
				Disconnect();
				return false;
			}
			int num2 = kcp.Receive(kcpMessageBuffer, num);
			if (num2 < 0)
			{
				OnError(ErrorCode.InvalidReceive, $"{GetType()}: Receive failed with error={num2}. closing connection.");
				Disconnect();
				return false;
			}
			byte b = kcpMessageBuffer[0];
			if (!KcpHeader.ParseReliable(b, out header))
			{
				OnError(ErrorCode.InvalidReceive, $"{GetType()}: Receive failed to parse header: {b} is not defined in {typeof(KcpHeaderReliable)}.");
				Disconnect();
				return false;
			}
			message = new ArraySegment<byte>(kcpMessageBuffer, 1, num - 1);
			lastReceiveTime = (uint)watch.ElapsedMilliseconds;
			return true;
		}

		private void TickIncoming_Connected(uint time)
		{
			HandleTimeout(time);
			HandleDeadLink();
			HandlePing(time);
			HandleChoked();
			if (ReceiveNextReliable(out var header, out var _))
			{
				switch (header)
				{
				case KcpHeaderReliable.Hello:
					Log.Info($"[KCP] {GetType()}: received hello with cookie={cookie}");
					state = KcpState.Authenticated;
					OnAuthenticated();
					break;
				case KcpHeaderReliable.Data:
					OnError(ErrorCode.InvalidReceive, $"[KCP] {GetType()}: received invalid header {header} while Connected. Disconnecting the connection.");
					Disconnect();
					break;
				case KcpHeaderReliable.Ping:
					break;
				}
			}
		}

		private void TickIncoming_Authenticated(uint time)
		{
			HandleTimeout(time);
			HandleDeadLink();
			HandlePing(time);
			HandleChoked();
			KcpHeaderReliable header;
			ArraySegment<byte> message;
			while (ReceiveNextReliable(out header, out message))
			{
				switch (header)
				{
				case KcpHeaderReliable.Hello:
					Log.Warning($"{GetType()}: received invalid header {header} while Authenticated. Disconnecting the connection.");
					Disconnect();
					break;
				case KcpHeaderReliable.Data:
					if (message.Count > 0)
					{
						OnData(message, KcpChannel.Reliable);
						break;
					}
					OnError(ErrorCode.InvalidReceive, $"{GetType()}: received empty Data message while Authenticated. Disconnecting the connection.");
					Disconnect();
					break;
				}
			}
		}

		public virtual void TickIncoming()
		{
			uint time = (uint)watch.ElapsedMilliseconds;
			try
			{
				switch (state)
				{
				case KcpState.Connected:
					TickIncoming_Connected(time);
					break;
				case KcpState.Authenticated:
					TickIncoming_Authenticated(time);
					break;
				case KcpState.Disconnected:
					break;
				}
			}
			catch (SocketException arg)
			{
				OnError(ErrorCode.ConnectionClosed, $"{GetType()}: Disconnecting because {arg}. This is fine.");
				Disconnect();
			}
			catch (ObjectDisposedException arg2)
			{
				OnError(ErrorCode.ConnectionClosed, $"{GetType()}: Disconnecting because {arg2}. This is fine.");
				Disconnect();
			}
			catch (Exception arg3)
			{
				OnError(ErrorCode.Unexpected, $"{GetType()}: unexpected Exception: {arg3}");
				Disconnect();
			}
		}

		public virtual void TickOutgoing()
		{
			uint currentTimeMilliSeconds = (uint)watch.ElapsedMilliseconds;
			try
			{
				switch (state)
				{
				case KcpState.Connected:
				case KcpState.Authenticated:
					kcp.Update(currentTimeMilliSeconds);
					break;
				}
			}
			catch (SocketException arg)
			{
				OnError(ErrorCode.ConnectionClosed, $"{GetType()}: Disconnecting because {arg}. This is fine.");
				Disconnect();
			}
			catch (ObjectDisposedException arg2)
			{
				OnError(ErrorCode.ConnectionClosed, $"{GetType()}: Disconnecting because {arg2}. This is fine.");
				Disconnect();
			}
			catch (Exception arg3)
			{
				OnError(ErrorCode.Unexpected, $"{GetType()}: unexpected exception: {arg3}");
				Disconnect();
			}
		}

		protected void OnRawInputReliable(ArraySegment<byte> message)
		{
			int num = kcp.Input(message.Array, message.Offset, message.Count);
			if (num != 0)
			{
				Log.Warning($"[KCP] {GetType()}: Input failed with error={num} for buffer with length={message.Count - 1}");
			}
		}

		protected void OnRawInputUnreliable(ArraySegment<byte> message)
		{
			if (message.Count < 1)
			{
				return;
			}
			byte b = message.Array[message.Offset];
			if (!KcpHeader.ParseUnreliable(b, out var header))
			{
				OnError(ErrorCode.InvalidReceive, $"{GetType()}: Receive failed to parse header: {b} is not defined in {typeof(KcpHeaderUnreliable)}.");
				Disconnect();
				return;
			}
			message = new ArraySegment<byte>(message.Array, message.Offset + 1, message.Count - 1);
			switch (header)
			{
			case KcpHeaderUnreliable.Data:
				if (state == KcpState.Authenticated)
				{
					OnData(message, KcpChannel.Unreliable);
					lastReceiveTime = (uint)watch.ElapsedMilliseconds;
				}
				break;
			case KcpHeaderUnreliable.Disconnect:
				Log.Info($"[KCP] {GetType()}: received disconnect message");
				Disconnect();
				break;
			}
		}

		private void RawSendReliable(byte[] data, int length)
		{
			rawSendBuffer[0] = 1;
			Utils.Encode32U(rawSendBuffer, 1, cookie);
			Buffer.BlockCopy(data, 0, rawSendBuffer, 5, length);
			ArraySegment<byte> data2 = new ArraySegment<byte>(rawSendBuffer, 0, length + 1 + 4);
			RawSend(data2);
		}

		private void SendReliable(KcpHeaderReliable header, ArraySegment<byte> content)
		{
			if (1 + content.Count > kcpSendBuffer.Length)
			{
				OnError(ErrorCode.InvalidSend, $"{GetType()}: Failed to send reliable message of size {content.Count} because it's larger than ReliableMaxMessageSize={reliableMax}");
				return;
			}
			kcpSendBuffer[0] = (byte)header;
			if (content.Count > 0)
			{
				Buffer.BlockCopy(content.Array, content.Offset, kcpSendBuffer, 1, content.Count);
			}
			int num = kcp.Send(kcpSendBuffer, 0, 1 + content.Count);
			if (num < 0)
			{
				OnError(ErrorCode.InvalidSend, $"{GetType()}: Send failed with error={num} for content with length={content.Count}");
			}
		}

		private void SendUnreliable(KcpHeaderUnreliable header, ArraySegment<byte> content)
		{
			if (content.Count > unreliableMax)
			{
				Log.Error($"[KCP] {GetType()}: Failed to send unreliable message of size {content.Count} because it's larger than UnreliableMaxMessageSize={unreliableMax}");
				return;
			}
			rawSendBuffer[0] = 2;
			Utils.Encode32U(rawSendBuffer, 1, cookie);
			rawSendBuffer[5] = (byte)header;
			if (content.Count > 0)
			{
				Buffer.BlockCopy(content.Array, content.Offset, rawSendBuffer, 6, content.Count);
			}
			ArraySegment<byte> data = new ArraySegment<byte>(rawSendBuffer, 0, content.Count + 1 + 4 + 1);
			RawSend(data);
		}

		public void SendHello()
		{
			Log.Info($"[KCP] {GetType()}: sending handshake to other end with cookie={cookie}");
			SendReliable(KcpHeaderReliable.Hello, default(ArraySegment<byte>));
		}

		public void SendData(ArraySegment<byte> data, KcpChannel channel)
		{
			if (data.Count == 0)
			{
				OnError(ErrorCode.InvalidSend, $"{GetType()}: tried sending empty message. This should never happen. Disconnecting.");
				Disconnect();
				return;
			}
			switch (channel)
			{
			case KcpChannel.Reliable:
				SendReliable(KcpHeaderReliable.Data, data);
				break;
			case KcpChannel.Unreliable:
				SendUnreliable(KcpHeaderUnreliable.Data, data);
				break;
			}
		}

		private void SendPing()
		{
			SendReliable(KcpHeaderReliable.Ping, default(ArraySegment<byte>));
		}

		private void SendDisconnect()
		{
			for (int i = 0; i < 5; i++)
			{
				SendUnreliable(KcpHeaderUnreliable.Disconnect, default(ArraySegment<byte>));
			}
		}

		public virtual void Disconnect()
		{
			if (state != KcpState.Disconnected)
			{
				try
				{
					SendDisconnect();
				}
				catch (SocketException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
				Log.Info($"[KCP] {GetType()}: Disconnected.");
				state = KcpState.Disconnected;
				OnDisconnected();
			}
		}
	}
}
