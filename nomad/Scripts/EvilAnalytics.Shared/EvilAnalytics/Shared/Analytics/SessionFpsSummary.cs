using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class SessionFpsSummary
	{
		public Guid SessionId { get; set; }

		public DateTimeOffset StartedAt { get; set; }

		public int? DurationSeconds { get; set; }

		public float AvgFps { get; set; }

		public float MinFps { get; set; }

		public float MaxFps { get; set; }

		public float P1Fps { get; set; }

		public float? AvgMemoryMb { get; set; }

		public int FpsSampleCount { get; set; }
	}
}
