using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Rendering/Lens Flares")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class LensFlares : VolumeComponent, IPostProcessComponent
	{
		public BoolParameter debug = new BoolParameter(value: false);

		[Space]
		[Range(0f, 1f)]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

		[Range(0.01f, 5f)]
		[Tooltip("Luminance threshold, pixels above this threshold will contribute to the effect")]
		public ClampedFloatParameter luminanceThreshold = new ClampedFloatParameter(1f, 0.01f, 5f);

		[Header("Flares")]
		[Range(1f, 4f)]
		public ClampedIntParameter iterations = new ClampedIntParameter(2, 1, 4);

		[Range(1f, 2f)]
		[Tooltip("Offsets the Flares towards the edge of the screen")]
		public ClampedFloatParameter distance = new ClampedFloatParameter(1.5f, 1f, 2f);

		[Range(1f, 10f)]
		[Tooltip("Fades out the Flares towards the edge of the screen")]
		public ClampedFloatParameter falloff = new ClampedFloatParameter(10f, 1f, 10f);

		[Header("Halo")]
		[Tooltip("Creates a halo at the center of the screen when looking directly at a bright spot")]
		[Range(0f, 1f)]
		public ClampedFloatParameter haloSize = new ClampedFloatParameter(0.2f, 0f, 1f);

		[Range(0f, 100f)]
		public ClampedFloatParameter haloWidth = new ClampedFloatParameter(70f, 0f, 100f);

		[Header("Colors and masking")]
		[Tooltip("Use a texture to mask out the effect")]
		public TextureParameter maskTex = new TextureParameter(null);

		[Range(0f, 20f)]
		[Tooltip("Refracts the color channels")]
		public ClampedFloatParameter chromaticAbberation = new ClampedFloatParameter(10f, 0f, 20f);

		[Tooltip("Color the flares from the center of the screen to the outer edges")]
		public TextureParameter colorTex = new TextureParameter(null);

		[Header("Blur")]
		[Range(1f, 8f)]
		[Tooltip("The amount of blurring that must be performed")]
		public ClampedFloatParameter blur = new ClampedFloatParameter(2f, 1f, 8f);

		[Range(1f, 12f)]
		[Tooltip("The number of times the effect is blurred. More iterations provide a smoother effect but induce more drawcalls.")]
		public ClampedIntParameter passes = new ClampedIntParameter(3, 1, 12);

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
			shader = Shader.Find("Hidden/SC Post Effects/Lensflares");
			return result;
		}
	}
}
