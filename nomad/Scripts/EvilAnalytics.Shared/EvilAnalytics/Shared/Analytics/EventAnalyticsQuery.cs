namespace EvilAnalytics.Shared.Analytics
{
	public class EventAnalyticsQuery : AnalyticsQueryBase
	{
		public string? EventName { get; set; }

		public string? Category { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 50;

		public string? SortBy { get; set; }

		public bool SortDescending { get; set; } = true;

		public string EffectiveSortBy => SortBy ?? "totalCount";
	}
}
