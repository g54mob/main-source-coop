using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class PlayerExportData
	{
		public Guid Id { get; set; }

		public DateTimeOffset FirstSeen { get; set; }

		public DateTimeOffset LastSeen { get; set; }

		public int TotalSessions { get; set; }

		public TimeSpan TotalPlayTime { get; set; }
	}
}
