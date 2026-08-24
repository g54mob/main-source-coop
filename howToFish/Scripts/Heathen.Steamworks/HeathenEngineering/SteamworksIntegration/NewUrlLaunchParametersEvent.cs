using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class NewUrlLaunchParametersEvent : UnityEvent<NewUrlLaunchParameters_t>
	{
	}
}
