namespace Features.MainMenuModule.Scripts
{
	public readonly struct QuickJoinResult
	{
		public static readonly QuickJoinResult NotFound = new QuickJoinResult(found: false, null, null, 0, 0, null, -1);

		public bool Found { get; }

		public string SessionName { get; }

		public string HostName { get; }

		public int PlayerCount { get; }

		public int MaxPlayers { get; }

		public string Region { get; }

		public int RegionPing { get; }

		public QuickJoinResult(bool found, string sessionName, string hostName, int playerCount, int maxPlayers, string region, int regionPing)
		{
			Found = found;
			SessionName = sessionName;
			HostName = hostName;
			PlayerCount = playerCount;
			MaxPlayers = maxPlayers;
			Region = region;
			RegionPing = regionPing;
		}
	}
}
