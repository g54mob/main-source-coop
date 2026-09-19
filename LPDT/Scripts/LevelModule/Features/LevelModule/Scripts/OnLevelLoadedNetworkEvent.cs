using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class OnLevelLoadedNetworkEvent : NetworkEventBase<OnLevelLoadedNetworkEvent>
	{
		[field: SerializeField]
		public LevelType TargetLevelType { get; private set; }

		[field: SerializeField]
		public int PlayerID { get; private set; }

		public void SendEvent(LevelType targetLevelType, int playerID)
		{
			TargetLevelType = targetLevelType;
			PlayerID = playerID;
			Send();
		}
	}
}
