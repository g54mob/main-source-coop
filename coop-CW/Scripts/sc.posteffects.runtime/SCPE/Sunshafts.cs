using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Environment/Sun Shafts")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Sunshafts : VolumeComponent, IPostProcessComponent
	{
		public enum BlendMode
		{
			Additive = 0,
			Screen = 1
		}

		public enum SunShaftsResolution
		{
			Full = 1,
			Half = 2,
			Third = 3,
			Quarter = 4
		}

		[Serializable]
		public sealed class SunShaftsSourceParameter : VolumeParameter<BlendMode>
		{
		}

		[Serializable]
		public sealed class SunShaftsResolutionParameter : VolumeParameter<SunShaftsResolution>
		{
		}

		[Tooltip("Use the color of the Directional Light that's set as the caster")]
		public BoolParameter useCasterColor = new BoolParameter(value: true);

		[Tooltip("Use the intensity of the Directional Light that's set as the caster")]
		public BoolParameter useCasterIntensity = new BoolParameter(value: false);

		[Tooltip("Additive mode adds the sunshaft color to the image, while Screen mode perserves color values")]
		public SunShaftsSourceParameter blendMode = new SunShaftsSourceParameter
		{
			value = BlendMode.Additive
		};

		[InspectorName("Resolution")]
		[Tooltip("Low, quater resolution\n\nNormal, half resolution\n\nHigh, full resolution\n\nLower resolutions may induce jittering")]
		public SunShaftsResolutionParameter resolution = new SunShaftsResolutionParameter
		{
			value = SunShaftsResolution.Half
		};

		[Tooltip("Enabling this option doubles the amount of blurring performed. Resulting in smoother sunshafts at a higher performance cost.")]
		public BoolParameter highQuality = new BoolParameter(value: false);

		[Tooltip("Any color values over this threshold will contribute to the sunshafts effect")]
		[InspectorName("Sky color threshold")]
		public ColorParameter sunThreshold = new ColorParameter(Color.black);

		[InspectorName("Color")]
		public ColorParameter sunColor = new ColorParameter(Color.white, hdr: true, showAlpha: false, showEyeDropper: false);

		[InspectorName("Intensity")]
		public FloatParameter sunShaftIntensity = new FloatParameter(0f);

		[Range(0.1f, 1f)]
		[Tooltip("The degree to which the shafts’ brightness diminishes with distance from the caster")]
		public ClampedFloatParameter falloff = new ClampedFloatParameter(0.5f, 0.1f, 1f);

		[Tooltip("The length of the sunrays from the caster's position to the camera")]
		[Min(0f)]
		public FloatParameter length = new FloatParameter(5f);

		[Range(0f, 1f)]
		public FloatParameter noiseStrength = new FloatParameter(0f);

		public static Vector3 sunPosition;

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active && sunShaftIntensity.value > 0f)
			{
				return RenderSettings.sun;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}

		private void Reset()
		{
			SerializeShader();
		}

		private bool SerializeShader()
		{
			bool result = !shader;
			shader = Shader.Find("Hidden/SC Post Effects/Sun Shafts");
			return result;
		}
	}
}
