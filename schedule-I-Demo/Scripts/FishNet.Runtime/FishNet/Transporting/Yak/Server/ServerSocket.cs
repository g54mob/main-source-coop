using System;
using System.Collections.Generic;
using FishNet.Transporting.Yak.Client;

namespace FishNet.Transporting.Yak.Server
{
	public class ServerSocket : CommonSocket
	{
		private Queue<LocalPacket> _incoming = new Queue<LocalPacket>();

		private ClientSocket _client;

		internal RemoteConnectionState GetConnectionState(int connectionId)
		{
			if (connectionId != 32767)
			{
				return RemoteConnectionState.Stopped;
			}
			if (_client.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return RemoteConnectionState.Stopped;
			}
			return RemoteConnectionState.Started;
		}

		internal override void Initialize(Transport t, CommonSocket socket)
		{
			base.Initialize(t, socket);
			_client = (ClientSocket)socket;
		}

		internal bool StartConnection()
		{
			SetLocalConnectionState(LocalConnectionState.Starting, server: true);
			SetLocalConnectionState(LocalConnectionState.Started, server: true);
			return true;
		}

		protected override void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			base.SetLocalConnectionState(connectionState, server);
			_client.OnLocalServerConnectionState(connectionState);
		}

		internal bool StopConnection()
		{
			if (GetLocalConnectionState() == LocalConnectionState.Stopped)
			{
				return false;
			}
			ClearQueue(ref _incoming);
			SetLocalConnectionState(LocalConnectionState.Stopping, server: true);
			SetLocalConnectionState(LocalConnectionState.Stopped, server: true);
			return true;
		}

		internal bool StopConnection(int connectionId)
		{
			if (connectionId != 32767)
			{
				return false;
			}
			_client.StopConnection();
			return true;
		}

		internal void IterateIncoming()
		{
			if (GetLocalConnectionState() == LocalConnectionState.Started)
			{
				while (_incoming.Count > 0)
				{
					LocalPacket localPacket = _incoming.Dequeue();
					ArraySegment<byte> data = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
					ServerReceivedDataArgs receivedDataArgs = new ServerReceivedDataArgs(data, (Channel)localPacket.Channel, 32767, Transport.Index);
					Transport.HandleServerReceivedDataArgs(receivedDataArgs);
				}
			}
		}

		internal void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			if (GetLocalConnectionState() == LocalConnectionState.Started && connectionId == 32767)
			{
				LocalPacket packet = new LocalPacket(segment, channelId);
				_client.ReceivedFromLocalServer(packet);
			}
		}

		internal void OnLocalClientConnectionState(LocalConnectionState state)
		{
			if (state != LocalConnectionState.Started)
			{
				ClearQueue(ref _incoming);
				if (state == LocalConnectionState.Stopped)
				{
					Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Stopped, 32767, Transport.Index));
				}
			}
			else
			{
				Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Started, 32767, Transport.Index));
			}
		}

		internal void ReceivedFromLocalClient(LocalPacket packet)
		{
			if (_client.GetLocalConnectionState() == LocalConnectionState.Started)
			{
				_incoming.Enqueue(packet);
			}
		}
	}
}
