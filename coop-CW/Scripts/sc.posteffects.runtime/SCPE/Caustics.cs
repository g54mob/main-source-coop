using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Environment/Caustics")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Caustics : VolumeComponent, IPostProcessComponent
	{
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		public TextureParameter causticsTexture = new TextureParameter(null);

		public FloatParameter brightness = new FloatParameter(1f);

		public ClampedFloatParameter luminanceThreshold = new ClampedFloatParameter(0f, 0f, 2f);

		public BoolParameter projectFromSun = new BoolParameter(value: false);

		public FloatParameter minHeight = new FloatParameter(-5f);

		public ClampedFloatParameter minHeightFalloff = new ClampedFloatParameter(10f, 0.01f, 64f);

		public FloatParameter maxHeight = new FloatParameter(0f);

		public ClampedFloatParameter maxHeightFalloff = new ClampedFloatParameter(10f, 0.01f, 64f);

		public ClampedFloatParameter size = new ClampedFloatParameter(0.5f, 0.1f, 1f);

		public ClampedFloatParameter speed = new ClampedFloatParameter(0.2f, 0f, 1f);

		public BoolParameter distanceFade = new BoolParameter(value: false);

		public FloatParameter startFadeDistance = new FloatParameter(0f);

		public FloatParameter endFadeDistance = new FloatParameter(200f);

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
			shader = Shader.Find("Hidden/SC Post Effects/Caustics");
			return result;
		}
	}
}
