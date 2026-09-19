using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanChasingSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanChasingSettings")]
	public class HeadmanChasingSettings : ScriptableObject
	{
		[field: SerializeField]
		public float DistanceToAttack { get; private set; } = 3f;

		[field: SerializeField]
		public float AttackTime { get; private set; } = 5f;

		[field: SerializeField]
		public float SprintSpeed { get; private set; } = 7f;

		[field: SerializeField]
		public float ChasingSpeedCoefficient { get; private set; } = 1f;

		[field: SerializeField]
		public float StoppingDistance { get; private set; } = 3f;

		[field: SerializeField]
		public float Damage { get; private set; } = 10f;

		[field: SerializeField]
		public float DamageImpulseStrength { get; private set; } = 2f;

		[field: SerializeField]
		public float BaseAttackRangeCoefficient { get; private set; } = 1.2f;

		[field: SerializeField]
		public string DefaultAreaName { get; private set; } = "Walkable";

		[field: SerializeField]
		public float BaseAttackWindupDuration { get; private set; } = 0.266f;

		[field: SerializeField]
		public float BaseAttackActiveDuration { get; private set; } = 0.366f;

		[field: SerializeField]
		public float LowAttackWindupDuration { get; private set; } = 1.133f;

		[field: SerializeField]
		public float LowAttackActiveDuration { get; private set; } = 1f;

		[Header("Non-player targets")]
		[Tooltip("Allows the headman to hunt registered non-player targets (the monkey porter) when no player is available.")]
		[field: SerializeField]
		public bool IsAuxTargetingEnabled { get; private set; } = true;

		[Tooltip("Radius in which a non-player target is picked up.")]
		[field: SerializeField]
		public float AuxTargetDetectionRadius { get; private set; } = 12f;

		[Tooltip("How often the headman looks for a non-player target while wandering.")]
		[field: SerializeField]
		public float AuxTargetSearchInterval { get; private set; } = 0.3f;

		[Tooltip("Height above the non-player target root used for line of sight.")]
		[field: SerializeField]
		public float AuxTargetAimHeight { get; private set; }

		[Tooltip("Distance within which a non-player target counts as seen even without line of sight.")]
		[field: SerializeField]
		public float AuxTargetHardDetectDistance { get; private set; } = 3f;
	}
}
