using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Rendering/AmbientOcclusion2D")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class AmbientOcclusion2D : VolumeComponent, IPostProcessComponent
	{
		[Tooltip("Shows only the effect, to alow for finetuning")]
		public BoolParameter aoOnly = new BoolParameter(value: false);

		[Header("Luminance-Based Amient Occlusion")]
		[Range(0f, 1f)]
		[Tooltip("Intensity")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0.01f, 1f)]
		[Tooltip("Luminance threshold, pixels above this threshold will contribute to the effect")]
		public ClampedFloatParameter luminanceThreshold = new ClampedFloatParameter(0.05f, 0f, 1f);

		[Range(0f, 3f)]
		[Tooltip("Distance")]
		public ClampedFloatParameter distance = new ClampedFloatParameter(0.3f, 0f, 3f);

		[Header("Blur")]
		[Range(0f, 3f)]
		[Tooltip("The amount of blurring that must be performed")]
		public ClampedFloatParameter blurAmount = new ClampedFloatParameter(0.85f, 0f, 3f);

		[Range(1f, 8f)]
		[Tooltip("The number of times the effect is blurred. More iterations provide a smoother effect but induce more drawcalls.")]
		public ClampedIntParameter iterations = new ClampedIntParameter(4, 1, 8);

		[Range(1f, 4f)]
		[Tooltip("Every step halves the resolution of the blur effect. Lower resolution provides a smoother blur but may induce flickering.")]
		public ClampedIntParameter downscaling = new ClampedIntParameter(2, 1, 4);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (intensity.value > 0f)
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
			shader = Shader.Find("Hidden/SC Post Effects/Ambient Occlusion 2D");
			return result;
		}
	}
}
