using Fusion;
using Fusion.Sockets;

namespace NetworkServices.NetworkEvents
{
	public class OnDisconnectedFromServerEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetDisconnectReason Reason;

		public OnDisconnectedFromServerEvent(NetworkRunner runner, NetDisconnectReason reason)
		{
			Runner = runner;
			Reason = reason;
		}
	}
}
