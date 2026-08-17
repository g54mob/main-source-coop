namespace EvilAnalytics.Shared.Events
{
	public class SessionPerformanceSummary
	{
		public float AvgFps { get; set; }

		public float MinFps { get; set; }

		public float MaxFps { get; set; }

		public float P1Fps { get; set; }

		public float? AvgMemoryMb { get; set; }

		public float? PeakMemoryMb { get; set; }

		public int SampleCount { get; set; }
	}
}
