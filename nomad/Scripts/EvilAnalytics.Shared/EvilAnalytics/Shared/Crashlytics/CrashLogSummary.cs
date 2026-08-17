using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class CrashLogSummary
	{
		public Guid Id { get; set; }

		public Guid PlayerId { get; set; }

		public string PlayerDeviceId { get; set; } = string.Empty;

		public LogSeverity Severity { get; set; }

		public string Message { get; set; } = string.Empty;

		public string SceneName { get; set; }

		public string Platform { get; set; }

		public string GameVersion { get; set; }

		public int OccurrenceCount { get; set; }

		public DateTimeOffset FirstOccurrence { get; set; }

		public DateTimeOffset LastOccurrence { get; set; }
	}
}
