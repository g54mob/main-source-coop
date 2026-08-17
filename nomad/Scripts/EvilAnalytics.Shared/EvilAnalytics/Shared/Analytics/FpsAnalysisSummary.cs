using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class FpsAnalysisSummary
	{
		public double OverallAvgFps { get; set; }

		public double OverallMinFps { get; set; }

		public double OverallMaxFps { get; set; }

		public double OverallP1Fps { get; set; }

		public int TotalSessionsWithFps { get; set; }

		public int TotalPlayersWithFps { get; set; }

		public List<FpsBucket> FpsDistribution { get; set; } = new List<FpsBucket>();

		public List<PlatformFpsStats> PlatformStats { get; set; } = new List<PlatformFpsStats>();
	}
}
