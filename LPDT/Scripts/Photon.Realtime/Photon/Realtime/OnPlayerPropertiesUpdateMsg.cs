using Photon.Client;

namespace Photon.Realtime
{
	public class OnPlayerPropertiesUpdateMsg
	{
		public Player targetPlayer;

		public PhotonHashtable changedProps;

		internal OnPlayerPropertiesUpdateMsg(Player targetPlayer, PhotonHashtable changedProps)
		{
			this.targetPlayer = targetPlayer;
			this.changedProps = changedProps;
		}
	}
}
