using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	[CreateAssetMenu(fileName = "LineArmConfiguration_Default", menuName = "Configurations/Arm/LineArmConfiguration")]
	public class LineArmConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public bool AutoMultiGrab { get; set; }

		[field: SerializeField]
		public bool AutoFirstGrab { get; set; }

		[field: SerializeField]
		public float VirtualCursorBaseSpeed { get; private set; } = 1400f;

		[field: SerializeField]
		public Vector2 VirtualCursorScreenPadding { get; private set; } = new Vector2(16f, 16f);

		[field: SerializeField]
		public float VirtualCursorSensitivityMultiplier { get; private set; } = 1f;

		[field: SerializeField]
		public float VirtualCursorMaxStepPerFrame { get; private set; } = 40f;
	}
}
