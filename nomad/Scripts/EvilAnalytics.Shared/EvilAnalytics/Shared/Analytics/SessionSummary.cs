using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class SessionSummary
	{
		public Guid Id { get; set; }

		public DateTimeOffset StartedAt { get; set; }

		public DateTimeOffset? EndedAt { get; set; }

		public TimeSpan? Duration { get; set; }

		public int EventCount { get; set; }

		public string? EndReason { get; set; }

		public float? AvgFps { get; set; }

		public float? MinFps { get; set; }

		public float? MaxFps { get; set; }

		public float? P1Fps { get; set; }

		public float? AvgMemoryMb { get; set; }

		public float? PeakMemoryMb { get; set; }

		public int? FpsSampleCount { get; set; }
	}
}
