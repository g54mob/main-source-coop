using System;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeEnvironmentOverride
	{
		public float temperatureWeatherMod;

		public float wetnessTarget;

		public float snowTarget;

		public float windDirectionX = 1f;

		public float windDirectionY = -1f;

		public float windSpeed = 0.25f;

		public float windTurbulence = 0.25f;
	}
}
