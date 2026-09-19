using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings
{
	[CreateAssetMenu(fileName = "MimicCauldronSettings_Default", menuName = "Configurations/AIModuleStateMachine/Mimic/MimicCauldronSettings")]
	public class MimicCauldronSettings : ScriptableObject
	{
		private const int DEFAULT_SHED_BLOCKING_MASK = -1679833019;

		[field: SerializeField]
		public float MinDuration { get; private set; } = 10f;

		[field: SerializeField]
		public float MaxDuration { get; private set; } = 10f;

		[field: SerializeField]
		public float UpImpulse { get; private set; } = 22.5f;

		[field: SerializeField]
		public float ForwardImpulse { get; private set; } = 16.5f;

		[field: Header("Shed headroom check")]
		[field: SerializeField]
		public float ShedCheckOriginHeight { get; private set; } = 0.3f;

		[field: SerializeField]
		public float ShedCheckDistance { get; private set; } = 1.5f;

		[field: SerializeField]
		public LayerMask ShedBlockingMask { get; private set; } = -1679833019;
	}
}
