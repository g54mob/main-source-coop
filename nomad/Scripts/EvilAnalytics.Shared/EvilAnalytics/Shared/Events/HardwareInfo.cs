using System;

namespace EvilAnalytics.Shared.Events
{
	public class HardwareInfo
	{
		public string DeviceId { get; set; } = string.Empty;

		[Obsolete("API key should be sent via X-API-Key header only. This field is ignored.")]
		public string ApiKey { get; set; } = string.Empty;

		public string? DeviceModel { get; set; }

		public string? DeviceType { get; set; }

		public string? DeviceName { get; set; }

		public string? OperatingSystem { get; set; }

		public string? OperatingSystemFamily { get; set; }

		public string? ProcessorType { get; set; }

		public int ProcessorCount { get; set; }

		public int ProcessorFrequencyMHz { get; set; }

		public int SystemMemoryMB { get; set; }

		public string? GraphicsDeviceName { get; set; }

		public string? GraphicsDeviceVendor { get; set; }

		public string? GraphicsDeviceType { get; set; }

		public int GraphicsMemoryMB { get; set; }

		public int GraphicsShaderLevel { get; set; }

		public int ScreenWidth { get; set; }

		public int ScreenHeight { get; set; }

		public float ScreenDpi { get; set; }

		public int ScreenRefreshRate { get; set; }

		public bool FullScreen { get; set; }

		public bool SupportsGyroscope { get; set; }

		public bool SupportsAccelerometer { get; set; }

		public bool SupportsLocationService { get; set; }

		public bool SupportsVibration { get; set; }

		public bool SupportsAudio { get; set; }

		public string? NetworkReachability { get; set; }

		public float BatteryLevel { get; set; }

		public string? BatteryStatus { get; set; }

		public DateTimeOffset CollectedAt { get; set; } = DateTimeOffset.UtcNow;
	}
}
