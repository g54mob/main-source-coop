using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Retro/Dithering")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Dithering : VolumeComponent, IPostProcessComponent
	{
		[Tooltip("Note that the texture's filter mode (Point or Bilinear) greatly affects the behavior of the pattern")]
		public TextureParameter lut = new TextureParameter(null);

		[Range(0f, 1f)]
		[Tooltip("Fades the effect in or out")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		[Tooltip("The screen's luminance values control the density of the dithering matrix")]
		public ClampedFloatParameter luminanceThreshold = new ClampedFloatParameter(0.5f, 0f, 1f);

		public ClampedFloatParameter tiling = new ClampedFloatParameter(1f, 0f, 2f);

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
			shader = Shader.Find("Hidden/SC Post Effects/Dithering");
			return result;
		}
	}
}
