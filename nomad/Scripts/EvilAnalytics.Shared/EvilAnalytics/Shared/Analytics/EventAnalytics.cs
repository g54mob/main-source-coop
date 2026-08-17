using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class EventAnalytics
	{
		public string EventName { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public long TotalCount { get; set; }

		public int UniquePlayers { get; set; }

		public int UniqueSessions { get; set; }

		public decimal? AverageValue { get; set; }

		public decimal? SumValue { get; set; }

		public DateTimeOffset FirstOccurrence { get; set; }

		public DateTimeOffset LastOccurrence { get; set; }
	}
}
