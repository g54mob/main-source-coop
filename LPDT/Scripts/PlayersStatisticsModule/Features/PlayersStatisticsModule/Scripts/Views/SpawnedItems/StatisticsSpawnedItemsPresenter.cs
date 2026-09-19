using System.Collections.Generic;
using Features.PlayersStatisticsModule.Scripts.Views.Statistics;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public class StatisticsSpawnedItemsPresenter : PresenterBehaviour<StatisticsSpawnedItemsViewBase>
	{
		private static readonly StatisticsSpawnedItemType[] _trackedItemTypes = new StatisticsSpawnedItemType[3]
		{
			StatisticsSpawnedItemType.Couldrone,
			StatisticsSpawnedItemType.Cart,
			StatisticsSpawnedItemType.DeadPart
		};

		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly DiContainer _container;

		private readonly StatisticWindow _statisticWindow;

		private readonly List<StatisticsSpawnedItemPresenter> _itemPresenters = new List<StatisticsSpawnedItemPresenter>();

		public StatisticsSpawnedItemsPresenter(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, DiContainer container, StatisticWindow statisticWindow)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_container = container;
			_statisticWindow = statisticWindow;
		}

		protected override void OnViewSet()
		{
			_levelPlayersGameStatisticsModel.OnSpawnedItemsChanged += OnSpawnedItemsChanged;
		}

		protected override void OnDisposed()
		{
			_levelPlayersGameStatisticsModel.OnSpawnedItemsChanged -= OnSpawnedItemsChanged;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			RefreshItems();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			ClearAll();
		}

		private void OnSpawnedItemsChanged()
		{
			RefreshItems();
		}

		private void RefreshItems()
		{
			if (!base.View.gameObject.activeInHierarchy)
			{
				return;
			}
			ClearAll();
			StatisticsSpawnedItemType[] trackedItemTypes = _trackedItemTypes;
			foreach (StatisticsSpawnedItemType statisticsSpawnedItemType in trackedItemTypes)
			{
				int spawnedItemCount = _levelPlayersGameStatisticsModel.GetSpawnedItemCount(statisticsSpawnedItemType);
				if (spawnedItemCount > 0)
				{
					StatisticsSpawnedItemViewBase statisticsSpawnedItemViewBase = _container.InstantiatePrefabForComponent<StatisticsSpawnedItemViewBase>(base.View.ItemPrefab.gameObject);
					_statisticWindow.AddView(statisticsSpawnedItemViewBase.transform, worldPositionStays: false);
					StatisticsSpawnedItemPresenter presenterForView = _statisticWindow.GetPresenterForView<StatisticsSpawnedItemPresenter>(statisticsSpawnedItemViewBase);
					presenterForView.SetData(statisticsSpawnedItemType, spawnedItemCount);
					_itemPresenters.Add(presenterForView);
					presenterForView.SetParent(base.View.ItemsContainer);
				}
			}
		}

		private void ClearAll()
		{
			for (int i = 0; i < _itemPresenters.Count; i++)
			{
				_itemPresenters[i].DestroyView();
			}
			_itemPresenters.Clear();
		}
	}
}
