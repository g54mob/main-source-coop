using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[CreateAssetMenu(fileName = "EnemySpawnConfiguration_Default", menuName = "Configurations/AIModule/EnemySpawnConfiguration")]
	public class EnemySpawnConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public EnemiesExactSpawnData DefaultEnemiesSpawnConfigurations { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, EnemiesExactSpawnData> EnemiesSpawnConfigurationsByGamePhase { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, SideBossSpawnData> SideBossesSpawnConfigurationsByLevel { get; private set; }

		[field: SerializeField]
		public int RadiusFromPlayerToNotSpawn { get; private set; }

		[field: SerializeField]
		public List<EnemyType> CloseSpawnEnemyTypes { get; private set; }
	}
}
