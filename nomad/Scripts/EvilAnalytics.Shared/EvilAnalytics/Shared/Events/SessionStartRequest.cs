using System;

namespace EvilAnalytics.Shared.Events
{
	public class SessionStartRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		public string ApiKey { get; set; } = string.Empty;

		public string SdkVersion { get; set; } = string.Empty;

		public string? AppVersion { get; set; }

		public string? Platform { get; set; }

		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
