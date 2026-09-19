using System.Collections.Generic;

namespace Photon.Realtime
{
	public class MatchMakingCallbacksContainer : List<IMatchmakingCallbacks>, IMatchmakingCallbacks
	{
		private readonly RealtimeClient client;

		public MatchMakingCallbacksContainer(RealtimeClient client)
		{
			this.client = client;
		}

		public void OnCreatedRoom()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnCreatedRoom();
				}
			}
			client.CallbackMessage.Raise(new OnCreatedRoomMsg());
		}

		public void OnJoinedRoom()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnJoinedRoom();
				}
			}
			client.CallbackMessage.Raise(new OnJoinedRoomMsg());
		}

		public void OnCreateRoomFailed(short returnCode, string message)
		{
			Log.Error($"OnCreateRoomFailed({returnCode}, \"{message}\")", client.LogLevel, client.LogPrefix);
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnCreateRoomFailed(returnCode, message);
				}
			}
			client.CallbackMessage.Raise(new OnCreateRoomFailedMsg(returnCode, message));
		}

		public void OnJoinRandomFailed(short returnCode, string message)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnJoinRandomFailed(returnCode, message);
				}
			}
			client.CallbackMessage.Raise(new OnJoinRandomFailedMsg(returnCode, message));
		}

		public void OnJoinRoomFailed(short returnCode, string message)
		{
			Log.Error($"OnJoinRoomFailed({returnCode}, \"{message}\")", client.LogLevel, client.LogPrefix);
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnJoinRoomFailed(returnCode, message);
				}
			}
			client.CallbackMessage.Raise(new OnJoinRoomFailedMsg(returnCode, message));
		}

		public void OnLeftRoom()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnLeftRoom();
				}
			}
			client.CallbackMessage.Raise(new OnLeftRoomMsg());
		}

		public void OnFriendListUpdate(List<FriendInfo> friendList)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnFriendListUpdate(friendList);
				}
			}
			client.CallbackMessage.Raise(new OnFriendListUpdateMsg(friendList));
		}
	}
}
