using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Danger")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Danger : VolumeComponent, IPostProcessComponent
	{
		public TextureParameter overlayTex = new TextureParameter(null);

		public ColorParameter color = new ColorParameter(new Color(0.66f, 0f, 0f));

		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		public ClampedFloatParameter size = new ClampedFloatParameter(0f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (!(size.value > 0f))
			{
				if (intensity.value > 0f)
				{
					return active;
				}
				return false;
			}
			return true;
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
			shader = Shader.Find("Hidden/SC Post Effects/Danger");
			return result;
		}
	}
}
