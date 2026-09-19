using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	[Serializable]
	public class PlayerSpawnRequestNetworkEvent : NetworkEventBase<PlayerSpawnRequestNetworkEvent>
	{
		[field: SerializeField]
		public int RequestingPlayerId { get; private set; }

		[field: SerializeField]
		public bool HasReconnectSpawnPosition { get; private set; }

		[field: SerializeField]
		public Vector3 ReconnectSpawnPosition { get; private set; }

		public void SendEvent(int requestingPlayerId)
		{
			RequestingPlayerId = requestingPlayerId;
			HasReconnectSpawnPosition = false;
			ReconnectSpawnPosition = default(Vector3);
			Send();
		}

		public void SendEvent(int requestingPlayerId, Vector3 reconnectSpawnPosition)
		{
			RequestingPlayerId = requestingPlayerId;
			HasReconnectSpawnPosition = true;
			ReconnectSpawnPosition = reconnectSpawnPosition;
			Send();
		}
	}
}
