using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.CompositeItemModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class LevelObjectsSpawnService : ILevelObjectsSpawnService
	{
		private readonly LevelObjectConfiguration _levelObjectConfiguration;

		private readonly LevelSpawnPointsModel _levelSpawnPointsModel;

		private readonly IItemSpawnService _itemSpawnService;

		private readonly LevelsObjectCostModel _levelsObjectCostModel;

		private readonly LevelObjectByLevelTypeConfiguration _levelObjectByLevelTypeConfiguration;

		private int _spawnGeneration;

		public LevelObjectsSpawnService(LevelObjectConfiguration levelObjectConfiguration, LevelSpawnPointsModel levelSpawnPointsModel, IItemSpawnService itemSpawnService, LevelsObjectCostModel levelsObjectCostModel, LevelObjectByLevelTypeConfiguration levelObjectByLevelTypeConfiguration)
		{
			_levelObjectConfiguration = levelObjectConfiguration;
			_levelSpawnPointsModel = levelSpawnPointsModel;
			_itemSpawnService = itemSpawnService;
			_levelsObjectCostModel = levelsObjectCostModel;
			_levelObjectByLevelTypeConfiguration = levelObjectByLevelTypeConfiguration;
		}

		public void SpawnItemsByLevel(LevelType targetLevel)
		{
			if (_levelObjectByLevelTypeConfiguration.LevelTypeMapper.TryGetValue(targetLevel, out var value) && _levelObjectConfiguration.LevelObjects.TryGetValue(value, out var value2))
			{
				List<PreparedSpawnItemData> preparedItems = PrepareSpawnItems(value2);
				SpawnItems(preparedItems, _spawnGeneration);
			}
		}

		public void SpawnItemAtPoint(LevelObjectSpawnPointData spawnPoint, LevelObjectType objectType, LevelType targetLevel)
		{
			if (!_levelObjectByLevelTypeConfiguration.LevelTypeMapper.TryGetValue(targetLevel, out var value))
			{
				return;
			}
			if (!_levelObjectConfiguration.LevelObjects.TryGetValue(value, out var value2))
			{
				Debug.LogWarning($"Level {value} not found in LevelObjectConfiguration");
				return;
			}
			List<LevelObjectData> list = value2.LevelObjects.LevelObjectData.Where((LevelObjectData x) => x.Type == objectType).ToList();
			if (list.Count == 0)
			{
				Debug.LogWarning($"No objects of type {objectType} found in LevelObjectData for level {value}");
				return;
			}
			LevelObjectData levelObjectData = list[Random.Range(0, list.Count)];
			List<ItemData> itemDataList = PrepareItemDataList(levelObjectData);
			PreparedSpawnItemData preparedItem = new PreparedSpawnItemData(spawnPoint, levelObjectData, itemDataList);
			SpawnItem(preparedItem, _spawnGeneration).Forget();
		}

		public void PrepareSpawnItems(LevelType targetLevel)
		{
			if (_levelObjectByLevelTypeConfiguration.LevelTypeMapper.TryGetValue(targetLevel, out var value) && _levelObjectConfiguration.LevelObjects.TryGetValue(value, out var value2))
			{
				_levelsObjectCostModel.PreparedSpawnItems = PrepareSpawnItems(value2);
			}
		}

		public void SpawnPreparedItems()
		{
			int spawnGeneration = _spawnGeneration;
			SpawnItems(_levelsObjectCostModel.PreparedSpawnItems, spawnGeneration);
		}

		public void InvalidatePreparedSpawns()
		{
			_spawnGeneration++;
			_levelSpawnPointsModel.Clear();
		}

		private List<PreparedSpawnItemData> PrepareSpawnItems(LevelData levelData)
		{
			_levelsObjectCostModel.Clear();
			foreach (LevelObjectSpawnPointData item in _levelSpawnPointsModel.LevelSpawnPointsPool)
			{
				item.IsOccupied = false;
			}
			List<PreparedSpawnItemData> list = new List<PreparedSpawnItemData>();
			Dictionary<LevelObjectType, int> dictionary = new Dictionary<LevelObjectType, int>();
			int num = 0;
			foreach (LevelObjectsSpawnData levelObjectsSpawnData in levelData.LevelObjectsSpawnPoints)
			{
				int num2 = Random.Range(levelObjectsSpawnData.MinObjectCount, levelObjectsSpawnData.MaxObjectCount + 1);
				List<LevelObjectSpawnPointData> list2 = _levelSpawnPointsModel.LevelSpawnPointsPool.Where((LevelObjectSpawnPointData x) => x.TargetObjects.Contains(levelObjectsSpawnData.ObjectType) && !x.IsOccupied).ToList();
				List<LevelObjectData> list3 = levelData.LevelObjects.LevelObjectData.Where((LevelObjectData x) => x.Type == levelObjectsSpawnData.ObjectType).ToList();
				if (list3.Count == 0)
				{
					Debug.LogWarning($"No objects of type {levelObjectsSpawnData.ObjectType} found in LevelObjectData");
					continue;
				}
				ShuffleList(list3);
				int num3 = 0;
				for (int num4 = 0; num4 < num2; num4++)
				{
					if (list2.Count <= 0)
					{
						break;
					}
					int index = Random.Range(0, list2.Count);
					LevelObjectSpawnPointData levelObjectSpawnPointData = list2[index];
					LevelObjectData levelObjectData = list3[num3];
					List<ItemData> list4 = PrepareItemDataList(levelObjectData);
					int num5 = list4.Sum((ItemData item) => item.CurrencyValue);
					num += num5;
					dictionary.TryAdd(levelObjectsSpawnData.ObjectType, 0);
					dictionary[levelObjectsSpawnData.ObjectType] += num5;
					list.Add(new PreparedSpawnItemData(levelObjectSpawnPointData, levelObjectData, list4));
					levelObjectSpawnPointData.IsOccupied = true;
					list2.RemoveAt(index);
					num3 = (num3 + 1) % list3.Count;
				}
			}
			_levelsObjectCostModel.SetCostData(num, dictionary);
			return list;
		}

		private List<ItemData> PrepareItemDataList(LevelObjectData levelObjectData)
		{
			List<ItemData> list = new List<ItemData>();
			if (levelObjectData.Item == null)
			{
				Debug.LogWarning("Item prefab is null, cannot prepare item data");
				return list;
			}
			List<ItemConfig> itemConfigsFromPrefab = GetItemConfigsFromPrefab(levelObjectData.Item);
			if (itemConfigsFromPrefab.Count == 0)
			{
				Debug.LogWarning("ItemConfig not found in prefab " + levelObjectData.Item.name);
				return list;
			}
			for (int i = 0; i < levelObjectData.ItemCount; i++)
			{
				for (int j = 0; j < itemConfigsFromPrefab.Count; j++)
				{
					ItemConfig itemConfig = itemConfigsFromPrefab[j];
					ItemData item = ((!levelObjectData.RandomizeCost) ? new ItemData(itemConfig.ItemType, itemConfig.CurrencyValue, itemConfig.CurrencyValue, itemConfig.IsCollectable) : new ItemData(itemConfig));
					list.Add(item);
				}
			}
			return list;
		}

		private static List<ItemConfig> GetItemConfigsFromPrefab(NetworkBehaviour prefab)
		{
			List<ItemConfig> list = new List<ItemConfig>();
			if (prefab == null)
			{
				return list;
			}
			if (prefab.TryGetComponent<CompositeItemGroup>(out var component) && component.TryGetPieceItemConfigs(list))
			{
				return list;
			}
			MonoItem component2 = prefab.GetComponent<MonoItem>();
			if (component2 != null && component2.DefaultConfig != null)
			{
				list.Add(component2.DefaultConfig);
			}
			return list;
		}

		private void SpawnItems(List<PreparedSpawnItemData> preparedItems, int generation)
		{
			foreach (PreparedSpawnItemData preparedItem in preparedItems)
			{
				SpawnItem(preparedItem, generation).Forget();
			}
		}

		private async UniTaskVoid SpawnItem(PreparedSpawnItemData preparedItem, int generation)
		{
			LevelObjectSpawnPointData spawnPoint = preparedItem.SpawnPoint;
			LevelObjectData levelObjectData = preparedItem.LevelObjectData;
			List<ItemData> itemDataList = preparedItem.ItemDataList;
			if (levelObjectData.Item == null)
			{
				Debug.LogWarning("Item prefab is null, cannot spawn");
			}
			else
			{
				if (itemDataList.Count == 0)
				{
					return;
				}
				int pieceCount = GetCompositePieceCount(levelObjectData.Item);
				bool isComposite = pieceCount > 0;
				int assemblyCount = (isComposite ? levelObjectData.ItemCount : itemDataList.Count);
				if (isComposite && itemDataList.Count != assemblyCount * pieceCount)
				{
					Debug.LogWarning("Composite spawn data mismatch for " + levelObjectData.Item.name + ": " + $"expected {assemblyCount * pieceCount} ItemData, got {itemDataList.Count}");
					return;
				}
				for (int assemblyIndex = 0; assemblyIndex < assemblyCount; assemblyIndex++)
				{
					if (generation != _spawnGeneration)
					{
						break;
					}
					Vector3 position = spawnPoint.Position;
					if (levelObjectData.UseSpread && assemblyIndex > 0)
					{
						Vector2 vector = Random.insideUnitCircle * 2f;
						position += Vector3.up * 0.5f + new Vector3(vector.x, 0f, vector.y);
					}
					Quaternion rotation = (spawnPoint.UseOriginalRotation ? spawnPoint.Rotation : Quaternion.identity);
					ItemData itemData = (isComposite ? null : itemDataList[assemblyIndex]);
					NetworkBehaviour networkBehaviour = await _itemSpawnService.SpawnItem(levelObjectData.Item, position, rotation, itemData);
					if (generation != _spawnGeneration)
					{
						if (networkBehaviour != null && networkBehaviour.Object != null && networkBehaviour.Object.IsValid)
						{
							networkBehaviour.Object.DespawnHierarchy();
						}
						break;
					}
					if (isComposite && networkBehaviour != null)
					{
						ApplyCompositePieceItemData(networkBehaviour, itemDataList, assemblyIndex * pieceCount, pieceCount);
					}
					if (networkBehaviour != null && networkBehaviour.TryGetComponent<LevelObjectMarker>(out var component))
					{
						component.SetType(levelObjectData.Type);
					}
				}
			}
		}

		private static int GetCompositePieceCount(NetworkBehaviour prefab)
		{
			if (prefab == null || !prefab.TryGetComponent<CompositeItemGroup>(out var component))
			{
				return 0;
			}
			List<ItemConfig> list = new List<ItemConfig>();
			if (!component.TryGetPieceItemConfigs(list))
			{
				return 0;
			}
			return list.Count;
		}

		private static void ApplyCompositePieceItemData(NetworkBehaviour spawnedItem, List<ItemData> itemDataList, int startIndex, int pieceCount)
		{
			if (!spawnedItem.TryGetComponent<CompositeItemGroup>(out var component))
			{
				return;
			}
			List<MonoItem> list = new List<MonoItem>();
			if (!component.TryGetPieceMonoItems(list) || list.Count != pieceCount)
			{
				Debug.LogWarning("Composite piece init mismatch on " + spawnedItem.name + ": " + $"expected {pieceCount} pieces, got {list.Count}");
				return;
			}
			for (int i = 0; i < pieceCount; i++)
			{
				list[i].ForceInitialize(itemDataList[startIndex + i]);
			}
		}

		private void ShuffleList<T>(List<T> list)
		{
			for (int num = list.Count - 1; num > 0; num--)
			{
				int num2 = Random.Range(0, num + 1);
				int index = num;
				int index2 = num2;
				T val = list[num2];
				T val2 = list[num];
				T val3 = (list[index] = val);
				val3 = (list[index2] = val2);
			}
		}
	}
}
