using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public abstract class StatisticsSpawnedItemsViewBase : ViewBehaviour
	{
		public abstract Transform ItemsContainer { get; }

		public abstract StatisticsSpawnedItemViewBase ItemPrefab { get; }
	}
}
