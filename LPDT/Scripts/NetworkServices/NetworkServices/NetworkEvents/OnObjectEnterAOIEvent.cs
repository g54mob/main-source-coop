using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnObjectEnterAOIEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkObject Object;

		public readonly PlayerRef Player;

		public OnObjectEnterAOIEvent(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			Runner = runner;
			Object = obj;
			Player = player;
		}
	}
}
