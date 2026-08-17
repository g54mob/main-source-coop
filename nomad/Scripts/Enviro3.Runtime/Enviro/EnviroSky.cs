using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	[Serializable]
	public class EnviroSky
	{
		public enum SkyMode
		{
			Normal = 0,
			Simple = 1
		}

		public enum MoonMode
		{
			Realistic = 0,
			Simple = 1,
			Off = 2
		}

		public SkyMode skyMode;

		public MoonMode moonMode;

		public bool forcedSkyboxSetup = true;

		[GradientUsage(true)]
		public Gradient frontColorGradient0;

		[GradientUsage(true)]
		public Gradient frontColorGradient1;

		[GradientUsage(true)]
		public Gradient frontColorGradient2;

		[GradientUsage(true)]
		public Gradient frontColorGradient3;

		[GradientUsage(true)]
		public Gradient frontColorGradient4;

		[GradientUsage(true)]
		public Gradient frontColorGradient5;

		[GradientUsage(true)]
		public Gradient backColorGradient0;

		[GradientUsage(true)]
		public Gradient backColorGradient1;

		[GradientUsage(true)]
		public Gradient backColorGradient2;

		[GradientUsage(true)]
		public Gradient backColorGradient3;

		[GradientUsage(true)]
		public Gradient backColorGradient4;

		[GradientUsage(true)]
		public Gradient backColorGradient5;

		[GradientUsage(true)]
		public Gradient sunDiscColorGradient;

		[GradientUsage(true)]
		public Gradient moonColorGradient;

		[GradientUsage(true)]
		public Gradient moonGlowColorGradient;

		public Cubemap starsTex;

		public Cubemap starsTwinklingTex;

		public Cubemap galaxyTex;

		public Texture2D sunTex;

		public Texture2D moonTex;

		public Texture2D moonGlowTex;

		[Range(-0.1f, 1f)]
		public float distribution0;

		[Range(-0.1f, 1f)]
		public float distribution1;

		[Range(-0.1f, 1f)]
		public float distribution2;

		[Range(-0.1f, 1f)]
		public float distribution3;

		public AnimationCurve mieScatteringIntensityCurve;

		public AnimationCurve moonGlowIntensityCurve;

		public AnimationCurve starIntensityCurve;

		public AnimationCurve galaxyIntensityCurve;

		public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		public float intensity;

		public float sunScale;

		public float moonScale;

		[Range(0f, 2f)]
		public float mieScatteringMultiplier = 1f;

		[Range(0f, 1f)]
		public float starsTwinklingSpeed = 0.1f;

		[Range(-2f, 2f)]
		public float moonPhase;

		public AnimationCurve skyExposureHDRP;

		public SkyAmbientMode skyAmbientModeHDRP = SkyAmbientMode.Dynamic;

		[ColorUsage(false, true)]
		public Color skyColorTint = Color.white;

		[Range(0f, 2f)]
		public float skyColorExponent = 1f;
	}
}
