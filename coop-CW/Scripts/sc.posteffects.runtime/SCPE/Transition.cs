using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Transition")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Transition : VolumeComponent, IPostProcessComponent
	{
		public TextureParameter gradientTex = new TextureParameter(null);

		public ClampedFloatParameter progress = new ClampedFloatParameter(0f, 0f, 1f);

		public BoolParameter invert = new BoolParameter(value: false);

		public ColorParameter color = new ColorParameter(Color.black);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active && progress.value > 0f)
			{
				return color.value.a > 0f;
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
			shader = Shader.Find("Hidden/SC Post Effects/Transition");
			return result;
		}
	}
}
