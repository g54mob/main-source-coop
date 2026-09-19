using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnShutdownEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly ShutdownReason Reason;

		public OnShutdownEvent(NetworkRunner runner, ShutdownReason reason)
		{
			Runner = runner;
			Reason = reason;
		}
	}
}
