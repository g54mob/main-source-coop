using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnPlayerJoinedEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly PlayerRef Player;

		public OnPlayerJoinedEvent(NetworkRunner runner, PlayerRef player)
		{
			Runner = runner;
			Player = player;
		}
	}
}
