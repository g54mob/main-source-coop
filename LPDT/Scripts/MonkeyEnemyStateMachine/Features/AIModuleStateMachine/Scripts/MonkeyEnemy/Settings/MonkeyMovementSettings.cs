using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyMovementSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyMovementSettings")]
	public class MonkeyMovementSettings : ScriptableObject
	{
		[field: SerializeField]
		public float WanderingSpeed { get; private set; } = 1f;

		[field: SerializeField]
		public float WanderRadius { get; private set; } = 30f;

		[field: SerializeField]
		public float StoppingDistance { get; private set; } = 1f;

		[field: SerializeField]
		public float MinWanderPositionUpdateTime { get; private set; } = 6f;

		[field: SerializeField]
		public float MaxWanderPositionUpdateTime { get; private set; } = 8f;

		[field: SerializeField]
		public float RotationSpeed { get; private set; } = 5f;
	}
}
