using System.Collections.Generic;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts
{
	[CreateAssetMenu(fileName = "StatisticsSpawnedItemsIconsConfiguration_Default", menuName = "Configurations/PlayersStatistics/StatisticsSpawnedItemsIconsConfiguration")]
	public class StatisticsSpawnedItemsIconsConfiguration : ScriptableObject
	{
		[SerializeField]
		private List<StatisticsSpawnedItemIconEntry> _entries = new List<StatisticsSpawnedItemIconEntry>();

		public Sprite GetIcon(StatisticsSpawnedItemType itemType)
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				if (_entries[i].ItemType == itemType)
				{
					return _entries[i].Icon;
				}
			}
			return null;
		}
	}
}
