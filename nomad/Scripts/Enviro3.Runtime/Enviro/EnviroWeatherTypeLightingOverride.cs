using System;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeLightingOverride
	{
		public float directLightIntensityModifier = 1f;

		public float ambientIntensityModifier = 1f;

		public float shadowIntensity = 1f;
	}
}
