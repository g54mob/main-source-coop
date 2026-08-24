using HeathenEngineering.Events;
using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[HelpURL("https://kb.heathenengineering.com/assets/steamworks")]
	[DisallowMultipleComponent]
	public class SteamworksCreator : MonoBehaviour
	{
		public bool createOnStart;

		public bool markAsDoNotDestroy;

		public SteamSettings settings;

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
			if (SteamSettings.behaviour != null)
			{
				SteamSettings.behaviour.evtSteamInitialized.RemoveListener(evtSteamInitialized.Invoke);
				SteamSettings.behaviour.evtSteamInitializationError.RemoveListener(evtSteamInitializationError.Invoke);
			}
		}

		private void Start()
		{
			if (createOnStart)
			{
				settings.CreateBehaviour(markAsDoNotDestroy, evtSteamInitialized.Invoke, evtSteamInitializationError.Invoke);
			}
		}

		public void CreateIfMissing()
		{
			settings.CreateBehaviour(markAsDoNotDestroy, evtSteamInitialized.Invoke, evtSteamInitializationError.Invoke);
		}
	}
}
