using System.Collections.Generic;
using Photon.Client;

namespace Photon.Realtime
{
	internal class InRoomCallbacksContainer : List<IInRoomCallbacks>, IInRoomCallbacks
	{
		private readonly RealtimeClient client;

		public InRoomCallbacksContainer(RealtimeClient client)
		{
			this.client = client;
		}

		public void OnPlayerEnteredRoom(Player newPlayer)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnPlayerEnteredRoom(newPlayer);
				}
			}
			client.CallbackMessage.Raise(new OnPlayerEnteredRoomMsg(newPlayer));
		}

		public void OnPlayerLeftRoom(Player otherPlayer)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnPlayerLeftRoom(otherPlayer);
				}
			}
			client.CallbackMessage.Raise(new OnPlayerLeftRoomMsg(otherPlayer));
		}

		public void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnRoomPropertiesUpdate(propertiesThatChanged);
				}
			}
			client.CallbackMessage.Raise(new OnRoomPropertiesUpdateMsg(propertiesThatChanged));
		}

		public void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProp)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnPlayerPropertiesUpdate(targetPlayer, changedProp);
				}
			}
			client.CallbackMessage.Raise(new OnPlayerPropertiesUpdateMsg(targetPlayer, changedProp));
		}

		public void OnMasterClientSwitched(Player newMasterClient)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnMasterClientSwitched(newMasterClient);
				}
			}
			client.CallbackMessage.Raise(new OnMasterClientSwitchedMsg(newMasterClient));
		}
	}
}
