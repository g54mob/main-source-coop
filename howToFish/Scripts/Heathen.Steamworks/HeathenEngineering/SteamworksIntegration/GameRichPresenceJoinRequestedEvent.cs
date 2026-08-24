using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class GameRichPresenceJoinRequestedEvent : UnityEvent<UserData, string>
	{
	}
}
