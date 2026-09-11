using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Stylized/Sketch")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Sketch : VolumeComponent, IPostProcessComponent
	{
		public enum SketchProjectionMode
		{
			WorldSpace = 0,
			ScreenSpace = 1
		}

		[Serializable]
		public sealed class SketchProjectionParameter : VolumeParameter<SketchProjectionMode>
		{
		}

		public enum SketchMode
		{
			EffectOnly = 0,
			Multiply = 1,
			Add = 2
		}

		[Serializable]
		public sealed class SketchModeParameter : VolumeParameter<SketchMode>
		{
		}

		[Tooltip("The Red channel is used for darker shades, whereas the Green channel is for lighter.")]
		public TextureParameter strokeTex = new TextureParameter(null);

		[Space]
		[Tooltip("Choose the type of UV space being used")]
		public SketchProjectionParameter projectionMode = new SketchProjectionParameter
		{
			value = SketchProjectionMode.WorldSpace
		};

		[Tooltip("Choose one of the different modes")]
		public SketchModeParameter blendMode = new SketchModeParameter
		{
			value = SketchMode.EffectOnly
		};

		[Range(0f, 1f)]
		[Tooltip("Fades the effect in or out")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		public Vector2Parameter brightness = new Vector2Parameter(new Vector2(0f, 1f));

		public ClampedFloatParameter tiling = new ClampedFloatParameter(8f, 1f, 32f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active)
			{
				return intensity.value > 0f;
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
			shader = Shader.Find("Hidden/SC Post Effects/Sketch");
			return result;
		}
	}
}
