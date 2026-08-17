using System;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeCloudsOverride
	{
		public float ambientLightIntensity = 1f;

		public float coverage;

		public float dilateCoverage = 0.5f;

		public float dilateType = 0.5f;

		public float typeModifier = 0.5f;

		public float cloudTypeShaping = 1f;

		public float scatteringIntensity = 1f;

		public float multiScatterStrength = 0.5f;

		public float multiScatterFalloff = 0.02f;

		public float ambientFloor = 0.02f;

		public float lightningIntensity = 0.5f;

		public float exposure = 1f;

		public float silverLiningSpread = 0.3f;

		public float silverLiningIntensity = 1f;

		public float edgeHighlightStrength = 1f;

		public float ligthAbsorbtion = 0.5f;

		public float density = 1f;

		public float densitySmoothness = 1f;

		public float baseErosionIntensity = 0.4f;

		public float baseNoiseMultiplier = 1f;

		public float detailErosionIntensity = 0.25f;

		public float detailNoiseMultiplier = 1f;

		public float curlIntensity = 0.2f;

		public float bottomShape;

		public float midShape;

		public float topShape;

		public float topLayer;

		public float rampShape = 1f;

		public float baseNoiseUVMultiplier = 1f;

		public float detailNoiseUVMultiplier = 1f;
	}
}
