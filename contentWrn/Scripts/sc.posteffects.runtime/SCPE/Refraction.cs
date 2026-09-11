using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Screen/Refraction")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Refraction : VolumeComponent, IPostProcessComponent
	{
		[FormerlySerializedAs("refractionTex")]
		[Tooltip("Takes a normal map to perturb the image")]
		public TextureParameter normalMap = new TextureParameter(null);

		[Range(0f, 1f)]
		[Tooltip("Amount")]
		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		public ColorParameter tint = new ColorParameter(new Color(1f, 1f, 1f, 0.1f));

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (amount.value > 0f && normalMap.value != null)
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
			shader = Shader.Find("Hidden/SC Post Effects/Refraction");
			return result;
		}
	}
}
