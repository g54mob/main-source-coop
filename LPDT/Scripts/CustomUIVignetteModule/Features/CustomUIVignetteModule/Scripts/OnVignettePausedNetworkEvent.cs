using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.CustomUIVignetteModule.Scripts
{
	[Serializable]
	public class OnVignettePausedNetworkEvent : NetworkEventBase<OnVignettePausedNetworkEvent>
	{
		[field: SerializeField]
		public int PlayerId { get; private set; }

		[field: SerializeField]
		public bool IsPaused { get; private set; }

		public void SendEvent(int playerId, bool isPaused)
		{
			PlayerId = playerId;
			IsPaused = isPaused;
			Send();
		}
	}
}
