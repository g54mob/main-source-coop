namespace EvilAnalytics.Shared.Leaderboards
{
	public class SubmitScoreResponse
	{
		public bool Accepted { get; set; }

		public decimal NewScore { get; set; }

		public int? NewRank { get; set; }

		public bool IsNewBest { get; set; }
	}
}
