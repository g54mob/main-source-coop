using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.JacuzziBeachInteractableModule.Scripts.Rendering
{
	[Serializable]
	[VolumeComponentMenu("Custom/JacuzziHaze")]
	[VolumeRequiresRendererFeatures(new Type[] { typeof(JacuzziHazeFeature) })]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public class JacuzziHazeVolume : VolumeComponent, IPostProcessComponent
	{
		public const float MIN_EFFECT_WEIGHT = 0.0001f;

		[Tooltip("Enable or disable the jacuzzi distortion.")]
		public BoolParameter isEnabled = new BoolParameter(value: false);

		[Tooltip("Soak amount driving the whole effect (0 = off, 1 = full). Driven from gameplay code.")]
		public ClampedFloatParameter blend = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("Peak pixel displacement at blend 1.")]
		public ClampedFloatParameter distortionStrength = new ClampedFloatParameter(12f, 0f, 128f);

		[Tooltip("Colour fringing along the displacement direction, as a fraction of the offset. 0 is off and costs nothing.")]
		public ClampedFloatParameter chromaticAberration = new ClampedFloatParameter(0.3f, 0f, 1f);

		[Tooltip("Tiling of the noise texture assigned on the renderer feature. Lower = larger, softer distortion patches.")]
		public ClampedFloatParameter noiseTiling = new ClampedFloatParameter(2f, 0.05f, 16f);

		[Tooltip("Noise scroll speed in UV units per second. Unequal X and Y stop the pattern reading as one diagonal slide.")]
		public Vector2Parameter noiseScrollSpeed = new Vector2Parameter(new Vector2(0.03f, 0.05f));

		public void SetBlend(float blend01)
		{
			blend.value = Mathf.Clamp01(blend01);
		}

		public bool IsActive()
		{
			if (isEnabled.value && blend.value >= 0.0001f)
			{
				return distortionStrength.value > 0f;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}
	}
}
