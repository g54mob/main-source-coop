using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class RealTimeMetrics
	{
		public DateTimeOffset Timestamp { get; set; }

		public int ActivePlayers { get; set; }

		public int ActiveSessions { get; set; }

		public double EventsPerMinute { get; set; }

		public List<RecentEvent> RecentEvents { get; set; } = new List<RecentEvent>();
	}
}
