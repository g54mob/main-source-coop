using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroFlatClouds
	{
		public bool useCirrusClouds = true;

		public Texture2D cirrusCloudsTex;

		[Range(0f, 1f)]
		public float cirrusCloudsAlpha;

		[Range(0f, 2f)]
		public float cirrusCloudsColorPower;

		[Range(0f, 1f)]
		public float cirrusCloudsCoverage;

		[GradientUsage(true)]
		public Gradient cirrusCloudsColor;

		[Range(0f, 1f)]
		public float cirrusCloudsWindIntensity = 0.5f;

		public bool useFlatClouds = true;

		public Texture2D flatCloudsBaseTex;

		public Texture2D flatCloudsDetailTex;

		[GradientUsage(true)]
		public Gradient flatCloudsLightColor;

		[GradientUsage(true)]
		public Gradient flatCloudsAmbientColor;

		[Range(0f, 2f)]
		public float flatCloudsLightIntensity = 1f;

		[Range(0f, 2f)]
		public float flatCloudsAmbientIntensity = 1f;

		[Range(0f, 2f)]
		public float flatCloudsShadowIntensity = 0.6f;

		[Range(1f, 12f)]
		public float flatCloudsShadowSteps = 8f;

		[Range(0f, 1f)]
		public float flatCloudsHGPhase = 0.6f;

		[Range(0f, 2f)]
		public float flatCloudsCoverage = 1f;

		[Range(0f, 2f)]
		public float flatCloudsDensity = 1f;

		public float flatCloudsAltitude = 10f;

		public bool flatCloudsTonemapping;

		public float flatCloudsBaseTiling = 4f;

		public float flatCloudsDetailTiling = 10f;

		[Range(0f, 1f)]
		public float flatCloudsWindIntensity = 0.2f;

		[Range(0f, 1f)]
		public float flatCloudsDetailWindIntensity = 0.5f;
	}
}
