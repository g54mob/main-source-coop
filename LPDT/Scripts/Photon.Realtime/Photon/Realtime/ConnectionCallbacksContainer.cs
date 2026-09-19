using System.Collections.Generic;

namespace Photon.Realtime
{
	public class ConnectionCallbacksContainer : List<IConnectionCallbacks>, IConnectionCallbacks
	{
		private readonly RealtimeClient client;

		public ConnectionCallbacksContainer(RealtimeClient client)
		{
			this.client = client;
		}

		public void OnConnected()
		{
		}

		public void OnConnectedToMaster()
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnConnectedToMaster();
				}
			}
			client.CallbackMessage.Raise(new OnConnectedToMasterMsg());
		}

		public void OnRegionListReceived(RegionHandler regionHandler)
		{
			_ = client.AppSettings.BestRegionSummaryFromStorage;
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnRegionListReceived(regionHandler);
				}
			}
			client.CallbackMessage.Raise(new OnRegionListReceivedMsg
			{
				regionHandler = regionHandler
			});
		}

		public void OnDisconnected(DisconnectCause cause)
		{
			if (client.Handler != null)
			{
				client.Handler.RemoveInstance();
			}
			_ = string.Empty;
			if (cause != DisconnectCause.ApplicationQuit)
			{
				_ = 15;
			}
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnDisconnected(cause);
				}
			}
			client.CallbackMessage.Raise(new OnDisconnectedMsg
			{
				cause = cause
			});
		}

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnCustomAuthenticationResponse(data);
				}
			}
			client.CallbackMessage.Raise(new OnCustomAuthenticationResponseMsg
			{
				data = data
			});
		}

		public void OnCustomAuthenticationFailed(string debugMessage)
		{
			Log.Error("OnCustomAuthenticationFailed() debugMessage: " + debugMessage, client.LogLevel, client.LogPrefix);
			client.UpdateCallbackTargets();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnCustomAuthenticationFailed(debugMessage);
				}
			}
			client.CallbackMessage.Raise(new OnCustomAuthenticationFailedMsg
			{
				debugMessage = debugMessage
			});
		}
	}
}
