using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Environment/Cloud Shadows")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class CloudShadows : VolumeComponent, IPostProcessComponent
	{
		[Tooltip("The red channel of this texture is used to sample the clouds")]
		public TextureParameter texture = new TextureParameter(null);

		[Space]
		[Range(0f, 1f)]
		public ClampedFloatParameter size = new ClampedFloatParameter(0.5f, 0f, 1f);

		[Range(0f, 1f)]
		public ClampedFloatParameter density = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		public ClampedFloatParameter speed = new ClampedFloatParameter(0.5f, 0f, 1f);

		[Tooltip("Set the X and Z world-space direction the clouds should move in")]
		public Vector2Parameter direction = new Vector2Parameter(new Vector2(0f, 1f));

		public BoolParameter projectFromSun = new BoolParameter(value: false);

		public FloatParameter startFadeDistance = new FloatParameter(0f);

		public FloatParameter endFadeDistance = new FloatParameter(200f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (density.value > 0f && (bool)texture.value)
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
			shader = Shader.Find("Hidden/SC Post Effects/Cloud Shadows");
			return result;
		}
	}
}
