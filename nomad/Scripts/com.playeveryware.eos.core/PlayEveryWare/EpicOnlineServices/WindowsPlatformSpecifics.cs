using System.Collections.Generic;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Platform;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public class WindowsPlatformSpecifics : PlatformSpecifics<WindowsConfig>
	{
		private static string Xaudio2DllName = "xaudio2_9redist.dll";

		public static string SteamConfigPath = "eos_steam_config.json";

		private static GCHandle SteamOptionsGCHandle;

		public WindowsPlatformSpecifics()
			: base(PlatformManager.Platform.Windows)
		{
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Register()
		{
			EOSManagerPlatformSpecificsSingleton.SetEOSManagerPlatformSpecificsInterface(new WindowsPlatformSpecifics());
		}

		public override void LoadDelegatesWithEOSBindingAPI()
		{
		}

		public override void ConfigureSystemInitOptions(ref EOSInitializeOptions initializeOptions)
		{
		}

		public override void ConfigureSystemPlatformCreateOptions(ref EOSCreateOptions createOptions)
		{
			List<string> pathsToPlugins = DLLHandle.GetPathsToPlugins();
			WindowsRTCOptionsPlatformSpecificOptions value = default(WindowsRTCOptionsPlatformSpecificOptions);
			foreach (string item in pathsToPlugins)
			{
				string text = FileSystemUtility.CombinePaths(item, "Windows", "x64", Xaudio2DllName);
				if (FileSystemUtility.FileExists(text))
				{
					value.XAudio29DllPath = text;
					break;
				}
				text = FileSystemUtility.CombinePaths(item, "x64", Xaudio2DllName);
				if (FileSystemUtility.FileExists(text))
				{
					value.XAudio29DllPath = text;
					break;
				}
			}
			WindowsRTCOptions value2 = new WindowsRTCOptions
			{
				PlatformSpecificOptions = value
			};
			createOptions.options.RTCOptions = value2;
		}
	}
}
