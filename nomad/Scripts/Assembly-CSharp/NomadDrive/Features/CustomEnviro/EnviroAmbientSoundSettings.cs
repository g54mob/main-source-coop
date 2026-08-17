using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.CustomEnviro
{
	[Serializable]
	public class EnviroAmbientSoundSettings
	{
		public List<AmbientLayer> layers = new List<AmbientLayer>();

		[Tooltip("Volume smoothing speed (Lerp factor per second). Higher = snappier.")]
		public float transitionSpeed = 1.5f;

		[Tooltip("Below this smoothed volume the layer loop is stopped to free a voice.")]
		[Range(0f, 0.05f)]
		public float silenceThreshold = 0.01f;

		[Tooltip("Minimum volume change before pushing a new value to BroAudio (throttles SetParameter).")]
		[Range(0f, 0.05f)]
		public float volumeEpsilon = 0.01f;
	}
}
