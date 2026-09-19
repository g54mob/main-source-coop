using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.CustomUIVignetteModule.Scripts
{
	[Serializable]
	public class OnVignetteDisabledNetworkEvent : NetworkEventBase<OnVignetteDisabledNetworkEvent>
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
