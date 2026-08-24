using System;
using HeathenEngineering.Events;
using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Obsolete("Replaced by Steamworks Event Triggers")]
	public class SteamSystemEvents : MonoBehaviour
	{
		[Header("Events")]
		public UnityEvent evtSteamInitialized = new UnityEvent();

		public UnityStringEvent evtSteamInitializationError = new UnityStringEvent();

		private void Awake()
		{
			App.evtSteamInitialized.AddListener(evtSteamInitialized.Invoke);
			App.evtSteamInitializationError.AddListener(evtSteamInitializationError.Invoke);
		}

		private void OnDestroy()
		{
			App.evtSteamInitialized.RemoveListener(evtSteamInitialized.Invoke);
			App.evtSteamInitializationError.RemoveListener(evtSteamInitializationError.Invoke);
		}
	}
}
