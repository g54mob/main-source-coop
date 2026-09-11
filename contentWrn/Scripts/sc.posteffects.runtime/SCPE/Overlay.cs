using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Overlay")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Overlay : VolumeComponent, IPostProcessComponent
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

		public TextureParameter overlayTex = new TextureParameter(null);

		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("The screen's luminance values control the opacity of the image")]
		public ClampedFloatParameter luminanceThreshold = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("Maintains the image aspect ratio, regardless of the screen width")]
		public BoolParameter autoAspect = new BoolParameter(value: false);

		[Tooltip("Blends the gradient through various Photoshop-like blending modes")]
		public BlendModeParameter blendMode = new BlendModeParameter();

		[Range(0f, 1f)]
		public ClampedFloatParameter tiling = new ClampedFloatParameter(0f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (intensity.value > 0f && overlayTex.value != null)
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
			shader = Shader.Find("Hidden/SC Post Effects/Overlay");
			return result;
		}
	}
}
