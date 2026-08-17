using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.Plates;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootSpawnPoint : MonoBehaviour
	{
		[SerializeField]
		private LootSpawnData[] lootSpawnDatas;

		private readonly LootSpawningService _lootSpawningService = new LootSpawningService();

		private int _thisSeed;

		private void SetLootSpawnDatas(LootSpawnData[] datas)
		{
			lootSpawnDatas = datas;
		}

		public void Setup(LootSpawnData[] datas, int seed, LootPlacementContext context = null)
		{
			SetupAsync(datas, seed, context, base.destroyCancellationToken).Forget();
		}

		public async UniTask SetupAsync(LootSpawnData[] datas, int seed, LootPlacementContext context = null, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				SetLootSpawnDatas(datas);
				_thisSeed = seed;
				if (!NetworkServer.active)
				{
					RequestServerToSpawnLoots();
				}
				else
				{
					if (NetworkSingleton<WorldGenerator>.Instance == null || !NetworkSingleton<WorldGenerator>.Instance.TryReserveLootSpawn(_thisSeed))
					{
						return;
					}
					GameObject spawnedLoot = null;
					try
					{
						ct.ThrowIfCancellationRequested();
						Quaternion parentRotation = ((base.transform.parent != null) ? base.transform.parent.rotation : Quaternion.identity);
						spawnedLoot = await _lootSpawningService.SpawnLootDeterministically(base.transform.position, _thisSeed, lootSpawnDatas, parentRotation, context);
					}
					finally
					{
						if (spawnedLoot == null)
						{
							NetworkSingleton<WorldGenerator>.Instance?.UnregisterLootSpawn(_thisSeed);
						}
					}
					if (!(spawnedLoot == null))
					{
						LootLifecycleManager.Instance?.ServerTrackWildLoot(spawnedLoot, _thisSeed);
						if (spawnedLoot.TryGetComponent<ConditionComponent>(out var component))
						{
							component.ServerInitializeCondition(_thisSeed);
						}
						if (spawnedLoot.TryGetComponent<Plate>(out var component2))
						{
							component2.ServerInitializeFromSeed(_thisSeed);
						}
						if (spawnedLoot.TryGetComponent<Food>(out var component3))
						{
							component3.ServerInitializeDurability(_thisSeed);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception arg)
			{
				EvilLogger.LogError($"<color=red>[LootSpawnPoint]</color> Setup failed: {arg}", "SetupAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawnPoint.cs", 255);
			}
		}

		private void RequestServerToSpawnLoots()
		{
			if (NetworkSingleton<WorldGenerator>.Instance == null)
			{
				EvilLogger.LogError("<color=red>[LootSpawnPoint]</color> WorldGenerator.Instance is null! Cannot request loot spawn.", "RequestServerToSpawnLoots", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawnPoint.cs", 267);
				return;
			}
			LootSpawnDataNetwork[] array = LootSpawnData.ToNetworkArray(lootSpawnDatas);
			NetworkSingleton<WorldGenerator>.Instance.CmdRequestLootSpawn(base.transform.position, _thisSeed, array);
		}
	}
}
