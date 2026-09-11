using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Stylized/Kuwahara")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Kuwahara : VolumeComponent, IPostProcessComponent
	{
		public enum KuwaharaMode
		{
			FullScreen = 0,
			DepthFade = 1
		}

		[Serializable]
		public sealed class KuwaharaModeParam : VolumeParameter<KuwaharaMode>
		{
		}

		[Tooltip("Choose to apply the effect to the entire screen, or fade in/out over a distance")]
		public KuwaharaModeParam mode = new KuwaharaModeParam
		{
			value = KuwaharaMode.FullScreen
		};

		public ClampedIntParameter radius = new ClampedIntParameter(0, 0, 8);

		public FloatParameter startFadeDistance = new FloatParameter(100f);

		public FloatParameter endFadeDistance = new FloatParameter(500f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (radius.value > 0)
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
			shader = Shader.Find("Hidden/SC Post Effects/Kuwahara");
			return result;
		}
	}
}
