namespace EvilAnalytics.Shared.Analytics
{
	public class ContinentStats
	{
		public string ContinentCode { get; set; } = string.Empty;

		public string ContinentName { get; set; } = string.Empty;

		public int PlayerCount { get; set; }

		public int OnlineCount { get; set; }

		public double Percentage { get; set; }
	}
}
