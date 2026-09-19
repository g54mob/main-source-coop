using System;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionRosterRejectedException : Exception
	{
		public SessionRosterRejectedException(string message)
			: base(message)
		{
		}
	}
}
