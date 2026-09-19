using System;

namespace Photon.Realtime
{
	public enum LobbyType : byte
	{
		Default = 0,
		Sql = 2,
		[Obsolete("Use LobbyType.Sql")]
		SqlLobby = 2,
		AsyncRandom = 3,
		[Obsolete("Use LobbyType.AsyncRandom")]
		AsyncRandomLobby = 3
	}
}
