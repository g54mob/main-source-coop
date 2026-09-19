using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using Features.SettingsMenuModule.Scripts.Data;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Services
{
	public class PerformanceDetectionService : IPerformanceDetectionService
	{
		private readonly QualitySettingsOptionsConfiguration _qualitySettingsOptionsConfiguration;

		private readonly IDeviceService _deviceService;

		public PerformanceDetectionService(QualitySettingsOptionsConfiguration qualitySettingsOptionsConfiguration, IDeviceService deviceService)
		{
			_qualitySettingsOptionsConfiguration = qualitySettingsOptionsConfiguration;
			_deviceService = deviceService;
		}

		public PerformanceLevel DeterminePerformanceLevel()
		{
			Features.DeviceModule.Scripts.DeviceData.DeviceType currentDevice = _deviceService.GetCurrentDevice();
			PlatformPerformanceProfile profileForDevice = _qualitySettingsOptionsConfiguration.GetProfileForDevice(currentDevice);
			if (profileForDevice == null)
			{
				return DetermineByThresholds(_qualitySettingsOptionsConfiguration.LowPerformanceSettings, _qualitySettingsOptionsConfiguration.MediumPerformanceSettings, _qualitySettingsOptionsConfiguration.HighPerformanceSettings);
			}
			if (profileForDevice.UseForcedLevel)
			{
				return profileForDevice.ForcedLevel;
			}
			return DetermineByThresholds(profileForDevice.LowPerformanceSettings, profileForDevice.MediumPerformanceSettings, profileForDevice.HighPerformanceSettings);
		}

		private PerformanceLevel DetermineByThresholds(PerformanceLevelSettings lowSettings, PerformanceLevelSettings mediumSettings, PerformanceLevelSettings highSettings)
		{
			int num = 0;
			if (SystemInfo.processorCount >= highSettings.ProcessorCount)
			{
				num += 2;
			}
			else if (SystemInfo.processorCount >= mediumSettings.ProcessorCount)
			{
				num++;
			}
			else if (SystemInfo.processorCount >= lowSettings.ProcessorCount)
			{
				num = num;
			}
			if (SystemInfo.systemMemorySize >= highSettings.MemorySize)
			{
				num += 3;
			}
			else if (SystemInfo.systemMemorySize >= mediumSettings.MemorySize)
			{
				num += 2;
			}
			else if (SystemInfo.systemMemorySize >= lowSettings.MemorySize)
			{
				num++;
			}
			if (SystemInfo.graphicsMemorySize >= highSettings.GraphicsMemorySize)
			{
				num += 5;
			}
			else if (SystemInfo.graphicsMemorySize >= mediumSettings.GraphicsMemorySize)
			{
				num += 3;
			}
			else if (SystemInfo.graphicsMemorySize >= lowSettings.GraphicsMemorySize)
			{
				num++;
			}
			if (num >= 8)
			{
				return PerformanceLevel.High;
			}
			if (num >= 4)
			{
				return PerformanceLevel.Medium;
			}
			return PerformanceLevel.Low;
		}
	}
}
