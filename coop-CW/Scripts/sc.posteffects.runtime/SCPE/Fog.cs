using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Environment/Fog")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class Fog : VolumeComponent, IPostProcessComponent
	{
		[Serializable]
		public sealed class FogModeParameter : VolumeParameter<FogMode>
		{
		}

		public enum FogColorSource
		{
			UniformColor = 0,
			GradientTexture = 1,
			SkyboxColor = 2
		}

		[Serializable]
		public sealed class FogColorSourceParameter : VolumeParameter<FogColorSource>
		{
		}

		[Range(0f, 1f)]
		[Tooltip("Use the settings of the current active scene found under the Lighting tab\n\nThis is also advisable for third-party scripts that modify fog settings\n\nThis will force the effect to use the scene's fog color")]
		public BoolParameter useSceneSettings = new BoolParameter(value: false);

		[Tooltip("Sets how the fog distance is calculated")]
		public FogModeParameter fogMode = new FogModeParameter
		{
			value = FogMode.Exponential
		};

		[Range(0f, 1f)]
		public ClampedFloatParameter globalDensity = new ClampedFloatParameter(0f, 0f, 1f);

		public FloatParameter fogStartDistance = new FloatParameter(0f);

		public FloatParameter fogEndDistance = new FloatParameter(600f);

		[Space]
		[Tooltip("Color: use a uniform color for the fog\n\nGradient: sample a gradient texture to control the fog color over distance, the alpha channel controls the density\n\nSkybox: Sample the skybox's color for the fog, only works well with low detail skies")]
		public FogColorSourceParameter colorSource = new FogColorSourceParameter
		{
			value = FogColorSource.UniformColor
		};

		[ColorUsage(true, true)]
		public ColorParameter fogColor = new ColorParameter(new Color(0.76f, 0.94f, 1f, 1f), hdr: true, showAlpha: false, showEyeDropper: true);

		public TextureParameter fogColorGradient = new TextureParameter(null);

		[Tooltip("Automatic mode uses the current camera's far clipping plane to set the max distance\n\nOtherwise, a fixed value may be used instead")]
		public FloatParameter gradientDistance = new FloatParameter(1000f);

		public BoolParameter gradientUseFarClipPlane = new BoolParameter(value: true);

		[Header("Distance")]
		public BoolParameter distanceFog = new BoolParameter(value: true);

		[Min(0f)]
		public FloatParameter distanceDensity = new FloatParameter(1f);

		[Tooltip("Distance based on radial distance from viewer, rather than parrallel")]
		public BoolParameter useRadialDistance = new BoolParameter(value: true);

		public Vector2Parameter horizonMinMax = new Vector2Parameter(new Vector2(0f, 1f));

		[Range(0f, 1f)]
		[Tooltip("Determines how much the fog influences the skybox")]
		public ClampedFloatParameter skyboxInfluence = new ClampedFloatParameter(1f, 0f, 1f);

		[Header("Directional Light")]
		[Tooltip("Translates the a Directional Light's direction and color into the fog. Creates a faux-atmospheric scattering effect.")]
		public BoolParameter enableDirectionalLight = new BoolParameter(value: false);

		[Tooltip("Use the intensity of the Directional Light that's set as the caster")]
		public BoolParameter useLightDirection = new BoolParameter(value: true);

		[Tooltip("Use the color of the Directional Light that's set as the caster")]
		public BoolParameter useLightColor = new BoolParameter(value: true);

		[Tooltip("Use the intensity of the Directional Light that's set as the caster")]
		public BoolParameter useLightIntensity = new BoolParameter(value: true);

		[ColorUsage(true, true)]
		public ColorParameter lightColor = new ColorParameter(new Color(1f, 0.89f, 0.55f, 1f));

		public Vector3Parameter lightDirection = new Vector3Parameter(new Vector3(0f, 0.5f, -1f));

		public FloatParameter lightIntensity = new FloatParameter(1f);

		public ClampedFloatParameter lightExponent = new ClampedFloatParameter(8f, 1f, 100f);

		[Header("Height")]
		[Tooltip("Enable vertical height fog")]
		public BoolParameter heightFog = new BoolParameter(value: false);

		[Tooltip("Height relative to 0 world height position")]
		public FloatParameter height = new FloatParameter(10f);

		[Range(0.001f, 1f)]
		public FloatParameter heightDensity = new FloatParameter(0.75f);

		[Header("Height noise (2D)")]
		[Tooltip("Enables height fog density variation through the use of a texture")]
		public BoolParameter heightFogNoise = new BoolParameter(value: true);

		[Tooltip("The density is read from this texture's red color channel")]
		public TextureParameter heightNoiseTex = new TextureParameter(null);

		[Range(0f, 1f)]
		public ClampedFloatParameter heightNoiseSize = new ClampedFloatParameter(0.25f, 0f, 1f);

		[Range(0f, 1f)]
		public ClampedFloatParameter heightNoiseStrength = new ClampedFloatParameter(1f, 0f, 1f);

		[Range(0f, 10f)]
		public ClampedFloatParameter heightNoiseSpeed = new ClampedFloatParameter(2f, 0f, 10f);

		[Header("Light scattering")]
		[Tooltip("Execute a bloom pass to diffuse light in dense fog")]
		public BoolParameter lightScattering = new BoolParameter(value: false);

		[Space]
		[Min(0f)]
		public FloatParameter scatterIntensity = new FloatParameter(10f);

		[Min(0f)]
		[Tooltip("Filters out pixels under this level of brightness. Value is in gamma-space.")]
		public FloatParameter scatterThreshold = new FloatParameter(1f);

		[Range(1f, 10f)]
		public ClampedFloatParameter scatterDiffusion = new ClampedFloatParameter(10f, 1f, 10f);

		[Range(0f, 1f)]
		[Tooltip("Makes transitions between under/over-threshold gradual. 0 for a hard threshold, 1 for a soft threshold).")]
		public ClampedFloatParameter scatterSoftKnee = new ClampedFloatParameter(0.5f, 0f, 1f);

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (globalDensity.value > 0f)
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
			shader = Shader.Find("Hidden/SC Post Effects/Fog");
			return result;
		}
	}
}
