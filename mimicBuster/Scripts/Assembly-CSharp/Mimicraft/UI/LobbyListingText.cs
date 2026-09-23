using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class LobbyListingText
	{
		private const string ValueColumn = "<pos=52%>";

		private const string CaptionTint = "<color=#FFFFFFA6>";

		private const int GoodPingMs = 80;

		private const int FairPingMs = 160;

		public static string Mode(in LobbyListing listing)
		{
			if (!string.IsNullOrEmpty(listing.ModeId))
			{
				return GameModeCatalog.DisplayName(listing.ModeId);
			}
			return "";
		}

		public static string Map(in LobbyListing listing)
		{
			if (!string.IsNullOrEmpty(listing.MapId))
			{
				return MapCatalog.DisplayName(listing.MapId);
			}
			return "";
		}

		public static Sprite MapPreview(in LobbyListing listing)
		{
			if (string.IsNullOrEmpty(listing.MapId))
			{
				return null;
			}
			MapScriptableObject mapScriptableObject = MapCatalog.Find(listing.MapId);
			if (!(mapScriptableObject != null))
			{
				return null;
			}
			return mapScriptableObject.Preview;
		}

		public static string Phase(in LobbyListing listing)
		{
			string text = LobbyPhase.LabelKey(listing.Phase);
			if (!string.IsNullOrEmpty(text))
			{
				return Loc.Get(text);
			}
			return "";
		}

		public static string Players(in LobbyListing listing)
		{
			if (listing.MaxPlayers <= 0)
			{
				return listing.Players.ToString();
			}
			return Loc.Format("LobbyBrowser.PlayerCount", listing.Players, listing.MaxPlayers);
		}

		public static string Seconds(int seconds)
		{
			return Loc.Format("Lobby.Row.Seconds", seconds);
		}

		public static string Ping(in LobbyListing listing)
		{
			return Ping(listing.PingMs);
		}

		public static string Ping(int ms)
		{
			if (ms >= 0)
			{
				return Loc.Format("LobbyBrowser.PingMs", ms);
			}
			return Loc.Get("LobbyBrowser.PingUnknown");
		}

		public static string PingColored(in LobbyListing listing)
		{
			string text = Ping(listing.PingMs);
			int pingMs = listing.PingMs;
			string text2 = ((pingMs < 0) ? "#FFFFFF80" : ((pingMs <= 80) ? "#6FD86F" : ((pingMs <= 160) ? "#E8C15A" : "#E06C5A")));
			return "<color=" + text2 + ">" + text + "</color>";
		}

		public static string Password(in LobbyListing listing)
		{
			return Loc.Get(listing.HasPassword ? "Lobby.Locked" : "Lobby.Unlocked");
		}

		public static string Transport(in LobbyListing listing)
		{
			if (!listing.OverSteam)
			{
				return "Direct";
			}
			return "Steam";
		}

		public static string Country(in LobbyListing listing)
		{
			if (!string.IsNullOrWhiteSpace(listing.Country))
			{
				return listing.Country.Trim().ToUpperInvariant();
			}
			return "";
		}

		public static void Rows(in LobbyListing listing, List<GameModeController.LobbyInfoRow> rows)
		{
			GameModeDefinition mode = (string.IsNullOrEmpty(listing.ModeId) ? null : GameModeCatalog.Find(listing.ModeId));
			Add(rows, Loc.Get("Lobby.Row.Phase"), Phase(in listing));
			Add(rows, Loc.Get("Lobby.Row.Mode"), Mode(in listing));
			Add(rows, Loc.Get("Lobby.Row.Map"), Map(in listing));
			Add(rows, Loc.Get("Lobby.Row.Players"), Players(in listing));
			if (Uses(mode, LobbySettingFields.PrepSeconds))
			{
				Add(rows, Loc.Get("Lobby.Row.Preparation"), Duration(listing.PrepSeconds));
			}
			if (Uses(mode, LobbySettingFields.HuntSeconds))
			{
				Add(rows, Loc.Get("Lobby.Row.Hunt"), Duration(listing.HuntSeconds));
			}
			if (Uses(mode, LobbySettingFields.RoundEndSeconds))
			{
				Add(rows, Loc.Get("Lobby.Row.RoundEnd"), Duration(listing.RoundEndSeconds));
			}
			if (Uses(mode, LobbySettingFields.MinHiders))
			{
				Add(rows, Loc.Get("Lobby.Row.MinModelers"), Count(listing.MinHiders));
			}
			if (Uses(mode, LobbySettingFields.MinHunters))
			{
				Add(rows, Loc.Get("Lobby.Row.MinHunters"), Count(listing.MinHunters));
			}
			if (listing.PingMs >= 0)
			{
				Add(rows, Loc.Get("Lobby.Row.Ping"), Ping(in listing));
			}
			Add(rows, Loc.Get("Lobby.Password"), Password(in listing));
			Add(rows, Loc.Get("Lobby.Row.Connection"), Transport(in listing));
			Add(rows, Loc.Get("Lobby.Row.Address"), listing.Target);
		}

		private static bool Uses(GameModeDefinition mode, LobbySettingFields field)
		{
			if (!(mode == null))
			{
				return mode.Uses(field);
			}
			return true;
		}

		public static string Summary(in LobbyListing listing)
		{
			List<GameModeController.LobbyInfoRow> list = new List<GameModeController.LobbyInfoRow>();
			Rows(in listing, list);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameModeController.LobbyInfoRow item in list)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('\n');
				}
				stringBuilder.Append("<color=#FFFFFFA6>").Append(item.Caption).Append("</color>")
					.Append("<pos=52%>")
					.Append(item.Value);
			}
			return stringBuilder.ToString();
		}

		private static string Duration(int seconds)
		{
			if (seconds <= 0)
			{
				return "";
			}
			return Seconds(seconds);
		}

		private static string Count(int value)
		{
			if (value <= 0)
			{
				return "";
			}
			return value.ToString();
		}

		private static void Add(List<GameModeController.LobbyInfoRow> rows, string caption, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				rows.Add(new GameModeController.LobbyInfoRow(caption, value));
			}
		}
	}
}
