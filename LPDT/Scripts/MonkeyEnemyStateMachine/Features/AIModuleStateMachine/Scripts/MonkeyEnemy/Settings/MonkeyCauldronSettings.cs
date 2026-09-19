using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyCauldronSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyCauldronSettings")]
	public class MonkeyCauldronSettings : ScriptableObject
	{
		[field: SerializeField]
		public float MinDuration { get; private set; } = 20f;

		[field: SerializeField]
		public float MaxDuration { get; private set; } = 30f;

		[field: SerializeField]
		public float UpImpulse { get; private set; } = 2f;

		[field: SerializeField]
		public float ForwardImpulse { get; private set; } = 1.5f;

		[field: Header("Panic jumping")]
		[field: SerializeField]
		public float IdleDuration { get; private set; } = 1f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float JumpAnimationStartNormalizedTime { get; private set; } = 0.5f;

		[field: SerializeField]
		public float JumpMoveDuration { get; private set; } = 1.2f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float JumpPeakNormalizedTime { get; private set; } = 0.5f;

		[field: SerializeField]
		public float JumpRadius { get; private set; } = 4f;

		[field: SerializeField]
		public float JumpSpeed { get; private set; } = 6f;

		[field: SerializeField]
		public int AngrySoundJumpInterval { get; private set; } = 3;
	}
}
