using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Portningsbolaget.Platforms
{
	public static class PlatformUtility
	{
		[Flags]
		public enum PlatformFamily
		{
			None = 0,
			Windows = 1,
			OSX = 2,
			Android = 4,
			IOS = 8,
			Switch = 0x10,
			Playstation = 0x20,
			Xbox = 0x40
		}

		public enum Platform
		{
			Windows = 1,
			OSX = 2,
			Steam = 4,
			SteamDeck = 8,
			Epic = 0x40,
			Android = 0x400,
			IOS = 0x800,
			Switch = 0x8000,
			Switch2 = 0x10000,
			Playstation4 = 0x100000,
			Playstation4Pro = 0x200000,
			Playstation5 = 0x400000,
			XboxOne = 0x2000000,
			XboxOneS = 0x4000000,
			XboxOneX = 0x8000000,
			XboxSeriesS = 0x10000000,
			XboxSeriesX = 0x20000000
		}

		private static bool _initialized = false;

		private static bool _settingsLoaded = false;

		private static bool _loadingSettings = false;

		private static PlatformFamily _currentPlatform = PlatformFamily.Windows;

		private static string SERVER_SETTINGS_URL;

		private static MonoBehaviour _coroutineRunner;

		public static bool Initialised => _initialized;

		public static PlatformFamily CurrentPlatform => _currentPlatform;

		public static bool AllowCrossplay { get; set; }

		public static event Action<bool> OnLoadedSettings;

		public static void InitializePlatform()
		{
			if (!_initialized)
			{
				Debug.Log("Initializing Platform Utility");
				switch (Application.platform)
				{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					_currentPlatform = PlatformFamily.Windows;
					break;
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer:
					_currentPlatform = PlatformFamily.OSX;
					break;
				case RuntimePlatform.Android:
					_currentPlatform = PlatformFamily.Android;
					break;
				case RuntimePlatform.IPhonePlayer:
					_currentPlatform = PlatformFamily.IOS;
					break;
				case RuntimePlatform.Switch:
				case RuntimePlatform.Switch2:
					_currentPlatform = PlatformFamily.Switch;
					break;
				case RuntimePlatform.PS4:
				case RuntimePlatform.PS5:
					_currentPlatform = PlatformFamily.Playstation;
					break;
				case RuntimePlatform.GameCoreXboxSeries:
				case RuntimePlatform.GameCoreXboxOne:
					_currentPlatform = PlatformFamily.Xbox;
					break;
				}
				_initialized = true;
				Debug.Log("Initialized Platform Utility");
			}
		}

		public static void LoadServerSettings(Action<bool> callback)
		{
			if (_loadingSettings)
			{
				return;
			}
			if (_settingsLoaded)
			{
				callback?.Invoke(_settingsLoaded);
				return;
			}
			if (_coroutineRunner == null)
			{
				GameObject gameObject = new GameObject("[ServerSettings]");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				_coroutineRunner = gameObject.AddComponent<EmptyBehaviour>();
			}
			_coroutineRunner.StartCoroutine(GetServerSettings(callback));
			_loadingSettings = true;
		}

		private static IEnumerator GetServerSettings(Action<bool> callback)
		{
			Debug.Log("Loading Server Settings");
			UnityWebRequest www = UnityWebRequest.Get(SERVER_SETTINGS_URL);
			www.timeout = 10;
			yield return www.SendWebRequest();
			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Failed Loading Server Settings: " + www.error);
				_settingsLoaded = false;
				AllowCrossplay = false;
			}
			else
			{
				string[] array = www.downloadHandler.text.Split(new char[1] { '\n' });
				bool result = true;
				bool result2 = true;
				bool result3 = true;
				bool result4 = true;
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (text.Contains("STEAM"))
					{
						bool.TryParse(text.Split('=')[1], out result);
					}
					else if (text.Contains("XBOX"))
					{
						bool.TryParse(text.Split('=')[1], out result2);
					}
					else if (text.Contains("PLAYSTATION"))
					{
						bool.TryParse(text.Split('=')[1], out result3);
					}
					else if (text.Contains("SWITCH"))
					{
						bool.TryParse(text.Split('=')[1], out result4);
					}
				}
				switch (_currentPlatform)
				{
				case PlatformFamily.Windows:
				case PlatformFamily.OSX:
					AllowCrossplay = result;
					break;
				case PlatformFamily.Switch:
					AllowCrossplay = result4;
					break;
				case PlatformFamily.Playstation:
					AllowCrossplay = result3;
					break;
				case PlatformFamily.Xbox:
					AllowCrossplay = result2;
					break;
				default:
					AllowCrossplay = true;
					break;
				}
				_settingsLoaded = true;
				Debug.Log("Loaded Server Settings");
			}
			Debug.Log($"Allowing Crossplay: {AllowCrossplay}");
			PlatformUtility.OnLoadedSettings?.Invoke(AllowCrossplay);
			callback?.Invoke(_settingsLoaded);
			_loadingSettings = false;
			if (_settingsLoaded)
			{
				UnityEngine.Object.Destroy(_coroutineRunner.gameObject);
				_coroutineRunner = null;
			}
		}

		static PlatformUtility()
		{
			PlatformUtility.OnLoadedSettings = null;
			SERVER_SETTINGS_URL = "https://portservice.games/content_warning/server_settings.txt";
			_coroutineRunner = null;
		}
	}
}
