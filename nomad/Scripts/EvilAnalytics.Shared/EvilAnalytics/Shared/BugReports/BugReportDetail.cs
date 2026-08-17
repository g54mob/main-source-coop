using System;
using System.Collections.Generic;
using EvilAnalytics.Shared.Crashlytics;

namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportDetail : BugReportSummary
	{
		public Guid SessionId { get; set; }

		public float? AvgFps { get; set; }

		public float? MinFps { get; set; }

		public float? MemoryUsedMb { get; set; }

		public string GameVersion { get; set; }

		public string AdminNotes { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }

		public DateTimeOffset? ResolvedAt { get; set; }

		public SystemResourceSnapshot SystemResources { get; set; }

		public List<LogEntry> RecentLogs { get; set; }

		public string FullReportJson { get; set; }
	}
}
