using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	[Serializable]
	public class LootSpawnPresetData
	{
		public LootSpawnData[] lootSpawnDatas;

		public Vector3 spawnPosition;
	}
}
