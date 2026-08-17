using System;

namespace EvilAnalytics.Shared.Events
{
	public class PerformanceSnapshotResponse
	{
		public bool Success { get; set; }

		public DateTimeOffset ServerTime { get; set; }
	}
}
