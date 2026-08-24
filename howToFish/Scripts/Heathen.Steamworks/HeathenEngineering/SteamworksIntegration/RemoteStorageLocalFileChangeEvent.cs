using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class RemoteStorageLocalFileChangeEvent : UnityEvent<RemoteStorageLocalFileChange_t>
	{
	}
}
