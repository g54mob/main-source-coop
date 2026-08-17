using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class LogEntry
	{
		public string Message { get; set; } = string.Empty;

		public string StackTrace { get; set; } = string.Empty;

		public LogSeverity Severity { get; set; }

		public DateTimeOffset Timestamp { get; set; }

		public string SceneName { get; set; }

		public float? CurrentFps { get; set; }

		public float? MemoryUsedMb { get; set; }
	}
}
