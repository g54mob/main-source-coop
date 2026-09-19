using System;
using System.Collections.Generic;
using Features.QuotaModule.Scripts;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class PlayersContributedQuotaStatisticsSystem : IInitializable, IDisposable
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly QuotaContainerModel _quotaContainerModel;

		public PlayersContributedQuotaStatisticsSystem(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, QuotaContainerModel quotaContainerModel)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_quotaContainerModel = quotaContainerModel;
		}

		public void Initialize()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated += UpdateItemsStatistics;
		}

		public void Dispose()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated -= UpdateItemsStatistics;
		}

		private void UpdateItemsStatistics()
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (QuotaContainerItemData freeContainerItem in _quotaContainerModel.FreeContainerItems)
			{
				int playerId = freeContainerItem.PointGrabable.NetworkObject.StateAuthority.PlayerId;
				if (dictionary.ContainsKey(playerId))
				{
					dictionary[playerId] += freeContainerItem.Item.CurrencyValue;
				}
				else
				{
					dictionary[playerId] = freeContainerItem.Item.CurrencyValue;
				}
			}
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				_levelPlayersGameStatisticsModel.SetCents(item.Key, item.Value);
			}
		}
	}
}
