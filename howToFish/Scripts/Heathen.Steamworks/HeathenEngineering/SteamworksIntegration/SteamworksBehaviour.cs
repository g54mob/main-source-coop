using HeathenEngineering.Events;
using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/for-unity-game-engine/components/steamworks-behaviour")]
	[DisallowMultipleComponent]
	public class SteamworksBehaviour : MonoBehaviour
	{
		public SteamSettings settings;

		[Header("Events")]
		public UnityEvent evtSteamInitialized = new UnityEvent();

		public UnityStringEvent evtSteamInitializationError = new UnityStringEvent();

		private void OnEnable()
		{
			App.evtSteamInitialized.AddListener(HandleInitialization);
			App.evtSteamInitializationError.AddListener(HandleInitializationError);
			if (SteamSettings.behaviour == null)
			{
				SteamSettings.behaviour = this;
			}
			settings.Initialize();
		}

		private void OnDestroy()
		{
			App.evtSteamInitialized.RemoveListener(HandleInitialization);
			App.evtSteamInitializationError.RemoveListener(HandleInitializationError);
		}

		public static void CreateIfMissing(SteamSettings settings, bool doNotDestroy = false)
		{
			settings.CreateBehaviour(doNotDestroy);
		}

		private void HandleInitializationError(string message)
		{
			Debug.LogError(message);
			evtSteamInitializationError.Invoke(message);
		}

		private void HandleInitialization()
		{
			evtSteamInitialized.Invoke();
		}
	}
}
