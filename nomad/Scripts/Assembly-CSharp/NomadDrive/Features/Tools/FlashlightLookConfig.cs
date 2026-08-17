using UnityEngine;

namespace NomadDrive.Features.Tools
{
	[CreateAssetMenu(menuName = "NomadDrive/Configs/Tools/Flashlight Look Config")]
	public class FlashlightLookConfig : ScriptableObject
	{
		[Tooltip("Max degrees the beam may tilt UPWARD (toward the sky) from its rest forward. Keep this high.")]
		[Range(0f, 80f)]
		public float maxAngleUp = 45f;

		[Tooltip("Max degrees the beam may tilt DOWNWARD (toward the ground) from its rest forward. Keep this low.")]
		[Range(0f, 80f)]
		public float maxAngleDown = 12f;

		[Tooltip("Max degrees the beam may swing left/right (horizontal) from its rest forward.")]
		[Range(0f, 80f)]
		public float maxAngleHorizontal = 30f;

		[Tooltip("How fast the owner's beam follows the look direction (exponential decay).")]
		[Range(1f, 30f)]
		public float followSmoothness = 10f;

		[Tooltip("How fast remote clients interpolate toward received aim updates. Keep above the send rate to smooth the network step.")]
		[Range(1f, 40f)]
		public float remoteFollowSmoothness = 16f;

		[Tooltip("How fast the beam eases back to its rest pose when not equipped (exponential decay).")]
		[Range(1f, 30f)]
		public float returnSmoothness = 8f;
	}
}
