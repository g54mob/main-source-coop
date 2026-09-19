using System.Collections.Generic;

namespace Photon.Realtime
{
	internal class LobbyCallbacksContainer : List<ILobbyCallbacks>, ILobbyCallbacks
	{
		private readonly RealtimeClient client;

		public LobbyCallbacksContainer(RealtimeClient client)
		{
			this.client = client;
		}

		public void OnJoinedLobby()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnJoinedLobby();
				}
			}
			client.CallbackMessage.Raise(new OnJoinedLobbyMsg());
		}

		public void OnLeftLobby()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnLeftLobby();
				}
			}
			client.CallbackMessage.Raise(new OnLeftLobbyMsg());
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnRoomListUpdate(roomList);
				}
			}
			client.CallbackMessage.Raise(new OnRoomListUpdateMsg(roomList));
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnLobbyStatisticsUpdate(lobbyStatistics);
				}
			}
			client.CallbackMessage.Raise(new OnLobbyStatisticsUpdateMsg(lobbyStatistics));
		}
	}
}
