using System;
using System.Collections.Generic;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.QuotaModule.Scripts.Systems
{
	public class QuotaAnalyticsSystem : IInitializable, IDisposable
	{
		private const bool SEND_ONLY_FIST_TIME = true;

		private readonly GameAnalyticsEventSendService _gameAnalytics;

		private readonly QuotaContainerModel _quotaContainerModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly List<MonoItem> _collectedItems = new List<MonoItem>();

		private int _eventCount;

		public QuotaAnalyticsSystem(GameAnalyticsEventSendService gameAnalytics, QuotaContainerModel quotaContainerModel, MultiplayerModel multiplayerModel)
		{
			_gameAnalytics = gameAnalytics;
			_quotaContainerModel = quotaContainerModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated += OnFreeContainerItemsUpdated;
		}

		public void Dispose()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated -= OnFreeContainerItemsUpdated;
			_collectedItems.Clear();
		}

		private void OnFreeContainerItemsUpdated()
		{
			foreach (QuotaContainerItemData freeContainerItem in _quotaContainerModel.FreeContainerItems)
			{
				MonoItem monoItem = freeContainerItem?.Item;
				if (!(monoItem == null) && !_collectedItems.Contains(monoItem))
				{
					_collectedItems.Add(monoItem);
					if (IsLocalStateAuthority(monoItem))
					{
						Send();
					}
				}
			}
		}

		private bool IsLocalStateAuthority(MonoItem item)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			if (item.CurrencyValue <= 1)
			{
				return false;
			}
			if (item.NetworkObject == null || !item.NetworkObject.IsValid)
			{
				return false;
			}
			return item.NetworkObject.StateAuthority == _multiplayerModel.NetworkRunner.LocalPlayer;
		}

		private void Send()
		{
			if (_eventCount <= 0)
			{
				_gameAnalytics.TrackItemLoaded();
				_eventCount++;
			}
		}
	}
}
