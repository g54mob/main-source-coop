namespace Mimicraft.Networking
{
	public readonly struct LobbyListing
	{
		public readonly string Name;

		public readonly string ModeId;

		public readonly string MapId;

		public readonly int PrepSeconds;

		public readonly int HuntSeconds;

		public readonly int RoundEndSeconds;

		public readonly int MinHiders;

		public readonly int MinHunters;

		public readonly int Players;

		public readonly int MaxPlayers;

		public readonly bool HasPassword;

		public readonly bool OverSteam;

		public readonly string Target;

		public readonly int PingMs;

		public readonly string Language;

		public readonly string Country;

		public readonly string Phase;

		public LobbyListing(string name, string modeId, string mapId, LobbySettingsData settings, int players, int maxPlayers, bool overSteam, string target, int pingMs = -1, string language = "", string country = "", string phase = "")
		{
			PingMs = pingMs;
			Language = language ?? "";
			Country = country ?? "";
			Phase = phase ?? "";
			Name = (string.IsNullOrEmpty(name) ? "" : name);
			ModeId = modeId ?? "";
			MapId = mapId ?? "";
			PrepSeconds = settings.PrepSeconds;
			HuntSeconds = settings.HuntSeconds;
			RoundEndSeconds = settings.RoundEndSeconds;
			MinHiders = settings.MinHiders;
			MinHunters = settings.MinHunters;
			HasPassword = settings.HasPassword;
			Players = players;
			MaxPlayers = maxPlayers;
			OverSteam = overSteam;
			Target = target ?? "";
		}
	}
}
