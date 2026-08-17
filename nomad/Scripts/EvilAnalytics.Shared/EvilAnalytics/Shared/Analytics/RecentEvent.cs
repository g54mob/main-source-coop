using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class RecentEvent
	{
		public string EventName { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public DateTimeOffset Timestamp { get; set; }

		public Guid PlayerId { get; set; }
	}
}
