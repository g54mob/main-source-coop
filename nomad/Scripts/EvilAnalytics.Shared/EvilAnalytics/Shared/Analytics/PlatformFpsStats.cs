namespace EvilAnalytics.Shared.Analytics
{
	public class PlatformFpsStats
	{
		public string Platform { get; set; } = string.Empty;

		public double AvgFps { get; set; }

		public double MinFps { get; set; }

		public double MaxFps { get; set; }

		public int PlayerCount { get; set; }

		public int SessionCount { get; set; }
	}
}
