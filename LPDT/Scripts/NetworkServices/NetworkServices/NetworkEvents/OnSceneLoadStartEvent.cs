using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnSceneLoadStartEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public OnSceneLoadStartEvent(NetworkRunner runner)
		{
			Runner = runner;
		}
	}
}
