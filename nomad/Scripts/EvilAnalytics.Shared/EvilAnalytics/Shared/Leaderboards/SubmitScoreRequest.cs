namespace EvilAnalytics.Shared.Leaderboards
{
	public class SubmitScoreRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		public decimal Score { get; set; }
	}
}
