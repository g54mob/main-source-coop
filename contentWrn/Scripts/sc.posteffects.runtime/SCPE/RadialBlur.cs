using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Blurring/Radial Blur")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class RadialBlur : VolumeComponent, IPostProcessComponent
	{
		[Range(0f, 1f)]
		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		[Space]
		[Tooltip("Sets the blur center point (screen center is [0.5, 0.5]).")]
		public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f));

		[Range(-180f, 180f)]
		public ClampedFloatParameter angle = new ClampedFloatParameter(0f, -180f, 180f);

		[Space]
		[Range(3f, 12f)]
		public ClampedIntParameter iterations = new ClampedIntParameter(6, 3, 12);

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
			shader = Shader.Find("Hidden/SC Post Effects/Radial Blur");
			return result;
		}
	}
}
