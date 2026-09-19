using Fusion;
using Fusion.Sockets;

namespace NetworkServices.NetworkEvents
{
	public class OnConnectFailedEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetAddress RemoteAddress;

		public readonly NetConnectFailedReason Reason;

		public OnConnectFailedEvent(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			Runner = runner;
			RemoteAddress = remoteAddress;
			Reason = reason;
		}
	}
}
