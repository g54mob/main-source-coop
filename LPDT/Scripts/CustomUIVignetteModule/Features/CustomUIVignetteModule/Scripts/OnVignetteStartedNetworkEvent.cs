using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.CustomUIVignetteModule.Scripts
{
	[Serializable]
	public class OnVignetteStartedNetworkEvent : NetworkEventBase<OnVignetteStartedNetworkEvent>
	{
		[field: SerializeField]
		public int PlayerId { get; private set; }

		[field: SerializeField]
		public float Time { get; private set; }

		[field: SerializeField]
		public float CurrentTime { get; private set; }

		public void SendEvent(int playerId, float time, float currentTime)
		{
			PlayerId = playerId;
			Time = time;
			CurrentTime = currentTime;
			Send();
		}
	}
}
