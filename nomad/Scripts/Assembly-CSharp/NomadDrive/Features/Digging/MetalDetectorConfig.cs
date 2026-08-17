using Ami.BroAudio;
using UnityEngine;

namespace NomadDrive.Features.Digging
{
	[CreateAssetMenu(menuName = "NomadDrive/Digging/Metal Detector Config", fileName = "MetalDetectorConfig")]
	public class MetalDetectorConfig : ScriptableObject
	{
		[Header("Detection")]
		[Tooltip("Max detection range (horizontal XZ distance from the detector head to a buried treasure).")]
		[Range(1f, 30f)]
		public float scanRadius = 6f;

		[Tooltip("Planar distance under which the detector is 'directly on top' and switches to the continuous signal tone.")]
		[Range(0.05f, 3f)]
		public float signalRadius = 0.4f;

		[Tooltip("How often (seconds) to re-find the nearest treasure. Distance to the cached treasure is recomputed every frame for a smooth beep rate.")]
		[Range(0.02f, 0.5f)]
		public float scanQueryInterval = 0.1f;

		[Header("Beep Rate")]
		[Tooltip("Fastest beep interval (seconds), reached at the edge of the signal radius (closest).")]
		[Range(0.02f, 1f)]
		public float minBeepInterval = 0.08f;

		[Tooltip("Slowest beep interval (seconds), at the edge of the scan radius (farthest).")]
		[Range(0.1f, 3f)]
		public float maxBeepInterval = 0.9f;

		[Tooltip("Optional shaping of normalized distance (X: 0 near → 1 far) to the min..max interval lerp weight (Y). Default linear.")]
		public AnimationCurve beepRateCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[Header("Audio")]
		[Tooltip("Discrete click played on each beep.")]
		public SoundID beepSound;

		[Tooltip("Continuous tone (looping AudioEntity) played while directly over a treasure.")]
		public SoundID signalSound;

		[Tooltip("Fade-out applied when the continuous signal tone stops.")]
		[Range(0f, 1f)]
		public float signalFadeout = 0.15f;
	}
}
