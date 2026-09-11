using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Misc/Kaleidoscope")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Kaleidoscope : VolumeComponent, IPostProcessComponent
	{
		[FormerlySerializedAs("splits")]
		[Range(0f, 10f)]
		[Tooltip("The number of times the screen is split up")]
		public ClampedIntParameter radialSplits = new ClampedIntParameter(0, 0, 10);

		[Range(1f, 6f)]
		public ClampedIntParameter horizontalSplits = new ClampedIntParameter(1, 1, 6);

		[Range(1f, 6f)]
		public ClampedIntParameter verticalSplits = new ClampedIntParameter(1, 1, 6);

		[Tooltip("Sets the pivot point (screen center is [0.5, 0.5]).")]
		public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f));

		[Space]
		public BoolParameter maintainAspectRatio = new BoolParameter(value: true);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (radialSplits.value > 0)
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
			shader = Shader.Find("Hidden/SC Post Effects/Kaleidoscope");
			return result;
		}
	}
}
