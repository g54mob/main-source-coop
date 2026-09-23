using Mimicraft.Networking;
using Unity.Netcode;

namespace Mimicraft.Dev
{
	public static class DevCheats
	{
		private static bool localEnabled;

		private static bool sessionAllows = true;

		public static bool InSession
		{
			get
			{
				if (NetworkManager.Singleton != null)
				{
					return NetworkManager.Singleton.IsListening;
				}
				return false;
			}
		}

		private static bool IsServer
		{
			get
			{
				if (NetworkManager.Singleton != null)
				{
					return NetworkManager.Singleton.IsServer;
				}
				return false;
			}
		}

		public static bool Enabled
		{
			get
			{
				if (!InSession)
				{
					return localEnabled;
				}
				LobbySettingsSync instance = LobbySettingsSync.Instance;
				if (instance != null)
				{
					return instance.CheatsEnabled;
				}
				return false;
			}
		}

		public static void SetSessionPolicy(bool allowed)
		{
			sessionAllows = allowed;
		}

		public static string TrySet(bool value)
		{
			if (!InSession)
			{
				localEnabled = value;
				return $"sv_cheats = {(value ? 1 : 0)}  (no session - local only)";
			}
			if (!IsServer)
			{
				return "Only the host can change sv_cheats.";
			}
			if (value && !sessionAllows)
			{
				return "sv_cheats stays 0: this is a public Steam lobby, and cheats cannot be enabled where strangers can join. Host a Friends Only or Invites Only lobby to use them.";
			}
			LobbySettingsSync instance = LobbySettingsSync.Instance;
			if (instance == null)
			{
				return "sv_cheats cannot be set: this session has no LobbySettingsSync to carry it.";
			}
			if (instance.CheatsEnabled == value)
			{
				return $"sv_cheats = {(value ? 1 : 0)}  (unchanged)";
			}
			instance.ServerSetCheats(value);
			return $"sv_cheats = {(value ? 1 : 0)}";
		}

		public static string Describe()
		{
			if (!InSession)
			{
				return "(no session - local value, `sv_cheats 1` to enable)";
			}
			if (!IsServer)
			{
				return "(set by the host)";
			}
			if (!sessionAllows)
			{
				return "(public Steam lobby - locked at 0)";
			}
			return "(host - `sv_cheats 1` to enable)";
		}
	}
}
