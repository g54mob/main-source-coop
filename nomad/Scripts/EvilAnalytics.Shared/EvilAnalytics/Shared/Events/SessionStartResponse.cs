using System;

namespace EvilAnalytics.Shared.Events
{
	public class SessionStartResponse
	{
		public Guid SessionId { get; set; }

		public Guid PlayerId { get; set; }

		public bool IsNewPlayer { get; set; }

		public DateTimeOffset ServerTime { get; set; }
	}
}
