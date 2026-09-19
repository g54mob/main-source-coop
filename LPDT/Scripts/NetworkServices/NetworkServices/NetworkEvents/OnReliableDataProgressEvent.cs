using Fusion;
using Fusion.Sockets;

namespace NetworkServices.NetworkEvents
{
	public class OnReliableDataProgressEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly PlayerRef Player;

		public readonly ReliableKey Key;

		public readonly float Progress;

		public OnReliableDataProgressEvent(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
		{
			Runner = runner;
			Player = player;
			Key = key;
			Progress = progress;
		}
	}
}
