using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnConnectedToServerEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public OnConnectedToServerEvent(NetworkRunner runner)
		{
			Runner = runner;
		}
	}
}
