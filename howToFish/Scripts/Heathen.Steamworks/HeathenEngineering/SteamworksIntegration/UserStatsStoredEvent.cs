using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class UserStatsStoredEvent : UnityEvent<UserStatsStored_t>
	{
	}
}
