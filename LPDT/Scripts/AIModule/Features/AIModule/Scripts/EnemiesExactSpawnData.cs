using System;
using System.Collections.Generic;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class EnemiesExactSpawnData
	{
		public List<ExactEnemySpawnConfiguration> ExactSpawnConfigurations = new List<ExactEnemySpawnConfiguration>();

		public float MaxSize;

		public float ChanceMultiplyIfNotSpawned;

		public float ChanceDivideIfSpawned;

		public float ChanceMultiplyIfNotSpawnedIncreasedByPhaseCount;

		public float ChanceDivideIfSpawnedIncreasedByPhaseCount;
	}
}
