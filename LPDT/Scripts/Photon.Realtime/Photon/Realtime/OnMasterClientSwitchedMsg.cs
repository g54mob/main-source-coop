namespace Photon.Realtime
{
	public class OnMasterClientSwitchedMsg
	{
		public Player newMasterClient;

		internal OnMasterClientSwitchedMsg(Player newMasterClient)
		{
			this.newMasterClient = newMasterClient;
		}
	}
}
