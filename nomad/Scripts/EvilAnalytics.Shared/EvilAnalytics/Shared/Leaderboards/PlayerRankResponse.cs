using System.Collections.Generic;

namespace EvilAnalytics.Shared.Leaderboards
{
	public class PlayerRankResponse
	{
		public RankingEntry Player { get; set; }

		public List<RankingEntry> NearbyPlayers { get; set; } = new List<RankingEntry>();

		public int TotalPlayers { get; set; }
	}
}
