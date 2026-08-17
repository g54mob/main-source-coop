namespace EvilCore.Networking
{
	public struct LobbyCreateOptions
	{
		public string LobbyName;

		public uint MaxPlayers;

		public bool IsPublic;

		public string Password;

		public int Seed;
	}
}
