using System.Collections.Generic;

namespace Features.AIModule.Scripts
{
	public class EnemiesPoolModel
	{
		public Dictionary<ExactEnemySpawnConfiguration, float> EnemiesSpawnChances { get; set; } = new Dictionary<ExactEnemySpawnConfiguration, float>();

		public List<EnemyType> ChosenEnemyTypes { get; set; } = new List<EnemyType>();

		public void CleanupEnemiesPool()
		{
			EnemiesSpawnChances.Clear();
		}
	}
}
