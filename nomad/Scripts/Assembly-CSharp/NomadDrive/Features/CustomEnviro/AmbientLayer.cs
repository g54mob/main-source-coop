using System;
using Ami.BroAudio;
using UnityEngine;

namespace NomadDrive.Features.CustomEnviro
{
	[Serializable]
	public class AmbientLayer
	{
		[Tooltip("Editor-only label for clarity, e.g. \"Day Birds\".")]
		public string label;

		[Tooltip("Distinct looping Ambience AudioEntity. Never reuse the same SoundID across layers.")]
		public SoundID sound;

		[Range(0f, 1f)]
		public float maxVolume = 1f;

		[Tooltip("X = solar time 0..1 (0/1 midnight, 0.5 noon), Y = volume weight 0..1.")]
		public AnimationCurve volumeOverDay = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
	}
}
