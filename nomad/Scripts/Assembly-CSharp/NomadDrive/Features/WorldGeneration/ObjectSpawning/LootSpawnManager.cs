using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootSpawnManager : MonoBehaviour
	{
		private struct SpawnEntry
		{
			public LootSpawnPresetData Data;

			public int SpawnOrder;

			public int OriginalIndex;
		}

		[SerializeField]
		private LootSpawnPreset[] lootSpawnPresets;

		[SerializeField]
		private int selectedPresetIndex;

		[Header("Preset Weights (Total = 100%)")]
		[Range(0f, 100f)]
		[SerializeField]
		private float[] presetWeights;

		private System.Random _random;

		private int _thisSeed;

		public async UniTask CreateLootsAsync(int baseSeed, LootPlacementContext context, CancellationToken ct = default(CancellationToken))
		{
			_thisSeed = baseSeed;
			_random = new System.Random(_thisSeed);
			if (context == null)
			{
				context = new LootPlacementContext();
			}
			LootSpawnPreset lootSpawnPreset = SelectLootSpawnPreset();
			if (lootSpawnPreset == null)
			{
				EvilLogger.LogError("LootSpawnManager: No valid LootSpawnPreset selected.", "CreateLootsAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawnManager.cs", 37);
			}
			else
			{
				if (lootSpawnPreset.spawnPointGroups == null)
				{
					return;
				}
				List<SpawnEntry> sortedEntries = BuildSortedSpawnEntries(lootSpawnPreset);
				for (int i = 0; i < sortedEntries.Count; i++)
				{
					SpawnEntry entry = sortedEntries[i];
					ct.ThrowIfCancellationRequested();
					Vector3 worldPos = base.transform.TransformPoint(entry.Data.spawnPosition);
					if (context.TryReservePosition(worldPos))
					{
						try
						{
							GameObject obj = new GameObject($"LootSpawnPoint_{entry.OriginalIndex}");
							obj.transform.parent = base.transform;
							obj.transform.localPosition = entry.Data.spawnPosition;
							await obj.AddComponent<LootSpawnPoint>().SetupAsync(seed: SeedManager.CombineSeed(_thisSeed, entry.OriginalIndex), datas: entry.Data.lootSpawnDatas, context: context, ct: ct);
						}
						catch (OperationCanceledException)
						{
							throw;
						}
						catch (Exception ex2)
						{
							EvilLogger.LogError($"[LootSpawnManager] Failed to spawn point {entry.OriginalIndex}: {ex2.Message}", "CreateLootsAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\LootSpawnManager.cs", 87);
						}
					}
				}
			}
		}

		private static List<SpawnEntry> BuildSortedSpawnEntries(LootSpawnPreset preset)
		{
			List<SpawnEntry> list = new List<SpawnEntry>();
			int num = 0;
			LootSpawnPointGroup[] spawnPointGroups = preset.spawnPointGroups;
			foreach (LootSpawnPointGroup lootSpawnPointGroup in spawnPointGroups)
			{
				if (lootSpawnPointGroup?.spawnPoints == null || !lootSpawnPointGroup.isEnabled)
				{
					continue;
				}
				LootSpawnPresetData[] spawnPoints = lootSpawnPointGroup.spawnPoints;
				foreach (LootSpawnPresetData lootSpawnPresetData in spawnPoints)
				{
					if (lootSpawnPresetData != null)
					{
						list.Add(new SpawnEntry
						{
							Data = lootSpawnPresetData,
							SpawnOrder = lootSpawnPointGroup.spawnOrder,
							OriginalIndex = num
						});
					}
					num++;
				}
			}
			list.Sort((SpawnEntry a, SpawnEntry b) => a.SpawnOrder.CompareTo(b.SpawnOrder));
			return list;
		}

		private LootSpawnPreset SelectLootSpawnPreset()
		{
			if (lootSpawnPresets == null || lootSpawnPresets.Length == 0 || presetWeights == null || presetWeights.Length != lootSpawnPresets.Length)
			{
				return null;
			}
			float num = (float)(_random.NextDouble() * 100.0);
			float num2 = 0f;
			for (int i = 0; i < lootSpawnPresets.Length; i++)
			{
				num2 += presetWeights[i];
				if (num <= num2)
				{
					return lootSpawnPresets[i];
				}
			}
			LootSpawnPreset[] array = lootSpawnPresets;
			return array[array.Length - 1];
		}
	}
}
