using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Retro/Scanlines")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Scanlines : VolumeComponent, IPostProcessComponent
	{
		[Range(0f, 1f)]
		[Tooltip("Intensity")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Lines")]
		public ClampedFloatParameter amount = new ClampedFloatParameter(700f, 0f, 2048f);

		[Range(-1f, 1f)]
		[Tooltip("Animation speed")]
		public ClampedFloatParameter speed = new ClampedFloatParameter(0f, -1f, 1f);

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
			shader = Shader.Find("Hidden/SC Post Effects/Scanlines");
			return result;
		}
	}
}
