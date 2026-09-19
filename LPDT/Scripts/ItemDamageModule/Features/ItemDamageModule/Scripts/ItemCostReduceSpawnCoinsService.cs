using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts
{
	public class ItemCostReduceSpawnCoinsService : IItemCostReduceSpawnCoinsService
	{
		private readonly ItemCostReduceConfig _config;

		private readonly IItemSpawnService _itemSpawnService;

		public ItemCostReduceSpawnCoinsService(ItemCostReduceConfig config, IItemSpawnService itemSpawnService)
		{
			_config = config;
			_itemSpawnService = itemSpawnService;
		}

		public async UniTaskVoid SpawnCoins(Vector3 position, int coinCount)
		{
			Dictionary<NetworkBehaviour, MultipleSpawnData> dictionary = new Dictionary<NetworkBehaviour, MultipleSpawnData>();
			int num = coinCount;
			while (num > 0)
			{
				foreach (KeyValuePair<int, MonoItem> item in _config.CoinPrefabs.OrderByDescending((KeyValuePair<int, MonoItem> c) => c.Key).ToDictionary((KeyValuePair<int, MonoItem> d) => d.Key, (KeyValuePair<int, MonoItem> d) => d.Value))
				{
					if (num >= item.Key)
					{
						num -= item.Key;
						ItemData itemData = new ItemData(item.Value.DefaultConfig)
						{
							CurrencyValue = item.Key
						};
						if (dictionary.TryGetValue(item.Value, out var value))
						{
							value.Count++;
							break;
						}
						MultipleSpawnData value2 = new MultipleSpawnData(1, itemData);
						dictionary.Add(item.Value, value2);
						break;
					}
				}
			}
			await _itemSpawnService.SpawnItemsMultiple(dictionary, position, _config.CoinSpawnRandomRadius, useSpread: true, addRandomForce: true, 50f);
		}
	}
}
