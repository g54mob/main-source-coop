using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerFpsProfile
	{
		public Guid PlayerId { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public string? Platform { get; set; }

		public double AvgFps { get; set; }

		public double MinFps { get; set; }

		public double MaxFps { get; set; }

		public double AvgP1Fps { get; set; }

		public double AvgMemoryMb { get; set; }

		public double PeakMemoryMb { get; set; }

		public int SessionCount { get; set; }

		public int TotalFpsSamples { get; set; }

		public PlayerHardwareInfo? Hardware { get; set; }

		public List<SessionFpsSummary> RecentSessions { get; set; } = new List<SessionFpsSummary>();
	}
}
