using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Image/Color Grading LUT")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class LUT : VolumeComponent, IPostProcessComponent
	{
		public enum Mode
		{
			Single = 0,
			DistanceBased = 1
		}

		[Serializable]
		public sealed class ModeParam : VolumeParameter<Mode>
		{
		}

		[Tooltip("Distance-based mode blends two LUTs over a distance")]
		public ModeParam mode = new ModeParam
		{
			value = Mode.Single
		};

		public FloatParameter startFadeDistance = new FloatParameter(0f);

		public FloatParameter endFadeDistance = new FloatParameter(1000f);

		[Range(0f, 1f)]
		[Tooltip("Fades the effect in or out")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("Supply a LUT strip texture.")]
		public TextureParameter lutNear = new TextureParameter(null);

		public TextureParameter lutFar = new TextureParameter(null);

		[Range(0f, 1f)]
		public ClampedFloatParameter invert = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Increases the saturation of muted colors")]
		public ClampedFloatParameter vibrance = new ClampedFloatParameter(0f, 0f, 1f);

		[ColorUsage(false, false)]
		[Tooltip("Controls the effect of vibrancy for each color channel (RGB)")]
		public ColorParameter vibranceRGBBalance = new ColorParameter(Color.white);

		public static bool Bypass;

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active)
			{
				if (!(intensity.value > 0f) && !(invert.value > 0f))
				{
					return vibrance.value > 0f;
				}
				return true;
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
			shader = Shader.Find("Hidden/SC Post Effects/LUT");
			return result;
		}
	}
}
