using System;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeFlatCloudsOverride
	{
		public float cirrusCloudsAlpha = 0.5f;

		public float cirrusCloudsCoverage = 0.5f;

		public float cirrusCloudsColorPower = 1f;

		public float flatCloudsCoverage = 1f;

		public float flatCloudsDensity = 1f;

		public float flatCloudsLightIntensity = 1f;

		public float flatCloudsAmbientIntensity = 1f;

		public float flatCloudsShadowIntensity = 0.6f;

		public int flatCloudsShadowSteps = 8;
	}
}
