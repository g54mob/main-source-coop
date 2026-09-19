using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using JetBrains.Annotations;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.Data;
using Steamworks;

namespace Features.SteamModule.Scripts.Real
{
	[PublicAPI]
	public class SteamRealDeviceOverrideService : IRealDeviceTypeAccessor
	{
		private readonly SteamEditorConfiguration _steamEditorConfiguration;

		private readonly SteamModel _steamModel;

		public SteamRealDeviceOverrideService(SteamEditorConfiguration steamEditorConfiguration, SteamModel steamModel)
		{
			_steamEditorConfiguration = steamEditorConfiguration;
			_steamModel = steamModel;
		}

		public DeviceType GetRealDeviceType(DeviceType defaultDeviceType)
		{
			if (!_steamEditorConfiguration.DevicesWithSteamChecking.Contains(defaultDeviceType) || !_steamModel.IsSteamInitialized)
			{
				return defaultDeviceType;
			}
			if (!SteamUtils.IsSteamRunningOnSteamDeck())
			{
				return defaultDeviceType;
			}
			return DeviceType.SteamDeck;
		}
	}
}
