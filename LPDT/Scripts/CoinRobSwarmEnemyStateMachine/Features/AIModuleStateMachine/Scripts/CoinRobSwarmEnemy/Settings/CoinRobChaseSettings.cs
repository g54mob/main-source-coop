using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobChaseSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobChaseSettings")]
	public class CoinRobChaseSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AttackRange { get; private set; } = 1.5f;

		[field: SerializeField]
		public float TargetReachError { get; private set; } = 1f;

		[field: SerializeField]
		public float ChaseRepathInterval { get; private set; } = 5f;

		[field: SerializeField]
		public float RunAwayNavMeshSampleRadius { get; private set; } = 2f;

		[field: SerializeField]
		public float RunAwayDepositArrivalDistance { get; private set; } = 1.25f;

		[field: SerializeField]
		public float FearArrivalDistance { get; private set; }

		[field: SerializeField]
		public float KingRunAwaySpawnOrbitRadius { get; private set; } = 2f;

		[field: SerializeField]
		public float ChaseTimeout { get; private set; } = 15f;
	}
}
