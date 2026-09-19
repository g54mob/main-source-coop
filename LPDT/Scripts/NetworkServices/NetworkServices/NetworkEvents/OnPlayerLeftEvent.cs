using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnPlayerLeftEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly PlayerRef Player;

		public OnPlayerLeftEvent(NetworkRunner runner, PlayerRef player)
		{
			Runner = runner;
			Player = player;
		}
	}
}
