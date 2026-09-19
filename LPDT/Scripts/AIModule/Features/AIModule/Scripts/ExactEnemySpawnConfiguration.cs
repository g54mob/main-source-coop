using System;
using Fusion;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class ExactEnemySpawnConfiguration
	{
		public EnemyType EnemyType;

		public NetworkPrefabRef EnemyPrefab;

		public int FirstSpawnTime;

		public int SpawnInterval;

		public int RespawnInterval;

		public float Size;

		public float DefaultChance;

		public float ChanceMultiplyByPhaseCount;
	}
}
