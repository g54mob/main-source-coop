using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace Features.SteamImplementationModule.Scripts.Real
{
	public class SteamLanguageProvider : ISteamLanguageProvider
	{
		private readonly SteamModel _steamModel;

		public bool IsAvailable => _steamModel.IsSteamInitialized;

		public SteamLanguageProvider(SteamModel steamModel)
		{
			_steamModel = steamModel;
		}

		public string GetSteamUILanguage()
		{
			return SteamUtils.GetSteamUILanguage();
		}
	}
}
