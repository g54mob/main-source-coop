using System;

namespace EvilAnalytics.Shared.Events
{
	public class HeartbeatRequest
	{
		public Guid SessionId { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public string ApiKey { get; set; } = string.Empty;

		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	}
}
