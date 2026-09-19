namespace Photon.Realtime
{
	public class OnErrorInfoMsg
	{
		public ErrorInfo errorInfo;

		internal OnErrorInfoMsg(ErrorInfo errorInfo)
		{
			this.errorInfo = errorInfo;
		}
	}
}
