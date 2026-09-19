using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.GameOverModule.Scripts
{
	[Serializable]
	public class GameOverNetworkEvent : NetworkEventBase<GameOverNetworkEvent>
	{
		[field: SerializeField]
		public GameOverReason GameOverReason { get; private set; }

		public void SendEvent(GameOverReason gameOverReason)
		{
			GameOverReason = gameOverReason;
			Send();
		}
	}
}
