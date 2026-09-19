using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	[Serializable]
	public class NetworkPlayerFollowRequest : NetworkEventBase<NetworkPlayerFollowRequest>
	{
		[field: SerializeField]
		public int PlayerId { get; private set; }

		[field: SerializeField]
		public PlayerFollowEventType EventType { get; private set; }

		public void Request(int playerId, PlayerFollowEventType eventType)
		{
			PlayerId = playerId;
			EventType = eventType;
			Send();
		}
	}
}
