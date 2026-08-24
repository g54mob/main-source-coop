using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class PersonaStateChangeEvent : UnityEvent<PersonaStateChange>
	{
	}
}
