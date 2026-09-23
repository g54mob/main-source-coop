using System;
using Mimicraft.Localization;
using Steamworks;
using Steamworks.Data;

namespace Mimicraft.Networking
{
	public static class SteamLobbyData
	{
		public const string CountryKey = "country";

		public const string PhaseKey = "phase";

		public const string MinPlayersKey = "min_players";

		public const string ScoreLimitKey = "score_limit";

		public const string TimeLimitKey = "time_limit_seconds";

		public const string ExtraWarmupKey = "extra_warmup_seconds";

		public const string RespawnKey = "respawn_seconds";

		public static void Write(Lobby lobby, LobbySettingsData data)
		{
			lobby.SetData("name", data.LobbyName.ToString());
			lobby.SetData("prep_seconds", data.PrepSeconds.ToString());
			lobby.SetData("hunt_seconds", data.HuntSeconds.ToString());
			lobby.SetData("round_end_seconds", data.RoundEndSeconds.ToString());
			lobby.SetData("min_hiders", data.MinHiders.ToString());
			lobby.SetData("min_hunters", data.MinHunters.ToString());
			lobby.SetData("min_players", data.MinPlayers.ToString());
			lobby.SetData("score_limit", data.ScoreLimit.ToString());
			lobby.SetData("time_limit_seconds", data.TimeLimitSeconds.ToString());
			lobby.SetData("extra_warmup_seconds", data.ExtraWarmupSeconds.ToString());
			lobby.SetData("respawn_seconds", data.RespawnSeconds.ToString());
			lobby.SetData("map", data.MapId.ToString());
			lobby.SetData("mode", data.ModeId.ToString());
			lobby.SetData("has_password", data.HasPassword ? "1" : "0");
			lobby.SetData("ping_location", SteamPing.LocalLocation);
			lobby.SetData("country", LocalCountry());
			lobby.SetData("lang", Loc.Language);
			lobby.SetData("tickrate", ServerTickRate.Preferred.ToString());
			lobby.SetData("version", GameVersion.Current);
			lobby.SetData("phase", "waiting");
		}

		public static void Republish(LobbySettingsData data)
		{
			if (!SteamClient.IsValid)
			{
				return;
			}
			Lobby? activeLobby = SteamManager.ActiveLobby;
			if (!activeLobby.HasValue)
			{
				return;
			}
			try
			{
				if ((ulong)activeLobby.Value.Owner.Id == (ulong)SteamClient.SteamId)
				{
					Write(activeLobby.Value, data);
				}
			}
			catch (Exception)
			{
			}
		}

		public static void PublishPhase(string phase)
		{
			if (!SteamClient.IsValid)
			{
				return;
			}
			Lobby? activeLobby = SteamManager.ActiveLobby;
			if (!activeLobby.HasValue)
			{
				return;
			}
			try
			{
				if ((ulong)activeLobby.Value.Owner.Id == (ulong)SteamClient.SteamId)
				{
					activeLobby.Value.SetData("phase", phase ?? "");
				}
			}
			catch (Exception)
			{
			}
		}

		private static string LocalCountry()
		{
			try
			{
				return SteamClient.IsValid ? (SteamUtils.IpCountry ?? "") : "";
			}
			catch (Exception)
			{
				return "";
			}
		}
	}
}
