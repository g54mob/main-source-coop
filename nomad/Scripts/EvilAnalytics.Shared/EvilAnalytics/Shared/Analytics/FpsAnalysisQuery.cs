namespace EvilAnalytics.Shared.Analytics
{
	public class FpsAnalysisQuery
	{
		public string? SortBy { get; set; } = "avgFps";

		public string? SortOrder { get; set; } = "desc";

		public int? MinAvgFps { get; set; }

		public int? MaxAvgFps { get; set; }

		public string? Platform { get; set; }

		public string? GpuVendor { get; set; }

		public int? MinMemoryMB { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 20;
	}
}
