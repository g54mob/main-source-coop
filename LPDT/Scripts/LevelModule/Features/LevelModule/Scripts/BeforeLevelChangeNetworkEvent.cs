using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class BeforeLevelChangeNetworkEvent : NetworkEventBase<BeforeLevelChangeNetworkEvent>
	{
		[field: SerializeField]
		public LevelType PastLevelType { get; private set; }

		[field: SerializeField]
		public LevelType NewLevelType { get; private set; }

		public void SendEvent(LevelType pastLevelType, LevelType newLevelType)
		{
			PastLevelType = pastLevelType;
			NewLevelType = newLevelType;
			Send();
		}
	}
}
