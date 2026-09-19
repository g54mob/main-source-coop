using System;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts
{
	[Serializable]
	public class StatisticsSpawnedItemIconEntry
	{
		[field: SerializeField]
		public StatisticsSpawnedItemType ItemType { get; private set; }

		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}
