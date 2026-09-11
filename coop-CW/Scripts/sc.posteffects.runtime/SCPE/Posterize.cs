using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Retro/Posterize")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Posterize : VolumeComponent, IPostProcessComponent
	{
		public BoolParameter hsvMode = new BoolParameter(value: false);

		[Range(0f, 256f)]
		public ClampedIntParameter levels = new ClampedIntParameter(256, 1, 256);

		[Header("Levels")]
		[Range(2f, 256f)]
		public ClampedIntParameter hue = new ClampedIntParameter(256, 2, 256);

		[Range(2f, 256f)]
		public ClampedIntParameter saturation = new ClampedIntParameter(256, 2, 256);

		[Range(2f, 256f)]
		public ClampedIntParameter value = new ClampedIntParameter(256, 2, 256);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (hsvMode.value || levels.value >= 256)
			{
				if (hsvMode.value && (hue.value < 256 || saturation.value < 256 || value.value < 256))
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
			shader = Shader.Find("Hidden/SC Post Effects/Posterize");
			return result;
		}
	}
}
