using System;
using Fusion;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class SideBossSpawnConfiguration
	{
		public EnemyType EnemyType;

		public NetworkPrefabRef BossPrefab;

		public bool IsRespawnable = true;

		public float SpawnChance = 1f;
	}
}
