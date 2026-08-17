using System;

namespace EvilAnalytics.Shared.Leaderboards
{
	public class RankingEntry
	{
		public int Rank { get; set; }

		public Guid PlayerId { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public decimal Score { get; set; }

		public int SubmissionCount { get; set; }

		public DateTimeOffset LastSubmittedAt { get; set; }
	}
}
