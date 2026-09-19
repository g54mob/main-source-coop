namespace Photon.Realtime
{
	public class OnPlayerLeftRoomMsg
	{
		public Player otherPlayer;

		internal OnPlayerLeftRoomMsg(Player otherPlayer)
		{
			this.otherPlayer = otherPlayer;
		}
	}
}
