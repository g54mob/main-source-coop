using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class GameServerChangeRequestedEvent : UnityEvent<string, string>
	{
	}
}
