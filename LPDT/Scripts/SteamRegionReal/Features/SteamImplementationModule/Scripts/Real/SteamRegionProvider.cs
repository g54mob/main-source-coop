using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace Features.SteamImplementationModule.Scripts.Real
{
	public class SteamRegionProvider : ISteamRegionProvider
	{
		private readonly SteamModel _steamModel;

		public bool IsAvailable => _steamModel.IsSteamInitialized;

		public SteamRegionProvider(SteamModel steamModel)
		{
			_steamModel = steamModel;
		}

		public string GetIpCountry()
		{
			return SteamUtils.GetIPCountry();
		}
	}
}
