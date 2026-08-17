namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportQuery
	{
		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 20;

		public BugReportStatus? Status { get; set; }

		public BugReportPriority? Priority { get; set; }

		public string Search { get; set; }

		public string SortBy { get; set; } = "createdAt";

		public bool SortDescending { get; set; } = true;
	}
}
