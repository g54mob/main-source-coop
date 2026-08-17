using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroFogSettings
	{
		public enum Quality
		{
			Low = 0,
			Medium = 1,
			High = 2
		}

		public enum FogQualityMode
		{
			Normal = 0,
			Simple = 1
		}

		public bool volumetrics = true;

		public int steps = 32;

		public Quality quality;

		[Range(0f, 2f)]
		public float scattering;

		public AnimationCurve scatteringMultiplier = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		[Range(0f, 1f)]
		public float extinction;

		[Range(0f, 1f)]
		public float anistropy;

		public float maxRange = 1000f;

		public float maxRangePointSpot = 100f;

		[Range(0f, 1f)]
		public float noiseIntensity;

		[Range(0f, 0.01f)]
		public float noiseScale;

		public Vector3 windDirection;

		public Texture3D noise;

		public Texture2D ditheringTex;

		public bool fog = true;

		public FogQualityMode fogQualityMode;

		public Vector3 floatingPointOriginMod;

		public float globalFogHeight;

		[Range(0f, 1f)]
		public float fogDensity = 0.02f;

		[Range(0.001f, 0.1f)]
		public float fogHeightFalloff = 0.2f;

		public float fogHeight;

		[Range(0f, 1f)]
		public float fogDensity2 = 0.02f;

		[Range(0.001f, 0.1f)]
		public float fogHeightFalloff2 = 0.2f;

		public float fogHeight2;

		[Range(0f, 1f)]
		public float fogMaxOpacity = 1f;

		[Range(0.01f, 5000f)]
		public float startDistance;

		[Range(0f, 1f)]
		public float fogColorBlend = 0.5f;

		public Color fogColorMod = Color.white;

		public bool blockScattering;

		[GradientUsage(true)]
		public Gradient ambientColorGradient;

		public bool controlHDRPFog;

		public float fogAttenuationDistance = 400f;

		public float baseHeight;

		public float maxHeight = 50f;

		[GradientUsage(true)]
		public Gradient fogColorTint;

		public bool controlHDRPVolumetrics;

		[GradientUsage(true)]
		public Gradient volumetricsColorTint;

		[Range(0f, 1f)]
		public float ambientDimmer = 1f;

		[Range(0f, 10f)]
		public float directLightMultiplier = 1f;

		[Range(0f, 1f)]
		public float directLightShadowdimmer = 1f;

		public bool unityFog;

		public FogMode unityFogMode = FogMode.Exponential;

		public float unityFogDensity = 0.002f;

		public float unityFogStartDistance;

		public float unityFogEndDistance = 1000f;

		[GradientUsage(true)]
		public Gradient unityFogColor = new Gradient();
	}
}
