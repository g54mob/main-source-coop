using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Blurring/Double Vision")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class DoubleVision : VolumeComponent, IPostProcessComponent
	{
		public enum Mode
		{
			FullScreen = 0,
			Edges = 1
		}

		[Serializable]
		public sealed class DoubleVisionMode : VolumeParameter<Mode>
		{
		}

		[Tooltip("Choose to apply the effect over the entire screen or just the edges")]
		public DoubleVisionMode mode = new DoubleVisionMode
		{
			value = Mode.FullScreen
		};

		[Range(0f, 1f)]
		[Tooltip("Intensity")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

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
			shader = Shader.Find("Hidden/SC Post Effects/Double Vision");
			return result;
		}
	}
}
