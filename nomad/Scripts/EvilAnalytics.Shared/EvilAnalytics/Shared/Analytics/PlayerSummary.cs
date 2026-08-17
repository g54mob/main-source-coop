using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerSummary
	{
		public Guid Id { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public DateTimeOffset FirstSeen { get; set; }

		public DateTimeOffset LastSeen { get; set; }

		public int TotalSessions { get; set; }

		public TimeSpan TotalPlayTime { get; set; }

		public long TotalEvents { get; set; }

		public bool IsAnonymized { get; set; }

		public string? Platform { get; set; }

		public string? Country { get; set; }
	}
}
