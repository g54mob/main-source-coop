using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	[Serializable]
	public class PlayerBodyEmoteNetworkEvent : NetworkEventBase<PlayerBodyEmoteNetworkEvent>
	{
		[field: SerializeField]
		public int PlayerId { get; private set; }

		[field: SerializeField]
		public BodyEmoteType BodyEmoteType { get; private set; }

		[field: SerializeField]
		public bool IsStarted { get; private set; }

		public void SendEvent(int playerId, BodyEmoteType bodyEmoteType, bool isStarted)
		{
			PlayerId = playerId;
			BodyEmoteType = bodyEmoteType;
			IsStarted = isStarted;
			Send();
		}
	}
}
