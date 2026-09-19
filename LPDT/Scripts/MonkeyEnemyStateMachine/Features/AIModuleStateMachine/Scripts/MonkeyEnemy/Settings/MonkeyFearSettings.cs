using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyFearSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyFearSettings")]
	public class MonkeyFearSettings : ScriptableObject
	{
		[field: SerializeField]
		public float FearSpeed { get; private set; } = 5.5f;

		[field: SerializeField]
		public float DestinationUpdateInterval { get; private set; } = 5f;

		[field: SerializeField]
		public float RunDistance { get; private set; } = 40f;

		[field: SerializeField]
		public float DestinationReachedDistance { get; private set; } = 5f;

		[field: SerializeField]
		public float DetectionDistance { get; private set; } = 15f;

		[field: SerializeField]
		public int SafePositionAttempts { get; private set; } = 15;

		[field: SerializeField]
		public float SafePositionMinDistanceFromCenter { get; private set; } = 3f;

		[field: SerializeField]
		public float SafePositionCenterDistanceWeight { get; private set; } = 1.5f;

		[field: SerializeField]
		public float SafePositionAverageAvoidDistanceWeight { get; private set; } = 5f;

		[field: SerializeField]
		public float SafePositionTooClosePenaltyRadius { get; private set; } = 4f;
	}
}
