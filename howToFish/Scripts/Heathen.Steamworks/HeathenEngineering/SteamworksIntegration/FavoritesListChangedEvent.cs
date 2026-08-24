using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class FavoritesListChangedEvent : UnityEvent<FavoritesListChanged_t>
	{
	}
}
