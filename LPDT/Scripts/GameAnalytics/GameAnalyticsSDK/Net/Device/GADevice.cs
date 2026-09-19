using System;
using System.IO;
using System.Text.RegularExpressions;
using GameAnalyticsSDK.Net.Logging;
using UnityEngine;

namespace GameAnalyticsSDK.Net.Device
{
	internal static class GADevice
	{
		private const string _sdkWrapperVersion = "mono 3.4.0";

		private static readonly string _buildPlatform = UnityRuntimePlatformToString(Application.platform);

		private static readonly string _deviceModel = SystemInfo.deviceType.ToString().ToLowerInvariant();

		private static string _writablepath = GetPersistentPath();

		private static readonly string _osVersion = GetOSVersionString();

		private static readonly string _deviceManufacturer = "unknown";

		public static string SdkGameEngineVersion { private get; set; }

		public static string GameEngineVersion { get; set; }

		public static string ConnectionType { get; set; }

		public static string RelevantSdkVersion
		{
			get
			{
				if (!string.IsNullOrEmpty(SdkGameEngineVersion))
				{
					return SdkGameEngineVersion;
				}
				return "mono 3.4.0";
			}
		}

		public static string BuildPlatform => _buildPlatform;

		public static string OSVersion => _osVersion;

		public static string DeviceModel => _deviceModel;

		public static string DeviceManufacturer => _deviceManufacturer;

		public static string WritablePath => _writablepath;

		public static void Touch()
		{
		}

		public static void UpdateConnectionType()
		{
			switch (Application.internetReachability)
			{
			case NetworkReachability.ReachableViaCarrierDataNetwork:
				ConnectionType = "wwan";
				break;
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				ConnectionType = "lan";
				break;
			default:
				ConnectionType = "offline";
				break;
			}
		}

		private static string GetOSVersionString()
		{
			string operatingSystem = SystemInfo.operatingSystem;
			GALogger.D("GetOSVersionString: " + operatingSystem);
			Match match = Regex.Match(operatingSystem, "Windows.*?\\((\\d{0,5}\\.\\d{0,5}\\.(\\d{0,5}))\\)");
			if (match.Success)
			{
				string text = match.Groups[1].Value;
				string value = match.Groups[2].Value;
				int result = 0;
				int.TryParse(value, out result);
				if (result > 10000)
				{
					text = "10.0." + value;
				}
				return "windows " + text;
			}
			match = Regex.Match(operatingSystem, "Mac OS X (\\d{0,5}\\.\\d{0,5}\\.\\d{0,5})");
			if (match.Success)
			{
				return "mac_osx " + match.Captures[0].Value.Replace("Mac OS X ", "");
			}
			match = Regex.Match(operatingSystem, "Mac OS X (\\d{0,5}_\\d{0,5}_\\d{0,5})");
			if (match.Success)
			{
				return "mac_osx " + match.Captures[0].Value.Replace("Mac OS X ", "").Replace("_", ".");
			}
			return UnityRuntimePlatformToString(Application.platform) + " 0.0.0";
		}

		private static string GetPersistentPath()
		{
			string[] obj = new string[5]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				null,
				null,
				null,
				null
			};
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[1] = directorySeparatorChar.ToString();
			obj[2] = "GameAnalytics";
			directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[3] = directorySeparatorChar.ToString();
			obj[4] = AppDomain.CurrentDomain.FriendlyName;
			string text = string.Concat(obj);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		private static string UnityRuntimePlatformToString(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.LinuxPlayer:
				return "linux";
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXDashboardPlayer:
				return "mac_osx";
			case RuntimePlatform.PS3:
				return "ps3";
			case RuntimePlatform.PS4:
				return "ps4";
			case RuntimePlatform.PSP2:
				return "vita";
			case RuntimePlatform.WindowsPlayer:
				return "windows";
			case RuntimePlatform.PSM:
				return "psm";
			case RuntimePlatform.WiiU:
				return "wiiu";
			case RuntimePlatform.WebGLPlayer:
				return "webgl";
			case RuntimePlatform.MetroPlayerX86:
			case RuntimePlatform.MetroPlayerX64:
			case RuntimePlatform.MetroPlayerARM:
				return SystemInfo.deviceType switch
				{
					DeviceType.Desktop => "uwp_desktop", 
					DeviceType.Handheld => "uwp_mobile", 
					DeviceType.Console => "uwp_console", 
					_ => "uwp_desktop", 
				};
			case RuntimePlatform.WP8Player:
				return "windows_phone";
			case RuntimePlatform.XBOX360:
				return "xbox360";
			case RuntimePlatform.XboxOne:
				return "xboxone";
			case RuntimePlatform.TizenPlayer:
				return "tizen";
			case RuntimePlatform.SamsungTVPlayer:
				return "samsung_tv";
			default:
				return "unknown";
			}
		}
	}
}
