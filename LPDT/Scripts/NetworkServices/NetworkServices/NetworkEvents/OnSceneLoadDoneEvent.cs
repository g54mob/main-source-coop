using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnSceneLoadDoneEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public OnSceneLoadDoneEvent(NetworkRunner runner)
		{
			Runner = runner;
		}
	}
}
