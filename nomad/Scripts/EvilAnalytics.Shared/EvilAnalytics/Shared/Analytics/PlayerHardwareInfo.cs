namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerHardwareInfo
	{
		public string? DeviceModel { get; set; }

		public string? OperatingSystem { get; set; }

		public string? OsFamily { get; set; }

		public string? ProcessorType { get; set; }

		public int ProcessorCount { get; set; }

		public int SystemMemoryMB { get; set; }

		public string? GraphicsDevice { get; set; }

		public string? GraphicsVendor { get; set; }

		public int GraphicsMemoryMB { get; set; }

		public int ScreenWidth { get; set; }

		public int ScreenHeight { get; set; }
	}
}
