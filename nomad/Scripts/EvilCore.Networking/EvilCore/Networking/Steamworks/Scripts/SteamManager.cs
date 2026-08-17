using System;
using EvilCore.EvilPack.EvilLogger;
using Steamworks;
using UnityEngine;

namespace EvilCore.Networking.Steamworks.Scripts
{
	public class SteamManager : MonoSingleton<SteamManager>
	{
		[Tooltip("Steam Application ID. 480 = Spacewar (test app).")]
		[SerializeField]
		private uint steamAppId;

		private bool _initialized;

		public void Initialize()
		{
			if (_initialized)
			{
				return;
			}
			try
			{
				SteamClient.Init(steamAppId, asyncCallbacks: false);
				_initialized = true;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[SteamManager] Steam init failed: " + ex.Message, "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\Steam\\SteamManager.cs", 27);
			}
		}

		private void Update()
		{
			if (SteamClient.IsValid)
			{
				SteamClient.RunCallbacks();
			}
		}

		private void OnApplicationQuit()
		{
			try
			{
				if (SteamClient.IsValid)
				{
					SteamClient.Shutdown();
				}
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[SteamManager] Shutdown error: " + ex.Message, "OnApplicationQuit", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\Steam\\SteamManager.cs", 46);
			}
		}
	}
}
