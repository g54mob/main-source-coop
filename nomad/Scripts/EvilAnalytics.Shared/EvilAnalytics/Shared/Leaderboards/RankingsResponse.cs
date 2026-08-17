using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Leaderboards
{
	public class RankingsResponse
	{
		public Guid LeaderboardId { get; set; }

		public string LeaderboardName { get; set; } = string.Empty;

		public string TimeWindow { get; set; } = "AllTime";

		public List<RankingEntry> Rankings { get; set; } = new List<RankingEntry>();

		public int TotalPlayers { get; set; }
	}
}
