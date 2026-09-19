using System.Collections.Generic;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobSwarmEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobSwarmEnemySettings")]
	public class CoinRobSwarmEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		public NetworkPrefabRef CoinRobPrefab { get; private set; }

		[field: SerializeField]
		public NetworkPrefabRef CoinRobKingPrefab { get; private set; }

		[field: SerializeField]
		public NetworkPrefabRef RatsHolePrefab { get; private set; }

		[field: SerializeField]
		public int InitialUnitsCount { get; private set; }

		[field: SerializeField]
		public int MaxUnitsInSwarmCount { get; private set; } = 6;

		[field: SerializeField]
		public float SwarmRadius { get; private set; } = 3f;

		[field: SerializeField]
		public float NearPlayerCoinSearchRadius { get; private set; } = 5f;

		[field: SerializeField]
		public float WanderingDestinationUpdateTime { get; private set; } = 12f;

		[field: SerializeField]
		public float WanderingUnitStuckRepathTime { get; private set; } = 0.75f;

		[field: SerializeField]
		public float WanderingMinDistanceFromPlayer { get; private set; } = 8f;

		[field: SerializeField]
		public float WanderingMaxDistanceFromPlayer { get; private set; } = 18f;

		[field: SerializeField]
		public int WanderingPlayerPatrolAttempts { get; private set; } = 12;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float CoinFallAggressionChance { get; private set; } = 1f;

		[field: SerializeField]
		public List<LevelObjectType> ItemsOfInterest { get; private set; } = new List<LevelObjectType>();

		public bool IsItemOfInterest(LevelObjectType type)
		{
			return ItemsOfInterest.Contains(type);
		}
	}
}
