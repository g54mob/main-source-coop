using System.Collections.Generic;

namespace Photon.Realtime
{
	internal class ErrorInfoCallbacksContainer : List<IErrorInfoCallback>, IErrorInfoCallback
	{
		private RealtimeClient client;

		public ErrorInfoCallbacksContainer(RealtimeClient client)
		{
			this.client = client;
		}

		public void OnErrorInfo(ErrorInfo errorInfo)
		{
			Log.Error("OnErrorInfo(" + errorInfo.Info + ")", client.LogLevel, client.LogPrefix);
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnErrorInfo(errorInfo);
				}
			}
			client.CallbackMessage.Raise(new OnErrorInfoMsg(errorInfo));
		}
	}
}
