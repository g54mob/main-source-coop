using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroCloudLayerSettings
	{
		[Range(-1f, 1f)]
		public float cloudsWindDirectionXModifier = 1f;

		[Range(-1f, 1f)]
		public float cloudsWindDirectionYModifier = 1f;

		[Range(-0.1f, 0.1f)]
		public float windSpeedModifier = 0.1f;

		[Range(0f, 0.1f)]
		public float windUpwards = 0.1f;

		[Range(-1f, 1f)]
		public float coverage = 1f;

		public float worleyFreq2 = 12f;

		public float worleyFreq1 = 48f;

		[Range(0f, 1f)]
		public float dilateCoverage = 0.5f;

		[Range(0f, 1f)]
		public float dilateType = 0.5f;

		[Range(0f, 1f)]
		public float cloudsTypeModifier = 0.5f;

		public Vector2 locationOffset;

		public float bottomCloudsHeight = 2000f;

		public float topCloudsHeight = 8000f;

		[Range(0f, 2f)]
		public float density = 1f;

		[Range(0f, 2f)]
		public float densitySmoothness = 1f;

		[Range(0f, 2f)]
		public float scatteringIntensity = 3f;

		[Range(0f, 1f)]
		public float edgeHighlightStrength = 0.5f;

		[Range(0f, 1f)]
		public float silverLiningSpread = 0.15f;

		[Range(0f, 2f)]
		public float silverLiningIntensity = 1f;

		[Range(0f, 2f)]
		public float lightningIntensity = 0.5f;

		[Range(0f, 1f)]
		public float curlIntensity = 1f;

		[Range(0f, 0.25f)]
		public float lightStepModifier = 0.1f;

		[Range(0f, 1f)]
		public float multiScatterStrength = 0.5f;

		[Range(0f, 1f)]
		public float multiScatterFalloff = 0.02f;

		[Range(0f, 0.5f)]
		public float ambientFloor = 0.02f;

		[Range(0f, 0.5f)]
		public float absorbtion = 0.25f;

		[Range(0f, 2f)]
		public float exposure = 1f;

		public float baseNoiseUV = 32f;

		public float detailNoiseUV = 32f;

		[Range(0f, 2f)]
		public float baseNoiseUVMultiplier = 1f;

		[Range(0f, 2f)]
		public float detailNoiseUVMultiplier = 1f;

		[Range(0f, 1f)]
		public float baseErosionIntensity;

		[Range(0f, 1f)]
		public float baseNoiseMultiplier = 1f;

		[Range(0f, 1f)]
		public float detailErosionIntensity = 0.2f;

		[Range(0f, 1f)]
		public float detailNoiseMultiplier = 1f;

		[Range(-1f, 1f)]
		public float bottomShape;

		public float midShape;

		[Range(-1f, 1f)]
		public float topShape;

		[Range(-1f, 1f)]
		public float topLayer;

		[Range(0f, 1f)]
		public float cloudTypeShaping = 1f;

		[Range(0f, 2f)]
		public float rampShape = 0.5f;
	}
}
