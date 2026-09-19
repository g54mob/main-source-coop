using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[CreateAssetMenu(fileName = "RatsHoleEnemySpawnSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/RatsHoleEnemySpawnSettings")]
	public class RatsHoleEnemySpawnSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float SpawnChance { get; private set; } = 0.1f;

		[field: SerializeField]
		[field: Min(0f)]
		public float GuaranteedEnemySpawnWindow { get; private set; } = 30f;

		[field: SerializeField]
		public MonoItem EmptyFlowItemPrefab { get; private set; }

		[field: SerializeField]
		[field: Min(1f)]
		public int EmptyFlowItemMinCount { get; private set; } = 1;

		[field: SerializeField]
		[field: Min(1f)]
		public int EmptyFlowItemMaxCount { get; private set; } = 4;

		[field: SerializeField]
		[field: Min(1f)]
		public int MaxAliveEnemiesPerHole { get; private set; } = 2;

		[field: SerializeField]
		public NetworkPrefabRef EnemyPrefab { get; private set; }
	}
}
