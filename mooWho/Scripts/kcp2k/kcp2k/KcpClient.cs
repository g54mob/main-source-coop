using System;
using System.Net;
using System.Net.Sockets;

namespace kcp2k
{
	public class KcpClient : KcpPeer
	{
		protected Socket socket;

		public EndPoint remoteEndPoint;

		protected readonly KcpConfig config;

		protected readonly byte[] rawReceiveBuffer;

		protected readonly Action OnConnectedCallback;

		protected readonly Action<ArraySegment<byte>, KcpChannel> OnDataCallback;

		protected readonly Action OnDisconnectedCallback;

		protected readonly Action<ErrorCode, string> OnErrorCallback;

		private bool active;

		public bool connected;

		public EndPoint LocalEndPoint => socket?.LocalEndPoint;

		public KcpClient(Action OnConnected, Action<ArraySegment<byte>, KcpChannel> OnData, Action OnDisconnected, Action<ErrorCode, string> OnError, KcpConfig config)
			: base(config, 0u)
		{
			OnConnectedCallback = OnConnected;
			OnDataCallback = OnData;
			OnDisconnectedCallback = OnDisconnected;
			OnErrorCallback = OnError;
			this.config = config;
			rawReceiveBuffer = new byte[config.Mtu];
		}

		protected override void OnAuthenticated()
		{
			Log.Info("[KCP] Client: OnConnected");
			connected = true;
			OnConnectedCallback();
		}

		protected override void OnData(ArraySegment<byte> message, KcpChannel channel)
		{
			OnDataCallback(message, channel);
		}

		protected override void OnError(ErrorCode error, string message)
		{
			OnErrorCallback(error, message);
		}

		protected override void OnDisconnected()
		{
			Log.Info("[KCP] Client: OnDisconnected");
			connected = false;
			socket?.Close();
			socket = null;
			remoteEndPoint = null;
			OnDisconnectedCallback();
			active = false;
		}

		public void Connect(string address, ushort port)
		{
			if (connected)
			{
				Log.Warning("[KCP] Client: already connected!");
				return;
			}
			if (!Common.ResolveHostname(address, out var addresses))
			{
				OnError(ErrorCode.DnsResolve, "Failed to resolve host: " + address);
				OnDisconnectedCallback();
				return;
			}
			Reset(config);
			Log.Info($"[KCP] Client: connect to {address}:{port}");
			remoteEndPoint = new IPEndPoint(addresses[0], port);
			socket = new Socket(remoteEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			active = true;
			socket.Blocking = false;
			Common.ConfigureSocketBuffers(socket, config.RecvBufferSize, config.SendBufferSize);
			socket.Connect(remoteEndPoint);
			SendHello();
		}

		protected virtual bool RawReceive(out ArraySegment<byte> segment)
		{
			segment = default(ArraySegment<byte>);
			if (socket == null)
			{
				return false;
			}
			try
			{
				return socket.ReceiveNonBlocking(rawReceiveBuffer, out segment);
			}
			catch (SocketException arg)
			{
				Log.Info($"[KCP] Client.RawReceive: looks like the other end has closed the connection. This is fine: {arg}");
				base.Disconnect();
				return false;
			}
		}

		protected override void RawSend(ArraySegment<byte> data)
		{
			if (socket == null)
			{
				return;
			}
			try
			{
				socket.SendNonBlocking(data);
			}
			catch (SocketException arg)
			{
				Log.Info($"[KCP] Client.RawSend: looks like the other end has closed the connection. This is fine: {arg}");
			}
		}

		public void Send(ArraySegment<byte> segment, KcpChannel channel)
		{
			if (!connected)
			{
				Log.Warning("[KCP] Client: can't send because not connected!");
			}
			else
			{
				SendData(segment, channel);
			}
		}

		public void RawInput(ArraySegment<byte> segment)
		{
			if (segment.Count > 5)
			{
				byte b = segment.Array[segment.Offset];
				Utils.Decode32U(segment.Array, segment.Offset + 1, out var value);
				if (value == 0)
				{
					Log.Error("[KCP] Client: received message with cookie=0, this should never happen. Server should always include the security cookie.");
				}
				if (cookie == 0)
				{
					cookie = value;
					Log.Info($"[KCP] Client: received initial cookie: {cookie}");
				}
				else if (cookie != value)
				{
					Log.Warning($"[KCP] Client: dropping message with mismatching cookie: {value} expected: {cookie}.");
					return;
				}
				ArraySegment<byte> message = new ArraySegment<byte>(segment.Array, segment.Offset + 1 + 4, segment.Count - 1 - 4);
				switch (b)
				{
				case 1:
					OnRawInputReliable(message);
					break;
				case 2:
					OnRawInputUnreliable(message);
					break;
				default:
					Log.Warning($"[KCP] Client: invalid channel header: {b}, likely internet noise");
					break;
				}
			}
		}

		public override void TickIncoming()
		{
			if (active)
			{
				ArraySegment<byte> segment;
				while (RawReceive(out segment))
				{
					RawInput(segment);
				}
			}
			if (active)
			{
				base.TickIncoming();
			}
		}

		public override void TickOutgoing()
		{
			if (active)
			{
				base.TickOutgoing();
			}
		}

		public virtual void Tick()
		{
			TickIncoming();
			TickOutgoing();
		}
	}
}
