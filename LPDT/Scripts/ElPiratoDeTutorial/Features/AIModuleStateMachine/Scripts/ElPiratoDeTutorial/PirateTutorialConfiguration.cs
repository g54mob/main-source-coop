using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	[CreateAssetMenu(fileName = "PirateTutorialConfiguration_Default", menuName = "Configurations/Enemy/PiratePirateTutorialConfiguration")]
	public class PirateTutorialConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float WalkingSpeed { get; private set; } = 2.7f;

		[field: SerializeField]
		public float ChaseSpeed { get; private set; } = 3.7f;

		[field: SerializeField]
		public float MeleeAttackRange { get; private set; }

		[field: SerializeField]
		public float AttackRange { get; private set; }

		[field: SerializeField]
		public float AttackChance { get; private set; }

		[field: SerializeField]
		public float AttackWaitCooldown { get; private set; }

		[field: SerializeField]
		public float LostTargetTime { get; private set; } = 5f;

		[field: SerializeField]
		public float SafeZoneObservedEntryGraceTime { get; private set; } = 1.5f;

		[field: SerializeField]
		public float InvestigationLookAroundTime { get; private set; } = 4f;

		[field: SerializeField]
		public float InvestigationSearchRadius { get; private set; } = 5f;

		[field: SerializeField]
		public float InvestigationSearchMoveInterval { get; private set; } = 1.5f;

		[field: SerializeField]
		public int InvestigationSearchAttempts { get; private set; } = 8;

		[field: SerializeField]
		public float InvestigationDirectionSearchDistance { get; private set; } = 4f;

		[field: SerializeField]
		public float InvestigationDirectionSearchRadius { get; private set; } = 2f;

		[field: SerializeField]
		public float InvestigationDirectionMinSpeed { get; private set; } = 0.1f;

		[field: SerializeField]
		public float MoveToPointUpdateDistanceThreshold { get; private set; } = 0.3f;

		[field: SerializeField]
		public float MeleeAttackDamage { get; private set; } = 3f;

		[field: SerializeField]
		public float MeleeAttackImpulseStrength { get; private set; } = 1f;

		[field: SerializeField]
		public bool IsSafeZoneAttackEnabled { get; private set; } = true;

		[field: SerializeField]
		public float FleeSearchRadius { get; private set; } = 25f;

		[field: SerializeField]
		public int FleeSearchAttempts { get; private set; } = 30;

		[field: SerializeField]
		public float FleeMinDistanceFromCurrentPosition { get; private set; } = 5f;

		[field: SerializeField]
		public float FleeDelayAfterReachingPlayers { get; private set; } = 5f;

		[field: SerializeField]
		public float IdleWanderRadius { get; private set; } = 10f;

		[field: SerializeField]
		public float IdleMinWanderPositionUpdateTime { get; private set; } = 5f;

		[field: SerializeField]
		public float IdleMaxWanderPositionUpdateTime { get; private set; } = 10f;

		[field: SerializeField]
		public int IdleWanderSearchAttempts { get; private set; } = 15;

		[field: SerializeField]
		public float IdleChangeAreaUpdateFrequency { get; private set; } = 45f;

		[field: SerializeField]
		public float IdleSafeAreaRange { get; private set; } = 60f;

		[field: SerializeField]
		public int IdleAreaSearchAttempts { get; private set; } = 15;
	}
}
