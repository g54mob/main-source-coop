using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class SteamMicroTransactionAuthorizationResponce : UnityEvent<AppId_t, ulong, bool>
	{
	}
}
