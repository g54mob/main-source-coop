using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	[Serializable]
	public class PlayerSpawnReadyNetworkEvent : NetworkEventBase<PlayerSpawnReadyNetworkEvent>
	{
		[field: SerializeField]
		public int ReadyPlayerId { get; private set; }

		public void SendEvent(int readyPlayerId)
		{
			ReadyPlayerId = readyPlayerId;
			Send();
		}
	}
}
