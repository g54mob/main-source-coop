using System;
using Febucci.Attributes;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	public class BehaviorDefaultValues
	{
		[Serializable]
		public class Defaults
		{
			[NotZero]
			public float wiggleAmplitude = 0.15f;

			[NotZero]
			public float wiggleFrequency = 7.67f;

			[NotZero]
			public float waveFrequency = 4.78f;

			[NotZero]
			public float waveAmplitude = 0.2f;

			public float waveWaveSize = 0.18f;

			[NotZero]
			public float angleSpeed = 180f;

			public float angleDiffBetweenChars = 10f;

			[NotZero]
			public float swingAmplitude = 27.5f;

			[NotZero]
			public float swingFrequency = 5f;

			public float swingWaveSize;

			[NotZero]
			public float shakeStrength = 0.085f;

			[PositiveValue]
			public float shakeDelay = 0.04f;

			public float sizeAmplitude = 1.4f;

			[NotZero]
			public float sizeFrequency = 4.84f;

			public float sizeWaveSize = 0.18f;

			[NotZero]
			public float slideAmplitude = 0.12f;

			[NotZero]
			public float slideFrequency = 5f;

			public float slideWaveSize;

			[NotZero]
			public float bounceAmplitude = 0.08f;

			[NotZero]
			public float bounceFrequency = 1f;

			public float bounceWaveSize = 0.08f;

			[NotZero]
			public float hueShiftSpeed = 0.8f;

			public float hueShiftWaveSize = 0.08f;

			[PositiveValue]
			public float fadeDelay = 1.2f;

			[NotZero]
			public float dangleAmplitude = 0.13f;

			[NotZero]
			public float dangleFrequency = 2.41f;

			public float dangleWaveSize = 0.18f;

			public bool dangleAnchorBottom;

			[NotZero]
			public float pendAmplitude = 25f;

			[NotZero]
			public float pendFrequency = 3f;

			public float pendWaveSize = 0.2f;

			public bool pendInverted;
		}

		[SerializeField]
		[Header("Default Behaviors")]
		public Defaults defaults = new Defaults();

		[SerializeField]
		[Header("Preset Effects")]
		internal PresetBehaviorValues[] presets = new PresetBehaviorValues[0];
	}
}
