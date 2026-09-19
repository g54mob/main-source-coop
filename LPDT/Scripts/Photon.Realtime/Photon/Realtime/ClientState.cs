namespace Photon.Realtime
{
	public enum ClientState
	{
		PeerCreated = 0,
		Authenticating = 1,
		Authenticated = 2,
		JoiningLobby = 3,
		JoinedLobby = 4,
		DisconnectingFromMasterServer = 5,
		ConnectingToGameServer = 6,
		ConnectedToGameServer = 7,
		Joining = 8,
		Joined = 9,
		Leaving = 10,
		DisconnectingFromGameServer = 11,
		ConnectingToMasterServer = 12,
		Disconnecting = 13,
		Disconnected = 14,
		ConnectedToMasterServer = 15,
		ConnectingToNameServer = 16,
		ConnectedToNameServer = 17,
		DisconnectingFromNameServer = 18,
		ConnectWithFallbackProtocol = 19
	}
}
