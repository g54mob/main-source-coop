using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanRageSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanRageSettings")]
	public class HeadmanRageSettings : ScriptableObject
	{
		[field: SerializeField]
		public float RageRadius { get; private set; } = 5f;

		[field: SerializeField]
		public float RageWanderSpeed { get; private set; } = 3.5f;

		[field: SerializeField]
		public float RageDuration { get; private set; } = 20f;

		[field: SerializeField]
		public float RageApproachInterval { get; private set; } = 5f;

		[field: SerializeField]
		public float RageWanderPositionUpdateTime { get; private set; } = 2f;

		[field: SerializeField]
		public float RageInteractedDuration { get; private set; } = 2f;

		[Min(1f)]
		[field: SerializeField]
		public int RoarCount { get; private set; } = 3;
	}
}
