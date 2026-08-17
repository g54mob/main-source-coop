using System;
using UnityEngine;

namespace EvilCore.GraphicsQuality
{
	public static class HardwareQualityDetector
	{
		public static GraphicsQualityLevel DetectFromSystemInfo(HardwareQualityConfig config)
		{
			return Evaluate(new HardwareQualitySample(SystemInfo.graphicsMemorySize, SystemInfo.systemMemorySize, SystemInfo.processorCount, SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceType), config);
		}

		public static GraphicsQualityLevel Evaluate(in HardwareQualitySample sample, HardwareQualityConfig config)
		{
			if (config == null)
			{
				return GraphicsQualityLevel.High;
			}
			if (config.ForceLowOnIntegratedGpu && IsIntegratedGpu(sample.GraphicsDeviceName, config))
			{
				return GraphicsQualityLevel.Low;
			}
			if (sample.VramMB <= 0)
			{
				return config.FallbackLevel;
			}
			if (Meets(in sample, config.MinVramMbHigh, config.MinRamMbHigh, config.MinCoresHigh))
			{
				return GraphicsQualityLevel.High;
			}
			if (Meets(in sample, config.MinVramMbMedium, config.MinRamMbMedium, config.MinCoresMedium))
			{
				return GraphicsQualityLevel.Medium;
			}
			return GraphicsQualityLevel.Low;
		}

		private static bool Meets(in HardwareQualitySample sample, int minVramMb, int minRamMb, int minCores)
		{
			if (sample.VramMB >= minVramMb && sample.SystemRamMB >= minRamMb)
			{
				return sample.LogicalCores >= minCores;
			}
			return false;
		}

		private static bool IsIntegratedGpu(string deviceName, HardwareQualityConfig config)
		{
			if (string.IsNullOrEmpty(deviceName) || config.IntegratedGpuNameKeywords == null)
			{
				return false;
			}
			string[] integratedGpuNameKeywords = config.IntegratedGpuNameKeywords;
			foreach (string value in integratedGpuNameKeywords)
			{
				if (!string.IsNullOrEmpty(value) && deviceName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
