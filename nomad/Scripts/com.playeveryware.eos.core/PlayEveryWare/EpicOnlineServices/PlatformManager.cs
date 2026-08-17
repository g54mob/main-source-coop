using System;
using System.Collections.Generic;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public static class PlatformManager
	{
		[Flags]
		public enum Platform
		{
			Unknown = 0,
			Windows = 1,
			Android = 2,
			XboxOne = 4,
			XboxSeriesX = 8,
			iOS = 0x10,
			Linux = 0x20,
			macOS = 0x40,
			PS4 = 0x80,
			PS5 = 0x100,
			Switch = 0x200,
			Steam = 0x400,
			Switch2 = 0x800,
			Console = 0xB8C,
			Any = 0xFFF
		}

		internal readonly struct PlatformInfo
		{
			public string FullName { get; }

			public string ConfigFileName { get; }

			public string DynamicLibraryExtension { get; }

			public string PlatformIconLabel { get; }

			public Func<PlatformConfig> GetConfigFunction { get; }

			public Type ConfigType { get; }

			private PlatformInfo(Func<PlatformConfig> getConfigFunction, Type configType, string fullName, string configFileName, string dynamicLibraryExtension, string platformIconLabel)
			{
				FullName = fullName;
				ConfigFileName = configFileName;
				DynamicLibraryExtension = dynamicLibraryExtension;
				PlatformIconLabel = platformIconLabel;
				GetConfigFunction = getConfigFunction;
				ConfigType = configType;
			}

			public static PlatformInfo Create<T>(string fullName, string configFileName, string dynamicLibraryExtension, string platformIconLabel) where T : PlatformConfig
			{
				return new PlatformInfo(Config.Get<T>, typeof(T), fullName, configFileName, dynamicLibraryExtension, platformIconLabel);
			}
		}

		internal static IDictionary<Platform, PlatformInfo> PlatformInformation;

		private static Platform s_CurrentPlatform;

		private static PlatformConfig s_platformConfig;

		private static Platform s_CurrentTargetedPlatform;

		private static readonly IDictionary<RuntimePlatform, Platform> RuntimeToPlatformsMap;

		public static IEnumerable<Platform> ConfigurablePlatforms => PlatformInformation.Keys;

		public static Platform CurrentPlatform
		{
			get
			{
				return s_CurrentPlatform;
			}
			set
			{
				if (CurrentPlatform == Platform.Unknown)
				{
					s_CurrentPlatform = value;
					Debug.Log("CurrentPlatform has been assigned as " + GetFullName(s_CurrentPlatform) + ".");
				}
				else
				{
					Debug.Log("CurrentPlatform has already been assigned as " + GetFullName(s_CurrentPlatform) + ".");
				}
			}
		}

		public static bool TryGetConfig(Platform platform, out PlatformConfig platformConfig)
		{
			platformConfig = null;
			if (!PlatformInformation.TryGetValue(platform, out var value))
			{
				return false;
			}
			platformConfig = value.GetConfigFunction();
			return true;
		}

		private static void InitializePlatformConfigs()
		{
			PlatformInformation.Add(Platform.Android, PlatformInfo.Create<AndroidConfig>("Android", "eos_android_config.json", null, "Android"));
			PlatformInformation.Add(Platform.iOS, PlatformInfo.Create<IOSConfig>("iOS", "eos_ios_config.json", null, "iPhone"));
			PlatformInformation.Add(Platform.Linux, PlatformInfo.Create<LinuxConfig>("Linux", "eos_linux_config.json", ".so", "Standalone"));
			PlatformInformation.Add(Platform.macOS, PlatformInfo.Create<MacOSConfig>("macOS", "eos_macos_config.json", ".dylib", "Standalone"));
			PlatformInformation.Add(Platform.Windows, PlatformInfo.Create<WindowsConfig>("Windows", "eos_windows_config.json", ".dll", "Standalone"));
		}

		static PlatformManager()
		{
			PlatformInformation = new Dictionary<Platform, PlatformInfo>();
			s_platformConfig = null;
			RuntimeToPlatformsMap = new Dictionary<RuntimePlatform, Platform>
			{
				{
					RuntimePlatform.Android,
					Platform.Android
				},
				{
					RuntimePlatform.IPhonePlayer,
					Platform.iOS
				},
				{
					RuntimePlatform.PS4,
					Platform.PS4
				},
				{
					RuntimePlatform.PS5,
					Platform.PS5
				},
				{
					RuntimePlatform.GameCoreXboxOne,
					Platform.XboxOne
				},
				{
					RuntimePlatform.XboxOne,
					Platform.XboxOne
				},
				{
					RuntimePlatform.Switch,
					Platform.Switch
				},
				{
					RuntimePlatform.GameCoreXboxSeries,
					Platform.XboxSeriesX
				},
				{
					RuntimePlatform.LinuxPlayer,
					Platform.Linux
				},
				{
					RuntimePlatform.LinuxEditor,
					Platform.Linux
				},
				{
					RuntimePlatform.EmbeddedLinuxX64,
					Platform.Linux
				},
				{
					RuntimePlatform.EmbeddedLinuxX86,
					Platform.Linux
				},
				{
					RuntimePlatform.LinuxServer,
					Platform.Linux
				},
				{
					RuntimePlatform.WindowsServer,
					Platform.Windows
				},
				{
					RuntimePlatform.WindowsPlayer,
					Platform.Windows
				},
				{
					RuntimePlatform.WindowsEditor,
					Platform.Windows
				},
				{
					RuntimePlatform.OSXEditor,
					Platform.macOS
				},
				{
					RuntimePlatform.OSXPlayer,
					Platform.macOS
				},
				{
					RuntimePlatform.OSXServer,
					Platform.macOS
				}
			};
			InitializePlatformConfigs();
			if (TryGetPlatform(Application.platform, out var platform))
			{
				CurrentPlatform = platform;
				return;
			}
			CurrentPlatform = Platform.Unknown;
			Debug.LogWarning("Platform could not be determined.");
		}

		public static PlatformConfig GetPlatformConfig()
		{
			if (s_platformConfig != null)
			{
				return s_platformConfig;
			}
			if (!PlatformInformation.TryGetValue(CurrentPlatform, out var value) || value.GetConfigFunction == null)
			{
				Debug.LogError($"Could not get platform config for platform \"{CurrentPlatform}\".");
				return null;
			}
			s_platformConfig = value.GetConfigFunction();
			return s_platformConfig;
		}

		public static bool TryGetPlatform(RuntimePlatform runtimePlatform, out Platform platform)
		{
			return RuntimeToPlatformsMap.TryGetValue(runtimePlatform, out platform);
		}

		public static string GetDynamicLibraryExtension(Platform platform)
		{
			return PlatformInformation[platform].DynamicLibraryExtension;
		}

		public static string GetConfigFilePath()
		{
			return GetConfigFilePath(CurrentPlatform);
		}

		public static string GetConfigFilePath(Platform platform)
		{
			return FileSystemUtility.CombinePaths(Application.streamingAssetsPath, "EOS", GetConfigFileName(platform));
		}

		public static bool TryGetConfigFilePath(Platform platform, out string configFilePath)
		{
			if (PlatformInformation.ContainsKey(platform))
			{
				configFilePath = GetConfigFilePath(platform);
				return true;
			}
			configFilePath = "";
			return false;
		}

		public static string GetConfigFileName(Platform platform)
		{
			return PlatformInformation[platform].ConfigFileName;
		}

		public static string GetFullName(Platform platform)
		{
			if (PlatformInformation.TryGetValue(platform, out var value))
			{
				return value.FullName;
			}
			return platform.ToString();
		}
	}
}
