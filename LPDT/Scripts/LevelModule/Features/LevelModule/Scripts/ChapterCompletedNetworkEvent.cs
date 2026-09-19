using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class ChapterCompletedNetworkEvent : NetworkEventBase<ChapterCompletedNetworkEvent>
	{
		[field: SerializeField]
		public int ChapterIndex { get; private set; }

		public void SendEvent(int chapterIndex)
		{
			ChapterIndex = chapterIndex;
			Send();
		}
	}
}
