using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using Mirror;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootSpawningService
	{
		private readonly LootPlacementSolver _placementSolver = new LootPlacementSolver();

		private const bool EnableCollisionCorrection = false;

		private const float LootIntegrationTimeMs = 1f;

		public async UniTask<GameObject> SpawnLootDeterministically(Vector3 position, int seed, LootSpawnData[] lootSpawnDatas, Quaternion parentRotation, LootPlacementContext context = null)
		{
			try
			{
				await Awaitable.BackgroundThreadAsync();
				System.Random random = new System.Random(seed);
				LootSpawnData selectedLootData = SelectLootDataByChances(random, lootSpawnDatas);
				if (selectedLootData == null || !selectedLootData.HasValidReference)
				{
					await Awaitable.MainThreadAsync();
					return null;
				}
				Quaternion quaternion = CalculateDeterministicRotation(random, selectedLootData);
				Quaternion quaternion2 = Quaternion.Euler(selectedLootData.initialRotationEuler);
				Quaternion finalRotation = parentRotation * quaternion2 * quaternion;
				await Awaitable.MainThreadAsync();
				GameObject gameObject = await LoadPrefabFromAssetReference(selectedLootData.lootPrefab);
				if (gameObject == null)
				{
					return null;
				}
				Vector3 trueSpawnPos = FloatingOriginManager.ToTrueWorld(position);
				AsyncInstantiateOperation.SetIntegrationTimeMS(1f);
				AsyncInstantiateOperation<GameObject> asyncOp = UnityEngine.Object.InstantiateAsync(gameObject, position, finalRotation);
				await asyncOp;
				GameObject loot = ((asyncOp.Result != null && asyncOp.Result.Length != 0) ? asyncOp.Result[0] : null);
				if (loot == null)
				{
					return null;
				}
				GameObject instantiatedLoot = null;
				await MainThreadWorkBudget.Run(delegate
				{
					if (!(loot == null))
					{
						loot.transform.position = FloatingOriginManager.ToRenderWorld(trueSpawnPos);
						_ = NetworkServer.active;
						if (!NetworkServer.active)
						{
							UnityEngine.Object.Destroy(loot);
						}
						else
						{
							NetworkServer.Spawn(loot, NetworkServer.localConnection);
							StampPersistentObject(loot, (selectedLootData.lootPrefab != null) ? selectedLootData.lootPrefab.AssetGUID : null);
							ParentSpawnedLootForFloatingOrigin(loot);
							instantiatedLoot = loot;
						}
					}
				}, WorkPriority.High);
				return instantiatedLoot;
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				await Awaitable.MainThreadAsync();
				EvilLogger.LogError("[LootSpawningService] Spawn failed: " + ex2.Message + "\n" + ex2.StackTrace, "SpawnLootDeterministically", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawningService.cs", 146);
				return null;
			}
		}

		public async UniTask<GameObject> SpawnLootDeterministicallyFromNetwork(Vector3 position, int seed, LootSpawnDataNetwork[] lootSpawnDatas, Quaternion parentRotation, LootPlacementContext context = null)
		{
			try
			{
				await Awaitable.BackgroundThreadAsync();
				System.Random random = new System.Random(seed);
				LootSpawnDataNetwork? lootSpawnDataNetwork = SelectLootDataByChancesNetwork(random, lootSpawnDatas);
				if (!lootSpawnDataNetwork.HasValue || !lootSpawnDataNetwork.Value.HasValidReference)
				{
					await Awaitable.MainThreadAsync();
					return null;
				}
				LootSpawnDataNetwork selected = lootSpawnDataNetwork.Value;
				Quaternion quaternion = CalculateDeterministicRotationFromNetwork(random, selected);
				Quaternion quaternion2 = Quaternion.Euler(selected.initialRotationEuler);
				Quaternion finalRotation = parentRotation * quaternion2 * quaternion;
				string prefabGuid = selected.lootPrefabGuid;
				await Awaitable.MainThreadAsync();
				GameObject gameObject = await LoadPrefabFromGuid(prefabGuid);
				if (gameObject == null)
				{
					return null;
				}
				Vector3 trueSpawnPos = FloatingOriginManager.ToTrueWorld(position);
				AsyncInstantiateOperation.SetIntegrationTimeMS(1f);
				AsyncInstantiateOperation<GameObject> asyncOp = UnityEngine.Object.InstantiateAsync(gameObject, position, finalRotation);
				await asyncOp;
				GameObject loot = ((asyncOp.Result != null && asyncOp.Result.Length != 0) ? asyncOp.Result[0] : null);
				if (loot == null)
				{
					return null;
				}
				GameObject instantiatedLoot = null;
				await MainThreadWorkBudget.Run(delegate
				{
					if (!(loot == null))
					{
						loot.transform.position = FloatingOriginManager.ToRenderWorld(trueSpawnPos);
						_ = NetworkServer.active;
						if (!NetworkServer.active)
						{
							UnityEngine.Object.Destroy(loot);
						}
						else
						{
							NetworkServer.Spawn(loot, NetworkServer.localConnection);
							StampPersistentObject(loot, prefabGuid);
							ParentSpawnedLootForFloatingOrigin(loot);
							instantiatedLoot = loot;
						}
					}
				}, WorkPriority.High);
				return instantiatedLoot;
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				await Awaitable.MainThreadAsync();
				EvilLogger.LogError("[LootSpawningService] SpawnFromNetwork failed: " + ex2.Message + "\n" + ex2.StackTrace, "SpawnLootDeterministicallyFromNetwork", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawningService.cs", 260);
				return null;
			}
		}

		private static void ParentSpawnedLootForFloatingOrigin(GameObject loot)
		{
			if (!(loot == null))
			{
				FloatingOriginManager instance = FloatingOriginManager.Instance;
				if (!(instance == null) && !(instance.WorldContentRoot == null) && loot.GetComponentInChildren<IFloatingOriginShiftable>(includeInactive: true) == null)
				{
					loot.transform.SetParent(instance.WorldContentRoot, worldPositionStays: true);
				}
			}
		}

		private static int SelectWeightedIndex(System.Random random, float[] chances)
		{
			float num = 0f;
			for (int i = 0; i < chances.Length; i++)
			{
				num += Mathf.Max(0f, chances[i]);
			}
			if (num <= 0f)
			{
				return 0;
			}
			float num2 = (float)(random.NextDouble() * (double)num);
			float num3 = 0f;
			for (int j = 0; j < chances.Length; j++)
			{
				num3 += Mathf.Max(0f, chances[j]);
				if (num2 < num3)
				{
					return j;
				}
			}
			return chances.Length - 1;
		}

		private LootSpawnData SelectLootDataByChances(System.Random random, LootSpawnData[] lootSpawnDatas)
		{
			if (lootSpawnDatas == null || lootSpawnDatas.Length == 0)
			{
				return null;
			}
			List<LootSpawnData> list = new List<LootSpawnData>();
			foreach (LootSpawnData lootSpawnData in lootSpawnDatas)
			{
				if (lootSpawnData != null && lootSpawnData.HasValidReference)
				{
					list.Add(lootSpawnData);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			float[] array = new float[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array[j] = list[j].chance;
			}
			return list[SelectWeightedIndex(random, array)];
		}

		private LootSpawnDataNetwork? SelectLootDataByChancesNetwork(System.Random random, LootSpawnDataNetwork[] lootSpawnDatas)
		{
			if (lootSpawnDatas == null || lootSpawnDatas.Length == 0)
			{
				return null;
			}
			List<LootSpawnDataNetwork> list = new List<LootSpawnDataNetwork>();
			for (int i = 0; i < lootSpawnDatas.Length; i++)
			{
				LootSpawnDataNetwork item = lootSpawnDatas[i];
				if (item.HasValidReference)
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			float[] array = new float[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array[j] = list[j].chance;
			}
			return list[SelectWeightedIndex(random, array)];
		}

		private UniTask<GameObject> LoadPrefabFromAssetReference(AssetReferenceGameObject assetRef)
		{
			return WorldPrefabCache.GetOrLoadAsync(assetRef?.AssetGUID);
		}

		private UniTask<GameObject> LoadPrefabFromGuid(string guid)
		{
			return WorldPrefabCache.GetOrLoadAsync(guid);
		}

		private static void StampPersistentObject(GameObject instance, string addressableGuid)
		{
			if (instance != null && !string.IsNullOrEmpty(addressableGuid))
			{
				PersistentObject.ServerEnsure(instance, addressableGuid);
			}
		}

		private Quaternion CalculateDeterministicRotation(System.Random random, LootSpawnData lootData)
		{
			float x = (lootData.randomRotationAxis.HasFlag(RotationAxis.X) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			float y = (lootData.randomRotationAxis.HasFlag(RotationAxis.Y) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			float z = (lootData.randomRotationAxis.HasFlag(RotationAxis.Z) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			return Quaternion.Euler(x, y, z);
		}

		private Quaternion CalculateDeterministicRotationFromNetwork(System.Random random, LootSpawnDataNetwork lootData)
		{
			float x = (lootData.randomRotationAxis.HasFlag(RotationAxis.X) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			float y = (lootData.randomRotationAxis.HasFlag(RotationAxis.Y) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			float z = (lootData.randomRotationAxis.HasFlag(RotationAxis.Z) ? ((float)(random.NextDouble() * (double)(lootData.maxRotationAngle - lootData.minRotationAngle) + (double)lootData.minRotationAngle)) : 0f);
			return Quaternion.Euler(x, y, z);
		}
	}
}
