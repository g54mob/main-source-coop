using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class SessionExportData
	{
		public Guid Id { get; set; }

		public DateTimeOffset StartedAt { get; set; }

		public DateTimeOffset? EndedAt { get; set; }

		public TimeSpan? Duration { get; set; }

		public int EventCount { get; set; }
	}
}
