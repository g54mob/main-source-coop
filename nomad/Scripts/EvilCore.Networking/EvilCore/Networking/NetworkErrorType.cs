namespace EvilCore.Networking
{
	public enum NetworkErrorType
	{
		Unknown = 0,
		ConnectionLost = 1,
		ConnectionTimeout = 2,
		ConnectionFailed = 3,
		ServerFull = 4,
		LobbyNotFound = 5,
		LobbyCreateFailed = 6,
		LobbyJoinFailed = 7,
		AuthenticationFailed = 8,
		RateLimited = 9,
		VoiceConnectFailed = 10
	}
}
