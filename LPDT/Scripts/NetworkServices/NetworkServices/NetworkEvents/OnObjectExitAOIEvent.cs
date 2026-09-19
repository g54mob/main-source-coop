using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnObjectExitAOIEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkObject Object;

		public readonly PlayerRef Player;

		public OnObjectExitAOIEvent(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			Runner = runner;
			Object = obj;
			Player = player;
		}
	}
}
