using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace kcp2k
{
	public class KcpServer
	{
		protected readonly Action<int, IPEndPoint> OnConnected;

		protected readonly Action<int, ArraySegment<byte>, KcpChannel> OnData;

		protected readonly Action<int> OnDisconnected;

		protected readonly Action<int, ErrorCode, string> OnError;

		protected readonly KcpConfig config;

		protected Socket socket;

		private EndPoint newClientEP;

		protected readonly byte[] rawReceiveBuffer;

		public Dictionary<int, KcpServerConnection> connections = new Dictionary<int, KcpServerConnection>();

		private readonly HashSet<int> connectionsToRemove = new HashSet<int>();

		public EndPoint LocalEndPoint => socket?.LocalEndPoint;

		public KcpServer(Action<int, IPEndPoint> OnConnected, Action<int, ArraySegment<byte>, KcpChannel> OnData, Action<int> OnDisconnected, Action<int, ErrorCode, string> OnError, KcpConfig config)
		{
			this.OnConnected = OnConnected;
			this.OnData = OnData;
			this.OnDisconnected = OnDisconnected;
			this.OnError = OnError;
			this.config = config;
			rawReceiveBuffer = new byte[config.Mtu];
			newClientEP = (config.DualMode ? new IPEndPoint(IPAddress.IPv6Any, 0) : new IPEndPoint(IPAddress.Any, 0));
		}

		public virtual bool IsActive()
		{
			return socket != null;
		}

		private static Socket CreateServerSocket(bool DualMode, ushort port)
		{
			if (DualMode)
			{
				Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
				try
				{
					socket.DualMode = true;
				}
				catch (NotSupportedException arg)
				{
					Log.Warning($"[KCP] Failed to set Dual Mode, continuing with IPv6 without Dual Mode. Error: {arg}");
				}
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					socket.IOControl(-1744830452, new byte[1], null);
				}
				socket.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
				return socket;
			}
			Socket obj = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			obj.Bind(new IPEndPoint(IPAddress.Any, port));
			return obj;
		}

		public virtual void Start(ushort port)
		{
			if (socket != null)
			{
				Log.Warning("[KCP] Server: already started!");
				return;
			}
			socket = CreateServerSocket(config.DualMode, port);
			socket.Blocking = false;
			Common.ConfigureSocketBuffers(socket, config.RecvBufferSize, config.SendBufferSize);
		}

		public void Send(int connectionId, ArraySegment<byte> segment, KcpChannel channel)
		{
			if (connections.TryGetValue(connectionId, out var value))
			{
				value.SendData(segment, channel);
			}
		}

		public void Disconnect(int connectionId)
		{
			if (connections.TryGetValue(connectionId, out var value))
			{
				value.Disconnect();
			}
		}

		public IPEndPoint GetClientEndPoint(int connectionId)
		{
			if (connections.TryGetValue(connectionId, out var value))
			{
				return value.remoteEndPoint as IPEndPoint;
			}
			return null;
		}

		protected virtual bool RawReceiveFrom(out ArraySegment<byte> segment, out int connectionId)
		{
			segment = default(ArraySegment<byte>);
			connectionId = 0;
			if (socket == null)
			{
				return false;
			}
			try
			{
				if (socket.ReceiveFromNonBlocking(rawReceiveBuffer, out segment, ref newClientEP))
				{
					connectionId = Common.ConnectionHash(newClientEP);
					return true;
				}
			}
			catch (SocketException arg)
			{
				Log.Info($"[KCP] Server: ReceiveFrom failed: {arg}");
			}
			return false;
		}

		protected virtual void RawSend(int connectionId, ArraySegment<byte> data)
		{
			if (!connections.TryGetValue(connectionId, out var value))
			{
				Log.Warning($"[KCP] Server: RawSend invalid connectionId={connectionId}");
				return;
			}
			try
			{
				socket.SendToNonBlocking(data, value.remoteEndPoint);
			}
			catch (SocketException arg)
			{
				Log.Error($"[KCP] Server: SendTo failed: {arg}");
			}
		}

		protected virtual KcpServerConnection CreateConnection(int connectionId)
		{
			uint cookie = Common.GenerateCookie();
			return new KcpServerConnection(OnConnectedCallback, delegate(ArraySegment<byte> message, KcpChannel channel)
			{
				OnData(connectionId, message, channel);
			}, OnDisconnectedCallback, delegate(ErrorCode error, string reason)
			{
				OnError(connectionId, error, reason);
			}, delegate(ArraySegment<byte> data)
			{
				RawSend(connectionId, data);
			}, config, cookie, newClientEP);
			void OnConnectedCallback(KcpServerConnection conn)
			{
				connections.Add(connectionId, conn);
				Log.Info($"[KCP] Server: added connection({connectionId})");
				Log.Info($"[KCP] Server: OnConnected({connectionId})");
				IPEndPoint arg = conn.remoteEndPoint as IPEndPoint;
				OnConnected(connectionId, arg);
			}
			void OnDisconnectedCallback()
			{
				connectionsToRemove.Add(connectionId);
				Log.Info($"[KCP] Server: OnDisconnected({connectionId})");
				OnDisconnected(connectionId);
			}
		}

		private void ProcessMessage(ArraySegment<byte> segment, int connectionId)
		{
			if (!connections.TryGetValue(connectionId, out var value))
			{
				value = CreateConnection(connectionId);
				value.RawInput(segment);
				value.TickIncoming();
			}
			else
			{
				value.RawInput(segment);
			}
		}

		public virtual void TickIncoming()
		{
			ArraySegment<byte> segment;
			int connectionId;
			while (RawReceiveFrom(out segment, out connectionId))
			{
				ProcessMessage(segment, connectionId);
			}
			foreach (KcpServerConnection value in connections.Values)
			{
				value.TickIncoming();
			}
			foreach (int item in connectionsToRemove)
			{
				connections.Remove(item);
			}
			connectionsToRemove.Clear();
		}

		public virtual void TickOutgoing()
		{
			foreach (KcpServerConnection value in connections.Values)
			{
				value.TickOutgoing();
			}
		}

		public virtual void Tick()
		{
			TickIncoming();
			TickOutgoing();
		}

		public virtual void Stop()
		{
			connections.Clear();
			socket?.Close();
			socket = null;
		}
	}
}
