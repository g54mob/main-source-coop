using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class UserAchievementStoredEvent : UnityEvent<UserAchievementStored_t>
	{
	}
}
