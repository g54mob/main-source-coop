namespace Photon.Realtime
{
	public class OnCreateRoomFailedMsg
	{
		public short returnCode;

		public string message;

		internal OnCreateRoomFailedMsg(short returnCode, string message)
		{
			this.returnCode = returnCode;
			this.message = message;
		}
	}
}
