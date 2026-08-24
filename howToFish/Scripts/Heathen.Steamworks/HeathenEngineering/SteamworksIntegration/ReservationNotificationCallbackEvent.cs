using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class ReservationNotificationCallbackEvent : UnityEvent<ReservationNotificationCallback_t>
	{
	}
}
