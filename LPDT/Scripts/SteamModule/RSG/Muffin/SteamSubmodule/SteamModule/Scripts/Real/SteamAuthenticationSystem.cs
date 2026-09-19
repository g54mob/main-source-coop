using System;
using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using Features.GameUpdaterModule;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.Data;
using Steamworks;
using UnityEngine;
using Zenject;

namespace RSG.Muffin.SteamSubmodule.SteamModule.Scripts.Real
{
	public class SteamAuthenticationSystem : IInitializable, IDisposable
	{
		private readonly SteamEditorConfiguration _steamEditorConfiguration;

		private readonly SteamModel _steamModel;

		private readonly IGameUpdater _gameUpdater;

		private SteamAPIWarningMessageHook_t _steamAPIWarningMessageHook;

		private readonly IUnityDeviceTypeAccessor _unityDeviceTypeAccessor;

		private bool _isInitialized;

		public SteamAuthenticationSystem(SteamEditorConfiguration steamEditorConfiguration, SteamModel steamModel, IUnityDeviceTypeAccessor unityDeviceTypeAccessor, IGameUpdater gameUpdater)
		{
			_steamEditorConfiguration = steamEditorConfiguration;
			_steamModel = steamModel;
			_gameUpdater = gameUpdater;
			_unityDeviceTypeAccessor = unityDeviceTypeAccessor;
		}

		public void Initialize()
		{
			Features.DeviceModule.Scripts.DeviceData.DeviceType defaultDeviceType = _unityDeviceTypeAccessor.GetDefaultDeviceType();
			if (_steamEditorConfiguration.DevicesWithSteamChecking.Contains(defaultDeviceType))
			{
				if (!Packsize.Test())
				{
					Debug.LogError("Packsize is wrong! You are likely using a Linux/OSX build on Windows or vice versa.");
				}
				if (!DllCheck.Test())
				{
					Debug.LogError("DllCheck returned false.");
				}
				try
				{
					_steamModel.IsSteamInitialized = SteamAPI.Init();
				}
				catch (DllNotFoundException ex)
				{
					Debug.LogError("[Steamworks] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex);
					Application.Quit();
					return;
				}
				if (!_steamModel.IsSteamInitialized)
				{
					Debug.LogError("SteamAPI_Init() failed");
					return;
				}
				_steamModel.InvokeOnInitialized();
				_gameUpdater.OnUpdate += RunCallbacks;
			}
		}

		public void Dispose()
		{
			if (_steamModel.IsSteamInitialized)
			{
				_gameUpdater.OnUpdate -= RunCallbacks;
				SteamAPI.Shutdown();
				_steamModel.IsSteamInitialized = false;
			}
		}

		private void RunCallbacks()
		{
			SteamAPI.RunCallbacks();
		}
	}
}
