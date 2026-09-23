namespace Mimicraft.Networking
{
	public struct LanLobbyInfo
	{
		public string Ip;

		public ushort Port;

		public LobbySettingsData Settings;

		public float LastSeenTime;

		public int Players;

		public int MaxPlayers;

		public string TickRate;

		public string Version;

		public string Phase;
	}
}
