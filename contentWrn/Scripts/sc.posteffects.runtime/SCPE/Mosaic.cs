using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Stylized/Mosaic")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Mosaic : VolumeComponent, IPostProcessComponent
	{
		public enum MosaicMode
		{
			Triangles = 0,
			Hexagons = 1,
			Circles = 2
		}

		[Serializable]
		public sealed class MosaicModeParam : VolumeParameter<MosaicMode>
		{
		}

		public MosaicModeParam mode = new MosaicModeParam
		{
			value = MosaicMode.Hexagons
		};

		[Range(0f, 1f)]
		[Tooltip("Size")]
		public ClampedFloatParameter size = new ClampedFloatParameter(0f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (size.value > 0f)
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
			shader = Shader.Find("Hidden/SC Post Effects/Mosaic");
			return result;
		}
	}
}
