using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Image/3D Hue Shift")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class HueShift3D : VolumeComponent, IPostProcessComponent
	{
		public enum ColorSource
		{
			RGBSpectrum = 0,
			GradientTexture = 1
		}

		[Serializable]
		public sealed class ColorSourceParameter : VolumeParameter<ColorSource>
		{
		}

		[Tooltip("Box blurring uses fewer texture samples but has a limited blur range")]
		public ColorSourceParameter colorSource = new ColorSourceParameter
		{
			value = ColorSource.RGBSpectrum
		};

		public TextureParameter gradientTex = new TextureParameter(null);

		[Range(0f, 1f)]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0f, 1f)]
		[Tooltip("Speed")]
		public ClampedFloatParameter speed = new ClampedFloatParameter(0.3f, 0f, 1f);

		[Range(0f, 3f)]
		[Tooltip("Size")]
		public ClampedFloatParameter size = new ClampedFloatParameter(1f, 0f, 3f);

		[Range(0f, 10f)]
		[Tooltip("Bends the effect over the scene's geometry normals\n\nHigh values may induce banding artifacts")]
		public ClampedFloatParameter geoInfluence = new ClampedFloatParameter(5f, 0f, 10f);

		public static bool isOrtho;

		[SerializeField]
		public Shader DepthNormalsShader;

		[SerializeField]
		public Shader shader;

		public bool RequireDepthNormals()
		{
			return true;
		}

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
			bool result = !shader || !DepthNormalsShader;
			shader = Shader.Find("Hidden/SC Post Effects/3D Hue Shift");
			DepthNormalsShader = Shader.Find("Hidden/SC Post Effects/DepthNormals");
			return result;
		}
	}
}
