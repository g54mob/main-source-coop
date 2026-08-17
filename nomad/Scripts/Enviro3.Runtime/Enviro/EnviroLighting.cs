using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	[Serializable]
	public class EnviroLighting
	{
		public enum LightingMode
		{
			Single = 0,
			Dual = 1
		}

		public LightingMode lightingMode;

		public bool setDirectLighting = true;

		public int updateIntervallFrames = 2;

		public AnimationCurve sunIntensityCurve;

		public AnimationCurve moonIntensityCurve;

		[GradientUsage(true)]
		public Gradient sunColorGradient;

		[GradientUsage(true)]
		public Gradient moonColorGradient;

		public AnimationCurve sunIntensityCurveHDRP = new AnimationCurve();

		public AnimationCurve moonIntensityCurveHDRP = new AnimationCurve();

		public AnimationCurve lightColorTemperatureHDRP = new AnimationCurve();

		[GradientUsage(true)]
		public Gradient ambientColorTintHDRP;

		public float lightIntensityHDRP = 750f;

		public bool controlExposure = true;

		public AnimationCurve sceneExposure = new AnimationCurve();

		public bool controlIndirectLighting = true;

		public AnimationCurve diffuseIndirectIntensity = new AnimationCurve();

		public AnimationCurve reflectionIndirectIntensity = new AnimationCurve();

		[Range(0f, 2f)]
		public float directLightIntensityModifier = 1f;

		public bool setAmbientLighting = true;

		public AmbientMode ambientMode;

		[GradientUsage(true)]
		public Gradient ambientSkyColorGradient;

		[GradientUsage(true)]
		public Gradient ambientEquatorColorGradient;

		[GradientUsage(true)]
		public Gradient ambientGroundColorGradient;

		public AnimationCurve ambientIntensityCurve;

		[Range(0f, 2f)]
		public float ambientIntensityModifier = 1f;

		public bool ambientUpdateEveryFrame;

		[Range(0f, 2f)]
		public float ambientUpdateIntervall = 0.1f;

		[Range(0f, 1f)]
		public float shadowIntensity = 1f;
	}
}
