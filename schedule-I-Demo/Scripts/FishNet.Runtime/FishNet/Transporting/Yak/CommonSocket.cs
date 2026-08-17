using System.Collections.Generic;

namespace FishNet.Transporting.Yak
{
	public abstract class CommonSocket
	{
		private LocalConnectionState _connectionState;

		protected Transport Transport;

		internal LocalConnectionState GetLocalConnectionState()
		{
			return _connectionState;
		}

		protected virtual void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			if (connectionState != _connectionState)
			{
				_connectionState = connectionState;
				if (server)
				{
					Transport.HandleServerConnectionState(new ServerConnectionStateArgs(connectionState, Transport.Index));
				}
				else
				{
					Transport.HandleClientConnectionState(new ClientConnectionStateArgs(connectionState, Transport.Index));
				}
			}
		}

		internal virtual void Initialize(Transport t, CommonSocket socket)
		{
			Transport = t;
		}

		internal void ClearQueue(ref Queue<LocalPacket> queue)
		{
			while (queue.Count > 0)
			{
				queue.Dequeue().Dispose();
			}
		}
	}
}
