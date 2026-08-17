using System;

namespace EvilAnalytics.Shared.BugReports
{
	public class BugReportSummary
	{
		public Guid Id { get; set; }

		public Guid PlayerId { get; set; }

		public string PlayerDeviceId { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		public bool HasScreenshot { get; set; }

		public float? CurrentFps { get; set; }

		public string SceneName { get; set; }

		public string Platform { get; set; }

		public BugReportStatus Status { get; set; }

		public BugReportPriority Priority { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
