namespace EvilCore.Networking
{
	public struct LobbySearchResult
	{
		public string LobbyId;

		public string LobbyName;

		public string HostPuid;

		public uint MemberCount;

		public uint MaxMembers;

		public string Version;

		public string GameMode;

		public RegionCode Region;

		public bool IsPublic;

		public bool HasPassword;

		public string PasswordHash;

		public string JoinCode;

		public string BannedMembers;
	}
}
