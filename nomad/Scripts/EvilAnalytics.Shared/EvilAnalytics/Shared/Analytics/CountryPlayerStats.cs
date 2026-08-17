namespace EvilAnalytics.Shared.Analytics
{
	public class CountryPlayerStats
	{
		public string CountryCode { get; set; } = string.Empty;

		public string CountryName { get; set; } = string.Empty;

		public string? ContinentCode { get; set; }

		public int TotalPlayers { get; set; }

		public int OnlinePlayers { get; set; }

		public int NewPlayers { get; set; }

		public double AvgSessionDurationSeconds { get; set; }

		public long TotalPlayTimeSeconds { get; set; }
	}
}
