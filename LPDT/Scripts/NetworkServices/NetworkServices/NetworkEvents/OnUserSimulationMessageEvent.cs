using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnUserSimulationMessageEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly SimulationMessagePtr Message;

		public OnUserSimulationMessageEvent(NetworkRunner runner, SimulationMessagePtr message)
		{
			Runner = runner;
			Message = message;
		}
	}
}
