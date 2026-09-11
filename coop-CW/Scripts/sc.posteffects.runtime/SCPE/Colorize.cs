using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Image/Colorize")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Colorize : VolumeComponent, IPostProcessComponent
	{
		public enum BlendMode
		{
			Linear = 0,
			Additive = 1,
			Multiply = 2,
			Screen = 3
		}

		[Serializable]
		public sealed class BlendModeParameter : VolumeParameter<BlendMode>
		{
		}

		[Tooltip("Blends the gradient through various Photoshop-like blending modes")]
		public BlendModeParameter mode = new BlendModeParameter
		{
			value = BlendMode.Linear
		};

		[Range(0f, 1f)]
		[Tooltip("Fades the effect in or out")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("Supply a gradient texture.\n\nLuminance values are colorized from left to right")]
		public TextureParameter colorRamp = new TextureParameter(null);

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
			shader = Shader.Find("Hidden/SC Post Effects/Colorize");
			return result;
		}
	}
}
