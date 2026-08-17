using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class CrashLogQuery
	{
		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 20;

		public LogSeverity? Severity { get; set; }

		public string Search { get; set; }

		public string GameVersion { get; set; }

		public DateTimeOffset? FromDate { get; set; }

		public DateTimeOffset? ToDate { get; set; }

		public string SortBy { get; set; } = "lastOccurrence";

		public bool SortDescending { get; set; } = true;
	}
}
