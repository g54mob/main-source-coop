using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class HardwareExportData
	{
		public DateTimeOffset CollectedAt { get; set; }

		public string? DeviceModel { get; set; }

		public string? OperatingSystem { get; set; }

		public string? ProcessorType { get; set; }

		public string? GraphicsDevice { get; set; }

		public string? ScreenResolution { get; set; }
	}
}
