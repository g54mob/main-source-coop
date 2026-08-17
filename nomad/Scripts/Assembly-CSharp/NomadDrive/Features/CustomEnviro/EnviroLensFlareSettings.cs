using System;
using UnityEngine;

namespace NomadDrive.Features.CustomEnviro
{
	[Serializable]
	public class EnviroLensFlareSettings
	{
		[Header("Sun")]
		public float sunFlareIntensity = 1f;

		public AnimationCurve sunFlareCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.4f, 0f), new Keyframe(0.55f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f));

		[Header("Moon")]
		public float moonFlareIntensity = 0.5f;

		public AnimationCurve moonFlareCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.35f, 1f), new Keyframe(0.5f, 0f), new Keyframe(0.85f, 0f), new Keyframe(1f, 1f));

		[Header("Transition")]
		public float transitionSpeed = 2f;
	}
}
