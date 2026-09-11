using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Gradient")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Gradient : VolumeComponent, IPostProcessComponent
	{
		public enum Mode
		{
			ColorFields = 0,
			Texture = 1
		}

		[Serializable]
		public sealed class GradientModeParameter : VolumeParameter<Mode>
		{
		}

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

		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		public GradientModeParameter input = new GradientModeParameter();

		[Tooltip("The color's alpha channel controls its opacity")]
		public ColorParameter color1 = new ColorParameter(new Color(0f, 0.8f, 0.56f, 0.5f));

		[Tooltip("The color's alpha channel controls its opacity")]
		public ColorParameter color2 = new ColorParameter(new Color(0.81f, 0.37f, 1f, 0.5f));

		[Range(0f, 1f)]
		[Tooltip("Size")]
		public ClampedFloatParameter rotation = new ClampedFloatParameter(0f, 0f, 1f);

		public TextureParameter gradientTex = new TextureParameter(null);

		[Tooltip("Blends the gradient through various Photoshop-like blending modes")]
		public BlendModeParameter mode = new BlendModeParameter();

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (intensity.value > 0f || (input.value == Mode.Texture && gradientTex.value == null))
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
			shader = Shader.Find("Hidden/SC Post Effects/Gradient");
			return result;
		}
	}
}
