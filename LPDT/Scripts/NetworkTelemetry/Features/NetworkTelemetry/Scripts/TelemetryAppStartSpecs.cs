using System;
using System.Collections.Generic;
using System.Globalization;
using Features.DebugModule.Scripts;
using UnityEngine;

namespace Features.NetworkTelemetry.Scripts
{
	internal static class TelemetryAppStartSpecs
	{
		internal static Dictionary<string, string> BuildExtras()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(64);
			dictionary["player_kind"] = "player_build";
			dictionary["scripting_backend"] = "mono";
			dictionary["platform"] = Application.platform.ToString();
			dictionary["app_version"] = Application.version;
			dictionary["git_commit_short"] = GitRevision.CommitHashShort;
			dictionary["debug"] = Debug.isDebugBuild.ToString();
			dictionary["unity_version"] = Application.unityVersion;
			dictionary["product_name"] = Application.productName;
			dictionary["identifier"] = Application.identifier;
			dictionary["install_mode"] = Application.installMode.ToString();
			dictionary["system_language"] = Application.systemLanguage.ToString();
			dictionary["target_frame_rate"] = Application.targetFrameRate.ToString(CultureInfo.InvariantCulture);
			try
			{
				dictionary["time_zone"] = TimeZoneInfo.Local.Id;
			}
			catch (TimeZoneNotFoundException)
			{
				dictionary["time_zone"] = "unknown";
			}
			catch (InvalidOperationException)
			{
				dictionary["time_zone"] = "unknown";
			}
			dictionary["device_model"] = SystemInfo.deviceModel;
			dictionary["device_type"] = SystemInfo.deviceType.ToString();
			dictionary["operating_system"] = SystemInfo.operatingSystem;
			dictionary["operating_system_family"] = SystemInfo.operatingSystemFamily.ToString();
			dictionary["os"] = SystemInfo.operatingSystem;
			dictionary["processor_type"] = SystemInfo.processorType;
			dictionary["processor_count"] = SystemInfo.processorCount.ToString(CultureInfo.InvariantCulture);
			dictionary["processor_frequency_mhz"] = SystemInfo.processorFrequency.ToString(CultureInfo.InvariantCulture);
			dictionary["system_memory_mb"] = SystemInfo.systemMemorySize.ToString(CultureInfo.InvariantCulture);
			dictionary["graphics_device_name"] = SystemInfo.graphicsDeviceName;
			dictionary["graphics_device"] = SystemInfo.graphicsDeviceName;
			dictionary["graphics_device_vendor"] = SystemInfo.graphicsDeviceVendor;
			dictionary["graphics_device_version"] = SystemInfo.graphicsDeviceVersion;
			dictionary["graphics_memory_mb"] = SystemInfo.graphicsMemorySize.ToString(CultureInfo.InvariantCulture);
			dictionary["graphics_device_type"] = SystemInfo.graphicsDeviceType.ToString();
			dictionary["graphics_shader_level"] = SystemInfo.graphicsShaderLevel.ToString(CultureInfo.InvariantCulture);
			Resolution currentResolution = Screen.currentResolution;
			dictionary["screen_width"] = currentResolution.width.ToString(CultureInfo.InvariantCulture);
			dictionary["screen_height"] = currentResolution.height.ToString(CultureInfo.InvariantCulture);
			dictionary["screen_refresh_hz"] = currentResolution.refreshRateRatio.value.ToString(CultureInfo.InvariantCulture);
			dictionary["screen_dpi"] = Screen.dpi.ToString(CultureInfo.InvariantCulture);
			dictionary["screen_fullscreen_mode"] = Screen.fullScreenMode.ToString();
			int qualityLevel = QualitySettings.GetQualityLevel();
			dictionary["quality_level_index"] = qualityLevel.ToString(CultureInfo.InvariantCulture);
			string[] names = QualitySettings.names;
			dictionary["quality_level_name"] = ((names != null && qualityLevel >= 0 && qualityLevel < names.Length) ? names[qualityLevel] : string.Empty);
			dictionary["quality_v_sync_count"] = QualitySettings.vSyncCount.ToString(CultureInfo.InvariantCulture);
			dictionary["quality_anti_aliasing"] = QualitySettings.antiAliasing.ToString(CultureInfo.InvariantCulture);
			return dictionary;
		}

		private static string Bool(bool value)
		{
			if (!value)
			{
				return "false";
			}
			return "true";
		}
	}
}
