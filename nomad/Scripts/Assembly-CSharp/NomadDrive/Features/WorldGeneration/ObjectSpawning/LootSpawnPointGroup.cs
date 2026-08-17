using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Serializable]
	public class LootSpawnPointGroup
	{
		public string groupName = "New Group";

		public Color groupColor = Color.white;

		[Tooltip("Lower values spawn first. Use 0 for furniture/surfaces, 10+ for items that go on top.")]
		public int spawnOrder;

		[Tooltip("When false, this group is hidden in the Scene and its loot is skipped at spawn time.")]
		public bool isEnabled = true;

		public LootSpawnPresetData[] spawnPoints;
	}
}
