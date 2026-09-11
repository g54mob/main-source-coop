using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Tube Distortion")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class TubeDistortion : VolumeComponent, IPostProcessComponent
	{
		public enum DistortionMode
		{
			Buldged = 0,
			Pinched = 1,
			Beveled = 2
		}

		[Serializable]
		public sealed class DistortionModeParam : VolumeParameter<DistortionMode>
		{
		}

		public DistortionModeParam mode = new DistortionModeParam
		{
			value = DistortionMode.Buldged
		};

		[Range(0f, 1f)]
		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (amount.value > 0f)
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
			shader = Shader.Find("Hidden/SC Post Effects/Tube Distortion");
			return result;
		}
	}
}
