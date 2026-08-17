namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerListQuery : AnalyticsQueryBase
	{
		public string? Search { get; set; }

		public string? Platform { get; set; }

		public bool? IsAnonymized { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 50;

		public string SortBy { get; set; } = "lastSeen";

		public bool SortDescending { get; set; } = true;
	}
}
