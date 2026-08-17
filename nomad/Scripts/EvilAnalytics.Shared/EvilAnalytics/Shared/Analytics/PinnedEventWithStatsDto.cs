using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class PinnedEventWithStatsDto : PinnedEventDto
	{
		public long TotalCount { get; set; }

		public int UniquePlayers { get; set; }

		public int UniqueSessions { get; set; }

		public decimal? TotalValue { get; set; }

		public DateTimeOffset? LastTriggered { get; set; }
	}
}
