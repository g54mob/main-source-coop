using System;
using System.Security.Cryptography;
using System.Text;
using EvilAnalytics.Shared.Events;

namespace EvilAnalytics.SDK.Hardware
{
	public class HardwareCollector
	{
		public HardwareInfo Collect(string deviceId)
		{
			HardwareInfo hardwareInfo = new HardwareInfo
			{
				DeviceId = deviceId,
				CollectedAt = DateTimeOffset.UtcNow
			};
			try
			{
				hardwareInfo.OperatingSystem = Environment.OSVersion.ToString();
				hardwareInfo.OperatingSystemFamily = GetOsFamily();
				hardwareInfo.ProcessorCount = Environment.ProcessorCount;
				hardwareInfo.DeviceName = HashDeviceName(Environment.MachineName);
				hardwareInfo.DeviceType = GetDeviceType();
				hardwareInfo.SystemMemoryMB = (int)(Environment.WorkingSet / 1048576);
				hardwareInfo.ScreenWidth = 1920;
				hardwareInfo.ScreenHeight = 1080;
				hardwareInfo.FullScreen = false;
			}
			catch (Exception)
			{
			}
			return hardwareInfo;
		}

		private static string HashDeviceName(string deviceName)
		{
			if (string.IsNullOrEmpty(deviceName))
			{
				return "unknown";
			}
			using SHA256 sHA = SHA256.Create();
			byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(deviceName));
			return "device_" + BitConverter.ToString(array, 0, 8).Replace("-", "").ToLowerInvariant();
		}

		private static string GetOsFamily()
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.Win32NT:
				return "Windows";
			case PlatformID.MacOSX:
				return "MacOS";
			case PlatformID.Unix:
				return "Unix";
			default:
				return "Unknown";
			}
		}

		private static string GetDeviceType()
		{
			return "Desktop";
		}
	}
}
