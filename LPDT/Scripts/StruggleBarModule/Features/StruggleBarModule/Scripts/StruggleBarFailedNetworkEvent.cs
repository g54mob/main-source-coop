using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.StruggleBarModule.Scripts
{
	[Serializable]
	public class StruggleBarFailedNetworkEvent : NetworkEventBase<StruggleBarFailedNetworkEvent>
	{
		[field: SerializeField]
		public int PlayerId { get; private set; }

		public void SendEvent(int playerId)
		{
			PlayerId = playerId;
			Send();
		}
	}
}
