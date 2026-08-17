using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class CrashLogOccurrence
	{
		public Guid Id { get; set; }

		public Guid PlayerId { get; set; }

		public string PlayerDeviceId { get; set; } = string.Empty;

		public Guid SessionId { get; set; }

		public string SceneName { get; set; }

		public float? CurrentFps { get; set; }

		public float? MemoryUsedMb { get; set; }

		public DateTimeOffset Timestamp { get; set; }
	}
}
