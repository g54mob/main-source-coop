namespace Photon.Realtime
{
	public class OnJoinRandomFailedMsg
	{
		public short returnCode;

		public string message;

		internal OnJoinRandomFailedMsg(short returnCode, string message)
		{
			this.returnCode = returnCode;
			this.message = message;
		}
	}
}
