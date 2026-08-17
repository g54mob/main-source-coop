using System;

namespace EvilAnalytics.Shared.Events
{
	public class PerformanceSnapshotRequest
	{
		public Guid SessionId { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public string ApiKey { get; set; } = string.Empty;

		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

		public float AvgFps { get; set; }

		public float MinFps { get; set; }

		public float MaxFps { get; set; }

		public float P1Fps { get; set; }

		public int SampleCount { get; set; }

		public float? MemoryUsedMb { get; set; }

		public float? GpuMemoryMb { get; set; }

		public string? SceneName { get; set; }
	}
}
