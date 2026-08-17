using System;

namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportResponse
	{
		public Guid BugReportId { get; set; }

		public DateTimeOffset ServerTime { get; set; }

		public bool Success { get; set; }
	}
}
