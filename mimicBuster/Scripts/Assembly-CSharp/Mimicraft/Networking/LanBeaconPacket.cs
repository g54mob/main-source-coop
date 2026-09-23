using System.Globalization;

namespace Mimicraft.Networking
{
	public static class LanBeaconPacket
	{
		private const string Magic = "MIMICRAFT_LOBBY";

		private const char Delimiter = '|';

		private const int MinimumFields = 8;

		private const int MapIdIndex = 8;

		private const int ModeIdIndex = 9;

		private const int HasPasswordIndex = 10;

		private const int PlayersIndex = 11;

		private const int MaxPlayersIndex = 12;

		private const int MinPlayersIndex = 13;

		private const int ScoreLimitIndex = 14;

		private const int TimeLimitIndex = 15;

		private const int ExtraWarmupIndex = 16;

		private const int RespawnTimeIndex = 17;

		private const int TickRateIndex = 18;

		private const int VersionIndex = 19;

		private const int PhaseIndex = 20;

		public static string Serialize(LobbySettingsData settings, ushort port, int players, int maxPlayers, string phase)
		{
			return string.Join('|'.ToString(), "MIMICRAFT_LOBBY", settings.LobbyName.ToString(), port.ToString(CultureInfo.InvariantCulture), settings.PrepSeconds.ToString(CultureInfo.InvariantCulture), settings.HuntSeconds.ToString(CultureInfo.InvariantCulture), settings.RoundEndSeconds.ToString(CultureInfo.InvariantCulture), settings.MinHiders.ToString(CultureInfo.InvariantCulture), settings.MinHunters.ToString(CultureInfo.InvariantCulture), settings.MapId.ToString(), settings.ModeId.ToString(), settings.HasPassword ? "1" : "0", players.ToString(CultureInfo.InvariantCulture), maxPlayers.ToString(CultureInfo.InvariantCulture), settings.MinPlayers.ToString(CultureInfo.InvariantCulture), settings.ScoreLimit.ToString(CultureInfo.InvariantCulture), settings.TimeLimitSeconds.ToString(CultureInfo.InvariantCulture), settings.ExtraWarmupSeconds.ToString(CultureInfo.InvariantCulture), settings.RespawnSeconds.ToString(CultureInfo.InvariantCulture), ServerTickRate.Active.ToString(CultureInfo.InvariantCulture), GameVersion.Current, phase ?? "");
		}

		public static string VersionOf(string raw)
		{
			if (!string.IsNullOrEmpty(raw))
			{
				return Field(raw.Split('|'), 19);
			}
			return "";
		}

		public static string PhaseOf(string raw)
		{
			if (!string.IsNullOrEmpty(raw))
			{
				return Field(raw.Split('|'), 20);
			}
			return "";
		}

		public static string TickRateOf(string raw)
		{
			if (!string.IsNullOrEmpty(raw))
			{
				return Field(raw.Split('|'), 18);
			}
			return "";
		}

		public static bool TryParse(string raw, out LobbySettingsData settings, out ushort port, out int players, out int maxPlayers)
		{
			settings = default(LobbySettingsData);
			port = 0;
			players = 0;
			maxPlayers = 0;
			if (string.IsNullOrEmpty(raw))
			{
				return false;
			}
			string[] array = raw.Split('|');
			if (array.Length < 8 || array[0] != "MIMICRAFT_LOBBY")
			{
				return false;
			}
			if (!ushort.TryParse(array[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out port))
			{
				return false;
			}
			settings = new LobbySettingsData
			{
				LobbyName = array[1],
				PrepSeconds = ParseIntOrZero(array[3]),
				HuntSeconds = ParseIntOrZero(array[4]),
				RoundEndSeconds = ParseIntOrZero(array[5]),
				MinHiders = ParseIntOrZero(array[6]),
				MinHunters = ParseIntOrZero(array[7]),
				MapId = Field(array, 8),
				ModeId = Field(array, 9),
				HasPassword = (Field(array, 10) == "1")
			};
			settings.MinPlayers = ParseIntOrZero(Field(array, 13));
			settings.ScoreLimit = ParseIntOrZero(Field(array, 14));
			settings.TimeLimitSeconds = ParseIntOrZero(Field(array, 15));
			settings.ExtraWarmupSeconds = ParseIntOrZero(Field(array, 16));
			settings.RespawnSeconds = ParseIntOrZero(Field(array, 17));
			players = ParseIntOrZero(Field(array, 11));
			maxPlayers = ParseIntOrZero(Field(array, 12));
			return true;
		}

		private static string Field(string[] parts, int index)
		{
			if (index >= parts.Length)
			{
				return "";
			}
			return parts[index];
		}

		private static int ParseIntOrZero(string text)
		{
			if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				return 0;
			}
			return result;
		}
	}
}
