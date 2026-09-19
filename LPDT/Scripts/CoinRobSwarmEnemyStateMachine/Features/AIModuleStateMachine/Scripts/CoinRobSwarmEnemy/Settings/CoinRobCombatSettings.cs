using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobCombatSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobCombatSettings")]
	public class CoinRobCombatSettings : ScriptableObject
	{
		[field: SerializeField]
		public float PlayerAttackRange { get; private set; } = 3f;

		[field: SerializeField]
		public float MaxAttackVerticalDelta { get; private set; } = 2.5f;

		[field: SerializeField]
		public float KingElevationSeparation { get; private set; } = 1.25f;

		[field: SerializeField]
		public float MinKingAbovePlayerForBlockedAttack { get; private set; } = 0.35f;

		[field: SerializeField]
		public float TargetSearchRadius { get; private set; } = 6.5f;

		[field: SerializeField]
		public float DiffuseRadius { get; private set; } = 10f;

		[field: SerializeField]
		public int UnitsInSwarmToAttackPlayer { get; private set; } = 3;

		[field: SerializeField]
		public float PlayerChaseAggroRadius { get; private set; } = 5f;

		[field: SerializeField]
		public float PlayerChaseAggroDuration { get; private set; } = 3f;

		[field: SerializeField]
		public LayerMask PlayerLayerMask { get; private set; }

		[field: SerializeField]
		public float AttackFrequency { get; private set; } = 1f;

		[field: SerializeField]
		public float KingAttackAnimationDuration { get; private set; } = 1.5f;

		[field: SerializeField]
		public float Damage { get; private set; } = 10f;

		[field: SerializeField]
		public float ForceStrength { get; private set; } = 2f;
	}
}
