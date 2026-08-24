using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class GameLobbyJoinRequestedEvent : UnityEvent<CSteamID, UserData>
	{
	}
}
