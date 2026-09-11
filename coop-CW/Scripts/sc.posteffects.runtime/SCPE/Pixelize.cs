using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Retro/Pixelize")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Pixelize : VolumeComponent, IPostProcessComponent
	{
		public enum Resolution
		{
			Custom = 1,
			[InspectorName("600p")]
			Sixhundred = 600,
			[InspectorName("480p")]
			FourEighty = 480,
			[InspectorName("240p")]
			TwoFourty = 240,
			[InspectorName("200p")]
			TwoHundred = 200,
			[InspectorName("160p")]
			HundredSixty = 160
		}

		[Serializable]
		public sealed class ResolutionPreset : VolumeParameter<Resolution>
		{
		}

		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		public IntParameter resolution = new IntParameter(240);

		public ResolutionPreset resolutionPreset = new ResolutionPreset
		{
			value = Resolution.Custom
		};

		[Tooltip("When disabled, pixels will retain a square aspect ratio")]
		public BoolParameter preserveAspectRatio = new BoolParameter(value: false);

		[Tooltip("When enabled, pixels are shifted by half. Mostly has a visible effect on extremely low resolutions")]
		public BoolParameter centerPixel = new BoolParameter(value: true);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (amount.value > 0f)
			{
				return active;
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
			shader = Shader.Find("Hidden/SC Post Effects/Pixelize");
			return result;
		}
	}
}
