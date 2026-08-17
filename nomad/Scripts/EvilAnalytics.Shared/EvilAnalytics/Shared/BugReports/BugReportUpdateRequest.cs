namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportUpdateRequest
	{
		public BugReportStatus? Status { get; set; }

		public BugReportPriority? Priority { get; set; }

		public string AdminNotes { get; set; }
	}
}
