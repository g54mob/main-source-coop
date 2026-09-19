using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnInputEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkInput Input;

		public OnInputEvent(NetworkRunner runner, NetworkInput input)
		{
			Runner = runner;
			Input = input;
		}
	}
}
