using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.PostProcessingModule.Scripts.Rendering
{
	[Serializable]
	[VolumeComponentMenu("Custom/Camera Water Lens")]
	[VolumeRequiresRendererFeatures(new Type[] { typeof(CameraWaterLensFeature) })]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public class CameraWaterLensVolume : VolumeComponent, IPostProcessComponent
	{
		private const float MIN_INTENSITY = 0.0001f;

		public BoolParameter isEnabled = new BoolParameter(value: false);

		[Tooltip("Master wetness intensity (0 = off, 1 = fully soaked lens). Driven from code.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("UV ripple / refraction strength at full wetness.")]
		public ClampedFloatParameter distortionStrength = new ClampedFloatParameter(0.12f, 0f, 0.3f);

		[Tooltip("Screen blur amount at full wetness.")]
		public ClampedFloatParameter blurStrength = new ClampedFloatParameter(0.018f, 0f, 0.05f);

		[Tooltip("How fast ripples flow across the lens.")]
		public ClampedFloatParameter flowSpeed = new ClampedFloatParameter(1.4f, 0.1f, 5f);

		public bool IsActive()
		{
			if (isEnabled.value)
			{
				return intensity.value > 0.0001f;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}
	}
}
