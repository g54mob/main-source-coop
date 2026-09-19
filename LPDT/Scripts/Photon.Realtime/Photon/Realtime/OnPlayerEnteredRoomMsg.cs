namespace Photon.Realtime
{
	public class OnPlayerEnteredRoomMsg
	{
		public Player newPlayer;

		internal OnPlayerEnteredRoomMsg(Player newPlayer)
		{
			this.newPlayer = newPlayer;
		}
	}
}
