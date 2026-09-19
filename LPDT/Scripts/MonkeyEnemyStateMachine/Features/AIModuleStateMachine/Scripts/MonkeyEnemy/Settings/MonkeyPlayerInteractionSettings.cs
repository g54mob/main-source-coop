using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyPlayerInteractionSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyPlayerInteractionSettings")]
	public class MonkeyPlayerInteractionSettings : ScriptableObject
	{
		[field: SerializeField]
		public float InteractionChaseSpeed { get; private set; } = 4f;

		[field: SerializeField]
		public float InteractionCatchUpDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float DistanceToContinueChasing { get; private set; } = 7f;

		[field: SerializeField]
		public float RunAroundSpeed { get; private set; } = 1.4f;

		[field: SerializeField]
		public float RunAroundSearchRadius { get; private set; } = 4f;

		[field: SerializeField]
		public float RunAroundUpdateInterval { get; private set; } = 0.5f;

		[field: SerializeField]
		public float MinStateDuration { get; private set; } = 3f;

		[field: SerializeField]
		public float MaxStateDuration { get; private set; } = 6f;

		[field: SerializeField]
		public float FakeAttackHungerThreshold { get; private set; } = 80f;

		[field: SerializeField]
		public float RunAroundSelectionWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float WaitingSelectionWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float FakeAttackSelectionWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float CalmRunAroundSelectionWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float CalmWaitingSelectionWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float CoinThinkAnimationWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float CoinRequestAnimationWeight { get; private set; } = 1f;

		[field: SerializeField]
		public float FakeAttackBackoffDistance { get; private set; } = 5f;

		[field: SerializeField]
		public float FakeAttackDestinationTolerance { get; private set; } = 1f;
	}
}
