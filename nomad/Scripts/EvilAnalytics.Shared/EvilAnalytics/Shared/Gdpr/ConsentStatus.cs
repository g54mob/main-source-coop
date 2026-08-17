using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class ConsentStatus
	{
		public string DeviceId { get; set; } = string.Empty;

		public bool Analytics { get; set; }

		public bool Hardware { get; set; }

		public bool Performance { get; set; }

		public bool CrashReporting { get; set; }

		public string? ConsentVersion { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}
}
