using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings
{
	[CreateAssetMenu(fileName = "CoinRobLocomotionSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/CoinRobLocomotionSettings")]
	public class CoinRobLocomotionSettings : ScriptableObject
	{
		[field: SerializeField]
		public float MinSpeed { get; private set; } = 2f;

		[field: SerializeField]
		public float MaxSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public float MinAngularSpeed { get; private set; } = 180f;

		[field: SerializeField]
		public float MaxAngularSpeed { get; private set; } = 720f;

		[field: SerializeField]
		public float FullSpeedPathDistance { get; private set; } = 5f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float TurnSlowdownStrength { get; private set; } = 0.45f;

		[field: SerializeField]
		public float SpeedAnimationLerpSpeed { get; private set; } = 6f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float SpeedAnimationDeadzone { get; private set; } = 0.12f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float RunBlendFootstepThreshold { get; private set; } = 0.35f;

		[field: SerializeField]
		public float LocomotionSpeedDeadzone { get; private set; } = 0.15f;
	}
}
