namespace Photon.Realtime
{
	public class TypedLobbyInfo : TypedLobby
	{
		public int PlayerCount { get; private set; }

		public int RoomCount { get; private set; }

		internal TypedLobbyInfo(string name, LobbyType type, int playerCount, int roomCount)
		{
			base.Name = name;
			base.Type = type;
			PlayerCount = playerCount;
			RoomCount = roomCount;
		}

		public override string ToString()
		{
			return $"LobbyInfo '{base.Name}'[{base.Type}] rooms: {RoomCount}, players: {PlayerCount}]";
		}
	}
}
