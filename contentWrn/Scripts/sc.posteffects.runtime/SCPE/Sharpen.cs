using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Image/Sharpen")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Sharpen : VolumeComponent, IPostProcessComponent
	{
		public enum Method
		{
			[InspectorName("Luminance Enhancement (4 samples)")]
			LuminanceEnhancement = 0,
			[InspectorName("Contrast Adaptive (9 samples)")]
			ContrastAdaptive = 1
		}

		[Serializable]
		public sealed class MethodParam : VolumeParameter<Method>
		{
		}

		public MethodParam mode = new MethodParam
		{
			value = Method.LuminanceEnhancement
		};

		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		public ClampedFloatParameter radius = new ClampedFloatParameter(1f, 0.1f, 2f);

		public ClampedFloatParameter contrast = new ClampedFloatParameter(1f, 0f, 1f);

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
			shader = Shader.Find("Hidden/SC Post Effects/Sharpen");
			return result;
		}
	}
}
