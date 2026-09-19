using System;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionRecoveryRequestedException : Exception
	{
		public RecoveryReason Reason { get; }

		public SessionRecoveryRequestedException(RecoveryReason reason)
			: base($"Session recovery requested: {reason}")
		{
			Reason = reason;
		}
	}
}
