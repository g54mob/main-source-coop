namespace EvilAnalytics.Shared.Crashlytics
{
	public class SystemResourceSnapshot
	{
		public long SystemMemoryTotalMb { get; set; }

		public long SystemMemoryUsedMb { get; set; }

		public float SystemMemoryUsagePercent { get; set; }

		public long UnityAllocatedMemoryMb { get; set; }

		public long UnityReservedMemoryMb { get; set; }

		public long GcTotalMemoryMb { get; set; }

		public int ScreenWidth { get; set; }

		public int ScreenHeight { get; set; }

		public int RefreshRate { get; set; }

		public bool IsFullScreen { get; set; }

		public string QualityLevel { get; set; } = string.Empty;

		public string GraphicsDeviceName { get; set; } = string.Empty;

		public int GraphicsMemoryMb { get; set; }

		public string ProcessorType { get; set; } = string.Empty;

		public int ProcessorCount { get; set; }

		public int ProcessorFrequencyMhz { get; set; }
	}
}
