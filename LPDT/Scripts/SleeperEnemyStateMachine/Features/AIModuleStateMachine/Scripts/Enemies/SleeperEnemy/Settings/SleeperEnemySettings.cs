using Features.RagdollModule.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings
{
	[CreateAssetMenu(fileName = "SleeperEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/Sleeper/SleeperEnemySettings")]
	public class SleeperEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		public float AggroRadius { get; private set; } = 5f;

		[field: SerializeField]
		public float AggroDuration { get; private set; } = 1.5f;

		[field: FormerlySerializedAs("<MinAggroDuration>k__BackingField")]
		[field: SerializeField]
		public float WakeUpDuration { get; private set; } = 1.5f;

		[field: SerializeField]
		public float ChaseRadius { get; private set; } = 10f;

		[field: SerializeField]
		public float ChaseGiveUpTime { get; private set; } = 5f;

		[field: SerializeField]
		public float DamageAggroDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float DistanceToAttack { get; private set; } = 2f;

		[field: SerializeField]
		public float AttackRadius { get; private set; } = 2.5f;

		[field: SerializeField]
		public float ItemsDamage { get; private set; } = 10f;

		[field: SerializeField]
		public float AttackForce { get; private set; } = 8f;

		[field: SerializeField]
		public float AttackKnockbackUpBias { get; private set; } = 0.35f;

		[field: SerializeField]
		public float AttackStopDistance { get; private set; } = 1.8f;

		[field: SerializeField]
		public float AttackDuration { get; private set; } = 1.5f;

		[field: SerializeField]
		public StunDurationPreset AttackStunDurationPreset { get; private set; } = StunDurationPreset.Medium;

		[field: SerializeField]
		public float HomeAttackRadius { get; private set; } = 1f;

		[field: SerializeField]
		public float InvestigateDamagableRadius { get; private set; } = 4f;

		[field: SerializeField]
		public LayerMask InvestigateOverlapMask { get; private set; } = -1;

		[field: SerializeField]
		public float InvestigateTimeout { get; private set; } = 8f;
	}
}
