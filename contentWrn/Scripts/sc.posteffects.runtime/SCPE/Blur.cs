using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Blurring/Blur")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Blur : VolumeComponent, IPostProcessComponent
	{
		public enum BlurMethod
		{
			Gaussian = 0,
			Box = 1
		}

		[Serializable]
		public sealed class BlurMethodParameter : VolumeParameter<BlurMethod>
		{
		}

		[Tooltip("Box blurring uses fewer texture samples but has a limited blur range")]
		public BlurMethodParameter mode = new BlurMethodParameter
		{
			value = BlurMethod.Gaussian
		};

		[Tooltip("When enabled, the amount of blur passes is doubled")]
		public BoolParameter highQuality = new BoolParameter(value: false);

		public BoolParameter distanceFade = new BoolParameter(value: false);

		public FloatParameter startFadeDistance = new FloatParameter(0f);

		public FloatParameter endFadeDistance = new FloatParameter(500f);

		[Space]
		[Range(0f, 5f)]
		[Tooltip("The amount of blurring that must be performed")]
		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 5f);

		[Range(1f, 12f)]
		[Tooltip("The number of times the effect is blurred. More iterations provide a smoother effect but induce more drawcalls.")]
		public ClampedIntParameter iterations = new ClampedIntParameter(6, 1, 12);

		[Range(1f, 4f)]
		[Tooltip("Every step halfs the resolution of the blur effect. Lower resolution provides a smoother blur but may induce flickering.")]
		public ClampedIntParameter downscaling = new ClampedIntParameter(2, 1, 8);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active)
			{
				return amount.value > 0f;
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
			shader = Shader.Find("Hidden/SC Post Effects/Blur");
			return result;
		}
	}
}
