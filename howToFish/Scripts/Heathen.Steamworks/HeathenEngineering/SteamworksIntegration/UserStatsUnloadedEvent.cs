using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class UserStatsUnloadedEvent : UnityEvent<UserStatsUnloaded_t>
	{
	}
}
