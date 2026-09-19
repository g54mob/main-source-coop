using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public class StatisticsSpawnedItemsView : StatisticsSpawnedItemsViewBase
	{
		[SerializeField]
		private Transform _itemsContainer;

		[SerializeField]
		private StatisticsSpawnedItemViewBase _itemPrefab;

		public override Transform ItemsContainer => _itemsContainer;

		public override StatisticsSpawnedItemViewBase ItemPrefab => _itemPrefab;
	}
}
