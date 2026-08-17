using System;
using System.Collections.Generic;

namespace Enviro
{
	[Serializable]
	public class EnviroWeather
	{
		public List<EnviroWeatherType> weatherTypes = new List<EnviroWeatherType>();

		public float cloudsTransitionSpeed = 1f;

		public float fogTransitionSpeed = 1f;

		public float lightingTransitionSpeed = 1f;

		public float skyTransitionSpeed = 1f;

		public float effectsTransitionSpeed = 1f;

		public float auroraTransitionSpeed = 1f;

		public float environmentTransitionSpeed = 1f;

		public float audioTransitionSpeed = 1f;
	}
}
