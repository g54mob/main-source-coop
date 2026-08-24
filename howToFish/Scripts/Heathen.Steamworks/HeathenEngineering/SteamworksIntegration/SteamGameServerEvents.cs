using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	public class SteamGameServerEvents : MonoBehaviour
	{
		public App.Server.DisconnectedEvent evtDisconnected;

		public App.Server.ConnectedEvent evtConnected;

		public App.Server.FailureEvent evtFailure;

		private void Awake()
		{
			App.Server.eventDisconnected.AddListener(evtDisconnected.Invoke);
			App.Server.eventConnected.AddListener(evtConnected.Invoke);
			App.Server.eventFailure.AddListener(evtFailure.Invoke);
		}

		private void OnDestroy()
		{
			App.Server.eventDisconnected.RemoveListener(evtDisconnected.Invoke);
			App.Server.eventConnected.RemoveListener(evtConnected.Invoke);
			App.Server.eventFailure.RemoveListener(evtFailure.Invoke);
		}
	}
}
