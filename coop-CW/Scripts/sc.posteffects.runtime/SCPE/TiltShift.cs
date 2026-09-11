using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Blurring/Tilt Shift")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class TiltShift : VolumeComponent, IPostProcessComponent
	{
		public enum TiltShiftMethod
		{
			Horizontal = 0,
			Radial = 1
		}

		[Serializable]
		public sealed class TiltShifMethodParameter : VolumeParameter<TiltShiftMethod>
		{
		}

		public enum Quality
		{
			Performance = 0,
			Appearance = 1
		}

		[Serializable]
		public sealed class TiltShiftQualityParameter : VolumeParameter<Quality>
		{
		}

		[Tooltip("The amount of blurring that must be performed")]
		public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

		public TiltShifMethodParameter mode = new TiltShifMethodParameter();

		[Tooltip("Choose to use more texture samples, for a smoother blur when using a high blur amout")]
		public TiltShiftQualityParameter quality = new TiltShiftQualityParameter();

		public ClampedFloatParameter areaSize = new ClampedFloatParameter(0.5f, 0f, 1f);

		public ClampedFloatParameter areaFalloff = new ClampedFloatParameter(1f, 0.01f, 1f);

		public ClampedFloatParameter offset = new ClampedFloatParameter(0f, -1f, 1f);

		public ClampedFloatParameter angle = new ClampedFloatParameter(0f, 0f, 360f);

		public static bool debug;

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
			shader = Shader.Find("Hidden/SC Post Effects/Tilt Shift");
			return result;
		}
	}
}
