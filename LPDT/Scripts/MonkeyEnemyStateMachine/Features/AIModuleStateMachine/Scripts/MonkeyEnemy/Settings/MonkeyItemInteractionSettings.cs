using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyItemInteractionSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyItemInteractionSettings")]
	public class MonkeyItemInteractionSettings : ScriptableObject
	{
		[field: SerializeField]
		public float ItemDetectionRadius { get; private set; } = 10f;

		[field: SerializeField]
		public float HeldCoinDetectionRadius { get; private set; } = 1.5f;

		[field: SerializeField]
		public int HeldCoinOverlapBufferSize { get; private set; } = 16;

		[field: SerializeField]
		public int MaxItemGrabTime { get; private set; } = 5;

		[field: SerializeField]
		public float MoveToItemSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public float MoveToItemTimeoutDuration { get; private set; } = 5f;

		[field: SerializeField]
		public float ConsumeDistanceStoppingDistanceMultiplier { get; private set; } = 1.5f;

		[field: SerializeField]
		public float EatingDuration { get; private set; } = 1.1f;

		[field: SerializeField]
		public float CoinPocketAnimationDuration { get; private set; } = 1f;

		[field: SerializeField]
		public float HideoutWanderDuration { get; private set; } = 8f;

		[field: SerializeField]
		public float HideoutMinDistanceFromCoinSourcePlayer { get; private set; } = 15f;

		[field: SerializeField]
		public float HideoutFleeSearchRadius { get; private set; } = 40f;

		[field: SerializeField]
		public float HideoutFleeSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public float HideoutFleeTimeoutDuration { get; private set; } = 8f;

		[field: SerializeField]
		public float HideoutFleeDestinationReachedDistance { get; private set; } = 3f;

		[field: SerializeField]
		public int HideoutFleePositionAttempts { get; private set; } = 20;

		[field: SerializeField]
		public float HideoutWanderRadius { get; private set; } = 8f;

		[field: SerializeField]
		public float HideoutFleeAverageAvoidDistanceWeight { get; private set; } = 5f;

		[field: SerializeField]
		public float HideoutFleeTooClosePenaltyWeight { get; private set; } = 10f;
	}
}
