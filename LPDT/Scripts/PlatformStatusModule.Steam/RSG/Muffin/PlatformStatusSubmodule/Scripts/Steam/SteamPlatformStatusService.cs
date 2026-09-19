using RSG.Muffin.PlatformStatusSubmodule.Scripts.API;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace RSG.Muffin.PlatformStatusSubmodule.Scripts.Steam
{
	public class SteamPlatformStatusService : IPlatformStatusService
	{
		private const string PLAYER_GROUP_KEY = "steam_player_group";

		private const string PLAYER_GROUP_SIZE_KEY = "steam_player_group_size";

		private readonly SteamModel _steamModel;

		public SteamPlatformStatusService(SteamModel steamModel)
		{
			_steamModel = steamModel;
		}

		public void UpdateGenericStatus<TValue>(string key, TValue value)
		{
			if (_steamModel.IsSteamInitialized)
			{
				SteamFriends.SetRichPresence(key, value.ToString());
			}
		}

		public void UpdatePlayerGroup(string groupId, int groupSize)
		{
			if (_steamModel.IsSteamInitialized)
			{
				SteamFriends.SetRichPresence("steam_player_group", groupId);
				SteamFriends.SetRichPresence("steam_player_group_size", groupSize.ToString());
			}
		}

		public void ClearPlayerGroup()
		{
			if (_steamModel.IsSteamInitialized)
			{
				SteamFriends.SetRichPresence("steam_player_group", null);
				SteamFriends.SetRichPresence("steam_player_group_size", null);
			}
		}
	}
}
