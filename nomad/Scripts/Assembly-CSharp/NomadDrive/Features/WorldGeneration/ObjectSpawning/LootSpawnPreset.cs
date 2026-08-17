using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Loot Spawn Preset", fileName = "LootSpawnPreset")]
	public class LootSpawnPreset : ScriptableObject
	{
		public string presetDescription;

		[Tooltip("Spawn point groups (e.g., Kitchen, Garage, Bedroom)")]
		public LootSpawnPointGroup[] spawnPointGroups;

		public int TotalSpawnPointCount
		{
			get
			{
				int num = 0;
				if (spawnPointGroups != null)
				{
					LootSpawnPointGroup[] array = spawnPointGroups;
					foreach (LootSpawnPointGroup lootSpawnPointGroup in array)
					{
						if (lootSpawnPointGroup?.spawnPoints != null)
						{
							num += lootSpawnPointGroup.spawnPoints.Length;
						}
					}
				}
				return num;
			}
		}

		public IEnumerable<LootSpawnPresetData> GetAllSpawnPoints()
		{
			if (spawnPointGroups == null)
			{
				yield break;
			}
			LootSpawnPointGroup[] array = spawnPointGroups;
			foreach (LootSpawnPointGroup lootSpawnPointGroup in array)
			{
				if (lootSpawnPointGroup?.spawnPoints == null)
				{
					continue;
				}
				LootSpawnPresetData[] spawnPoints = lootSpawnPointGroup.spawnPoints;
				foreach (LootSpawnPresetData lootSpawnPresetData in spawnPoints)
				{
					if (lootSpawnPresetData != null)
					{
						yield return lootSpawnPresetData;
					}
				}
			}
		}
	}
}
