namespace Photon.Realtime
{
	public class EnterRoomArgs
	{
		public string RoomName;

		public RoomOptions RoomOptions;

		public TypedLobby Lobby;

		public string[] ExpectedUsers;

		public object Ticket;

		protected internal bool OnGameServer = true;

		protected internal JoinMode JoinMode;

		internal static EnterRoomArgs ShallowCopyToNewArgs(EnterRoomArgs o)
		{
			EnterRoomArgs enterRoomArgs = new EnterRoomArgs();
			if (o != null)
			{
				enterRoomArgs.RoomName = o.RoomName;
				enterRoomArgs.RoomOptions = o.RoomOptions;
				enterRoomArgs.Lobby = o.Lobby;
				enterRoomArgs.ExpectedUsers = o.ExpectedUsers;
				enterRoomArgs.Ticket = o.Ticket;
				enterRoomArgs.JoinMode = o.JoinMode;
			}
			return enterRoomArgs;
		}
	}
}
