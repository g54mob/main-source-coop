using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnInputMissingEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly PlayerRef Player;

		public readonly NetworkInput Input;

		public OnInputMissingEvent(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
			Runner = runner;
			Player = player;
			Input = input;
		}
	}
}
