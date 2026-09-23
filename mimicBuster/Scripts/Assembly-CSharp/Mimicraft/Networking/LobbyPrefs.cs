using TMPro;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class LobbyPrefs
	{
		private const string Prefix = "Mimicraft.Lobby.";

		public const string Transport = "Transport";

		public const string LobbyName = "Name";

		public const string PrepSeconds = "PrepSeconds";

		public const string HuntSeconds = "HuntSeconds";

		public const string RoundEndSeconds = "RoundEndSeconds";

		public const string MinHiders = "MinHiders";

		public const string MinHunters = "MinHunters";

		public const string MinPlayers = "MinPlayers";

		public const string ScoreLimit = "ScoreLimit";

		public const string TimeLimit = "TimeLimit";

		public const string ExtraWarmup = "ExtraWarmup";

		public const string RespawnTime = "RespawnTime";

		public const string Port = "Port";

		public const string Visibility = "Visibility";

		public const string MaxPlayers = "MaxPlayers";

		public const string TauntInterval = "TauntInterval";

		public const string SelfDamage = "SelfDamage";

		public const string HunterShare = "HunterShare";

		public const string MapId = "MapId";

		public const string ModeId = "ModeId";

		public const string GarticWordLanguage = "Gartic.WordLanguage";

		public const string BrowserTransport = "Browser.Transport";

		public const string BrowserSteamScope = "Browser.SteamScope";

		public const string BrowserNameFilter = "Browser.NameFilter";

		public const string BrowserModeFilter = "Browser.ModeFilter";

		public const string BrowserJoinTarget = "Browser.JoinTarget";

		public static string GetString(string key, string fallback = "")
		{
			return PlayerPrefs.GetString("Mimicraft.Lobby." + key, fallback);
		}

		public static int GetInt(string key, int fallback = 0)
		{
			return PlayerPrefs.GetInt("Mimicraft.Lobby." + key, fallback);
		}

		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString("Mimicraft.Lobby." + key, value ?? "");
			PlayerPrefs.Save();
		}

		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt("Mimicraft.Lobby." + key, value);
			PlayerPrefs.Save();
		}

		public static void Bind(TMP_InputField field, string key)
		{
			if (!(field == null))
			{
				string text = GetString(key);
				if (!string.IsNullOrEmpty(text))
				{
					field.SetTextWithoutNotify(text);
				}
				field.onEndEdit.AddListener(delegate(string value)
				{
					SetString(key, value);
				});
			}
		}

		public static void Bind(TMP_Dropdown dropdown, string key)
		{
			if (!(dropdown == null))
			{
				int num = GetInt(key, -1);
				if (num >= 0 && num < dropdown.options.Count)
				{
					dropdown.SetValueWithoutNotify(num);
				}
				dropdown.onValueChanged.AddListener(delegate(int value)
				{
					SetInt(key, value);
				});
			}
		}
	}
}
