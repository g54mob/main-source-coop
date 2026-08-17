using System;
using System.Collections.Generic;
using FishNet.Transporting.Yak.Server;

namespace FishNet.Transporting.Yak.Client
{
	public class ClientSocket : CommonSocket
	{
		private ServerSocket _server;

		private Queue<LocalPacket> _incoming = new Queue<LocalPacket>();

		internal override void Initialize(Transport t, CommonSocket socket)
		{
			base.Initialize(t, socket);
			_server = (ServerSocket)socket;
		}

		internal bool StartConnection()
		{
			if (GetLocalConnectionState() != LocalConnectionState.Stopped)
			{
				return false;
			}
			SetLocalConnectionState(LocalConnectionState.Starting, server: false);
			LocalConnectionState localConnectionState = _server.GetLocalConnectionState();
			if (localConnectionState == LocalConnectionState.Stopping || localConnectionState == LocalConnectionState.Started)
			{
				OnLocalServerConnectionState(_server.GetLocalConnectionState());
			}
			return true;
		}

		protected override void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			base.SetLocalConnectionState(connectionState, server);
			if (connectionState == LocalConnectionState.Started || connectionState == LocalConnectionState.Stopped)
			{
				_server.OnLocalClientConnectionState(connectionState);
			}
		}

		internal bool StopConnection()
		{
			if (GetLocalConnectionState() == LocalConnectionState.Stopped || GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return false;
			}
			ClearQueue(ref _incoming);
			SetLocalConnectionState(LocalConnectionState.Stopping, server: false);
			SetLocalConnectionState(LocalConnectionState.Stopped, server: false);
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
					ClientReceivedDataArgs receivedDataArgs = new ClientReceivedDataArgs(data, (Channel)localPacket.Channel, Transport.Index);
					Transport.HandleClientReceivedDataArgs(receivedDataArgs);
					localPacket.Dispose();
				}
			}
		}

		internal void ReceivedFromLocalServer(LocalPacket packet)
		{
			_incoming.Enqueue(packet);
		}

		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (GetLocalConnectionState() == LocalConnectionState.Started && _server.GetLocalConnectionState() == LocalConnectionState.Started)
			{
				LocalPacket packet = new LocalPacket(segment, channelId);
				_server.ReceivedFromLocalClient(packet);
			}
		}

		internal void OnLocalServerConnectionState(LocalConnectionState state)
		{
			if (state == LocalConnectionState.Started && GetLocalConnectionState() == LocalConnectionState.Starting)
			{
				SetLocalConnectionState(LocalConnectionState.Started, server: false);
			}
			else if ((state == LocalConnectionState.Stopping || state == LocalConnectionState.Stopped) && (GetLocalConnectionState() == LocalConnectionState.Started || GetLocalConnectionState() == LocalConnectionState.Starting))
			{
				SetLocalConnectionState(LocalConnectionState.Stopping, server: false);
				SetLocalConnectionState(LocalConnectionState.Stopped, server: false);
			}
		}
	}
}
