using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public class StatisticsSpawnedItemPresenter : PresenterBehaviour<StatisticsSpawnedItemViewBase>
	{
		private readonly StatisticsSpawnedItemsIconsConfiguration _iconsConfiguration;

		public StatisticsSpawnedItemPresenter(StatisticsSpawnedItemsIconsConfiguration iconsConfiguration)
		{
			_iconsConfiguration = iconsConfiguration;
		}

		public void SetData(StatisticsSpawnedItemType itemType, int count)
		{
			base.View.SetIcon(_iconsConfiguration.GetIcon(itemType));
			base.View.SetCount(count);
		}

		public void DestroyView()
		{
			Object.Destroy(base.View.gameObject);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent);
		}
	}
}
