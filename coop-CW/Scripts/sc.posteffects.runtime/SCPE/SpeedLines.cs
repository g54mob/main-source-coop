using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Speed Lines")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class SpeedLines : VolumeComponent, IPostProcessComponent
	{
		[Tooltip("Assign any grayscale texture with a vertically repeating pattern and a falloff from left to right")]
		public TextureParameter noiseTex = new TextureParameter(null);

		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Determines the radial tiling of the noise texture")]
		public ClampedFloatParameter size = new ClampedFloatParameter(0.5f, 0f, 1f);

		[Range(0f, 1f)]
		public ClampedFloatParameter falloff = new ClampedFloatParameter(0.25f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (intensity.value > 0f && (bool)noiseTex.value)
			{
				return active;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}

		private bool SerializeShader()
		{
			bool result = !shader;
			shader = Shader.Find("Hidden/SC Post Effects/SpeedLines");
			return result;
		}
	}
}
