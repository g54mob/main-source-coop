using Photon.Client;

namespace Photon.Realtime
{
	public interface IInRoomCallbacks
	{
		void OnPlayerEnteredRoom(Player newPlayer);

		void OnPlayerLeftRoom(Player otherPlayer);

		void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged);

		void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps);

		void OnMasterClientSwitched(Player newMasterClient);
	}
}
