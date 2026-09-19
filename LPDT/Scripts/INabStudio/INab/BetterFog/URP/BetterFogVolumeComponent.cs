using System;
using INab.BetterFog.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	[VolumeComponentMenu("INab Studio/Better Fog")]
	[VolumeRequiresRendererFeatures(new Type[] { typeof(BetterFog) })]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class BetterFogVolumeComponent : VolumeComponent, IPostProcessComponent
	{
		[Serializable]
		public sealed class FogParameterURP : VolumeParameter<FogMode>
		{
			public FogParameterURP(FogMode value, bool overrideState = false)
				: base(value, overrideState)
			{
			}
		}

		[Serializable]
		public sealed class HeightFogParameterURP : VolumeParameter<HeightFogType>
		{
			public HeightFogParameterURP(HeightFogType value, bool overrideState = false)
				: base(value, overrideState)
			{
			}
		}

		[Serializable]
		public sealed class NoiseAffectParameterURP : VolumeParameter<NoiseAffect>
		{
			public NoiseAffectParameterURP(NoiseAffect value, bool overrideState = false)
				: base(value, overrideState)
			{
			}
		}

		[Serializable]
		public class MyTextureParameter : TextureParameter
		{
			public float LerpValue;

			public Texture FromTexture;

			public MyTextureParameter(Texture value, bool overrideState = false)
				: this(value, TextureDimension.Any, overrideState)
			{
			}

			public MyTextureParameter(Texture value, TextureDimension dimension, bool overrideState = false)
				: base(value, overrideState)
			{
				base.dimension = dimension;
			}

			public override void Interp(Texture from, Texture to, float t)
			{
				LerpValue = t;
				FromTexture = from;
				value = to;
			}
		}

		public ClampedFloatParameter _FogIntensity = new ClampedFloatParameter(1f, 0f, 1f);

		public FloatParameter _EnergyLoss = new ClampedFloatParameter(0f, 0f, 1f);

		public BoolParameter _UseGradient = new BoolParameter(value: false);

		public ColorParameter _FogColor = new ColorParameter(new Color(0.8f, 0.8f, 0.8f, 1f));

		public FloatParameter _GradientStart = new FloatParameter(0f);

		public FloatParameter _GradientEnd = new FloatParameter(100f);

		public MyTextureParameter _GradientTexture = new MyTextureParameter(null);

		public BoolParameter _UseSunLight = new BoolParameter(value: false);

		public ColorParameter _SunColor = new ColorParameter(new Color(0.9f, 0.85f, 0.8f, 1f));

		public ClampedFloatParameter _SunIntensity = new ClampedFloatParameter(0f, 0f, 1f);

		public ClampedFloatParameter _SunPower = new ClampedFloatParameter(2f, 0.1f, 12f);

		public BoolParameter _UseDistanceFog = new BoolParameter(value: true);

		public BoolParameter _UseRadialDistance = new BoolParameter(value: false);

		public FogParameterURP _FogType = new FogParameterURP(FogMode.ExponentialSquared);

		public FloatParameter _DistanceFogOffset = new FloatParameter(-20f);

		public FloatParameter _SceneStart = new FloatParameter(10f);

		public FloatParameter _SceneEnd = new FloatParameter(100f);

		public ClampedFloatParameter _FogDensity = new ClampedFloatParameter(0f, 0f, 0.1f);

		public BoolParameter _UseSkyboxHeightFog = new BoolParameter(value: false);

		public ClampedFloatParameter _SkyboxFogOffset = new ClampedFloatParameter(0f, -0.1f, 0.1f);

		public ClampedFloatParameter _SkyboxFogHardness = new ClampedFloatParameter(0.75f, 0f, 0.999f);

		public ClampedFloatParameter _SkyboxFogIntensity = new ClampedFloatParameter(1f, 0f, 1f);

		public ClampedFloatParameter _SkyboxFill = new ClampedFloatParameter(0f, 0f, 1f);

		public BoolParameter _UseHeightFog = new BoolParameter(value: false);

		public FloatParameter _Height = new FloatParameter(4f);

		public ClampedFloatParameter _HeightDensity = new ClampedFloatParameter(0f, 0f, 0.5f);

		public HeightFogParameterURP _HeightFogType = new HeightFogParameterURP(HeightFogType.ExponentialSquared);

		public BoolParameter _UseNoise = new BoolParameter(value: false);

		public NoiseAffectParameterURP _NoiseAffect = new NoiseAffectParameterURP(NoiseAffect.Both);

		public ClampedFloatParameter _NoiseIntensity = new ClampedFloatParameter(0f, 0f, 1f);

		public FloatParameter _NoiseDistanceEnd = new FloatParameter(80f);

		public ClampedFloatParameter _NoiseEndHardness = new ClampedFloatParameter(0.35f, 1f, 16f);

		public ClampedFloatParameter _Scale1 = new ClampedFloatParameter(40f, 5f, 140f);

		public ClampedFloatParameter _Lerp1 = new ClampedFloatParameter(0.5f, 0f, 1f);

		public Vector3Parameter _NoiseSpeed1 = new Vector3Parameter(new Vector3(0f, 0f, 0f));

		public ClampedFloatParameter _NoiseTimeScale1 = new ClampedFloatParameter(0.1f, 0f, 0.5f);

		public BoolParameter _UseSSMS = new BoolParameter(value: false);

		public ClampedFloatParameter _Threshold = new ClampedFloatParameter(0f, -1f, 1f);

		public FloatParameter _SoftKnee = new FloatParameter(0.5f);

		public ClampedFloatParameter _Radius = new ClampedFloatParameter(7f, 1f, 7f);

		public ClampedFloatParameter _BlurWeight = new ClampedFloatParameter(1f, 0.1f, 100f);

		public ClampedFloatParameter _Intensity = new ClampedFloatParameter(1f, 0f, 1f);

		public BoolParameter _HighQuality = new BoolParameter(value: false);

		public BoolParameter _AntiFlicker = new BoolParameter(value: false);

		public TextureParameter _FadeRamp = new TextureParameter(null);

		public BetterFogVolumeComponent()
		{
			base.displayName = "Better Fog";
		}

		public bool IsActive()
		{
			return _FogIntensity.value > 0f;
		}
	}
}
