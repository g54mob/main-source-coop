using System;

namespace Enviro
{
	[Serializable]
	public class EnviroZoneWeather
	{
		public bool showEditor;

		public EnviroWeatherType weatherType;

		public float probability = 50f;

		public bool seasonalProbability;

		public float probabilitySpring = 50f;

		public float probabilitySummer = 50f;

		public float probabilityAutumn = 50f;

		public float probabilityWinter = 50f;
	}
}
