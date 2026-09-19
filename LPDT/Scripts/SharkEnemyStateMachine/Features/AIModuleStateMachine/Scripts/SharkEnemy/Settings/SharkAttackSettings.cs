using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings
{
	[CreateAssetMenu(fileName = "SharkAttackSettings_Default", menuName = "Configurations/AIModuleStateMachine/Shark/SharkAttackSettings")]
	public class SharkAttackSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AttackRange { get; private set; } = 2.5f;

		[field: SerializeField]
		public float AttackCooldown { get; private set; } = 1.5f;

		[field: SerializeField]
		[field: Tooltip("Seconds in the attack state before bite damage is applied (wind-up).")]
		public float AttackWindupDuration { get; private set; } = 0.25f;

		[field: SerializeField]
		[field: Tooltip("Seconds after wind-up before OnAttackFinished → Backoff. Increase so the SharkRoot attack animation can finish before switching visuals.")]
		public float AttackActiveDuration { get; private set; } = 0.35f;

		[field: SerializeField]
		public float SimpleDamage { get; private set; } = 50f;

		[field: SerializeField]
		public float LethalDamage { get; private set; } = 500f;

		[field: SerializeField]
		public float KnockUpForce { get; private set; } = 12f;

		[field: SerializeField]
		public float KnockForwardRatio { get; private set; } = 0.15f;

		[field: SerializeField]
		[field: Tooltip("How long the Backoff state runs before returning to Idle (not delay before entering Backoff).")]
		public float BackoffDuration { get; private set; } = 0.8f;

		[field: SerializeField]
		public float BackoffDistance { get; private set; } = 3f;

		[field: SerializeField]
		[field: Tooltip("World-space height above the damage target root for bite hit VFX.")]
		public float BiteHitVfxHeightOnPlayer { get; private set; } = 1.1f;
	}
}
