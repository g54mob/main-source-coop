using System;
using System.Collections.Generic;
using EvilAnalytics.Shared.Crashlytics;

namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		public Guid SessionId { get; set; }

		public string Message { get; set; } = string.Empty;

		public float? CurrentFps { get; set; }

		public float? AvgFps { get; set; }

		public float? MinFps { get; set; }

		public float? MemoryUsedMb { get; set; }

		public string SceneName { get; set; }

		public string GameVersion { get; set; }

		public DateTimeOffset Timestamp { get; set; }

		public SystemResourceSnapshot SystemResources { get; set; }

		public List<LogEntry> RecentLogs { get; set; }

		public string FullReportJson { get; set; }
	}
}
