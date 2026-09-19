namespace Photon.Realtime
{
	public class OnJoinRoomFailedMsg
	{
		public short returnCode;

		public string message;

		internal OnJoinRoomFailedMsg(short returnCode, string message)
		{
			this.returnCode = returnCode;
			this.message = message;
		}
	}
}
