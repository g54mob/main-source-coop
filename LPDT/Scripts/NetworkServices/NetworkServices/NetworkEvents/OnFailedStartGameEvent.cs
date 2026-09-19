using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnFailedStartGameEvent : NetworkRunnerEvent
	{
		public readonly string ErrorMessage;

		public readonly ShutdownReason ShutdownReason;

		public readonly string StackTrace;

		public OnFailedStartGameEvent(string errorMessage, ShutdownReason shutdownReason, string stackTrace)
		{
			ErrorMessage = errorMessage;
			ShutdownReason = shutdownReason;
			StackTrace = stackTrace;
		}
	}
}
