using System;
using System.Collections.Generic;
using System.Linq;
using Features.GrabModule.Scripts;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class FreeContainerItemsTrackSystem : IInitializable, IDisposable
	{
		private readonly QuotaContainerModel _quotaContainerModel;

		private readonly Dictionary<IPointGrabable, QuotaContainerItemData> _trackedItems = new Dictionary<IPointGrabable, QuotaContainerItemData>();

		public FreeContainerItemsTrackSystem(QuotaContainerModel quotaContainerModel)
		{
			_quotaContainerModel = quotaContainerModel;
		}

		public void Initialize()
		{
			_quotaContainerModel.OnContainerItemsUpdated += OnContainerItemsUpdated;
		}

		public void Dispose()
		{
			_quotaContainerModel.OnContainerItemsUpdated -= OnContainerItemsUpdated;
			foreach (KeyValuePair<IPointGrabable, QuotaContainerItemData> trackedItem in _trackedItems)
			{
				if (trackedItem.Key != null)
				{
					trackedItem.Key.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
				}
			}
			_trackedItems.Clear();
		}

		private void OnContainerItemsUpdated()
		{
			UpdateTrackedItems();
			UpdateFreeContainerItems();
		}

		private void UpdateTrackedItems()
		{
			if (_quotaContainerModel.AllContainerItems == null)
			{
				return;
			}
			HashSet<IPointGrabable> currentItems = new HashSet<IPointGrabable>();
			foreach (QuotaContainerItemData allContainerItem in _quotaContainerModel.AllContainerItems)
			{
				if (allContainerItem.PointGrabable != null)
				{
					currentItems.Add(allContainerItem.PointGrabable);
					if (_trackedItems.TryAdd(allContainerItem.PointGrabable, allContainerItem))
					{
						allContainerItem.PointGrabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
					}
				}
			}
			foreach (IPointGrabable item in _trackedItems.Keys.Where((IPointGrabable key) => !currentItems.Contains(key)).ToList())
			{
				item.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
				_trackedItems.Remove(item);
			}
		}

		private void OnGrabbedPlayersChanged()
		{
			UpdateFreeContainerItems();
		}

		private void UpdateFreeContainerItems()
		{
			if (_quotaContainerModel.AllContainerItems == null)
			{
				_quotaContainerModel.UpdateFreeContainerItems(new List<QuotaContainerItemData>());
				return;
			}
			List<QuotaContainerItemData> containerItems = _quotaContainerModel.AllContainerItems.Where(IsItemFree).ToList();
			_quotaContainerModel.UpdateFreeContainerItems(containerItems);
		}

		private bool IsItemFree(QuotaContainerItemData itemData)
		{
			if (itemData.PointGrabable == null)
			{
				return true;
			}
			if (!itemData.PointGrabable.Initialized)
			{
				return false;
			}
			if (itemData.PointGrabable.GrabbedByPlayers.Count == 0)
			{
				return itemData.PointGrabable.IsReadyToQuota;
			}
			return false;
		}
	}
}
