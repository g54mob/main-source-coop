using Photon.Client;

namespace Photon.Realtime
{
	public class JoinRandomRoomArgs
	{
		public PhotonHashtable ExpectedCustomRoomProperties;

		public int ExpectedMaxPlayers;

		public MatchmakingMode MatchingType;

		public TypedLobby Lobby;

		public string SqlLobbyFilter;

		public string[] ExpectedUsers;

		public object Ticket;
	}
}
