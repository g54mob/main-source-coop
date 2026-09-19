using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.PostProcessingModule.Scripts.Rendering
{
	[Serializable]
	[VolumeComponentMenu("Custom/Flicker")]
	[VolumeRequiresRendererFeatures(new Type[] { typeof(FlickerFeature) })]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public class FlickerVolume : VolumeComponent, IPostProcessComponent
	{
		private const float MinEffectWeight = 0.0001f;

		[Tooltip("Enable or disable the flicker effect.")]
		public BoolParameter isEnabled = new BoolParameter(value: false);

		[Tooltip("Master intensity for flicker-driven parameters (0 = off, 1 = full profile values). Usually driven from code.")]
		public ClampedFloatParameter blend = new ClampedFloatParameter(1f, 0f, 1f);

		[Tooltip("Flicker frequency in Hz.")]
		public ClampedFloatParameter frequency = new ClampedFloatParameter(4f, 0.1f, 30f);

		[Tooltip("How far the effect swings between gray and color (0 = no change, 1 = full swing).")]
		public ClampedFloatParameter strength = new ClampedFloatParameter(0.5f, 0f, 1f);

		[Tooltip("Random variation in flicker timing and amplitude.")]
		public ClampedFloatParameter randomization = new ClampedFloatParameter(0.5f, 0f, 1f);

		[Header("Color Adjustments")]
		[Tooltip("Flicker saturation on top of Color Adjustments already merged in the volume stack.")]
		public BoolParameter syncSaturation = new BoolParameter(value: true);

		[Tooltip("Flicker post-exposure on top of the merged stack value. Off by default to avoid overriding scene exposure.")]
		public BoolParameter syncPostExposure = new BoolParameter(value: false);

		[Tooltip("Saturation at flicker low point. -100 matches a fully desaturated Color Adjustments override.")]
		public ClampedFloatParameter minSaturation = new ClampedFloatParameter(-100f, -100f, 100f);

		[Tooltip("Saturation at flicker peak. 0 is full color in Color Adjustments.")]
		public ClampedFloatParameter maxSaturation = new ClampedFloatParameter(0f, -100f, 100f);

		[Tooltip("Post-exposure (EV) at flicker low point. Same units as Color Adjustments.")]
		public ClampedFloatParameter minPostExposure = new ClampedFloatParameter(0f, -5f, 5f);

		[Tooltip("Post-exposure (EV) at flicker peak.")]
		public ClampedFloatParameter maxPostExposure = new ClampedFloatParameter(0f, -5f, 5f);

		[Header("Film Grain")]
		[Tooltip("Apply constant film grain (does not pulse). Intensity scales with blend only.")]
		public BoolParameter syncFilmGrain = new BoolParameter(value: false);

		public FilmGrainLookupParameter type = new FilmGrainLookupParameter(FilmGrainLookup.Large01);

		[Tooltip("Constant film grain intensity (0–1). Scales with blend only.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(1f, 0f, 1f);

		public float EffectWeight => strength.value * blend.value;

		public float EffectiveFilmGrainIntensity => intensity.value * blend.value;

		public bool HasColorAdjustmentSync
		{
			get
			{
				if (!syncSaturation.value)
				{
					return syncPostExposure.value;
				}
				return true;
			}
		}

		public bool ShouldDriveColorAdjustments
		{
			get
			{
				if (HasColorAdjustmentSync)
				{
					return EffectWeight >= 0.0001f;
				}
				return false;
			}
		}

		public bool IsActive()
		{
			if (!isEnabled.value)
			{
				return false;
			}
			if (EffectWeight >= 0.0001f && (HasColorAdjustmentSync || strength.value > 0f))
			{
				return true;
			}
			if (syncFilmGrain.value)
			{
				return EffectiveFilmGrainIntensity >= 0.0001f;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}
	}
}
