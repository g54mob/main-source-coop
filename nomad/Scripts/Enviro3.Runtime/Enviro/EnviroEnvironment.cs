using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroEnvironment
	{
		public enum Seasons
		{
			Spring = 0,
			Summer = 1,
			Autumn = 2,
			Winter = 3
		}

		public Seasons season;

		public bool changeSeason = true;

		[Tooltip("Start Day of Year for Spring")]
		[Range(0f, 366f)]
		public int springStart = 60;

		[Tooltip("End Day of Year for Spring")]
		[Range(0f, 366f)]
		public int springEnd = 92;

		[Tooltip("Start Day of Year for Summer")]
		[Range(0f, 366f)]
		public int summerStart = 93;

		[Tooltip("End Day of Year for Summer")]
		[Range(0f, 366f)]
		public int summerEnd = 185;

		[Tooltip("Start Day of Year for Autumn")]
		[Range(0f, 366f)]
		public int autumnStart = 186;

		[Tooltip("End Day of Year for Autumn")]
		[Range(0f, 366f)]
		public int autumnEnd = 276;

		[Tooltip("Start Day of Year for Winter")]
		[Range(0f, 366f)]
		public int winterStart = 277;

		[Tooltip("End Day of Year for Winter")]
		[Range(0f, 366f)]
		public int winterEnd = 59;

		[Tooltip("Base Temperature in Spring")]
		public AnimationCurve springBaseTemperature = new AnimationCurve();

		[Tooltip("Base Temperature in Summer")]
		public AnimationCurve summerBaseTemperature = new AnimationCurve();

		[Tooltip("Base Temperature in Autumn")]
		public AnimationCurve autumnBaseTemperature = new AnimationCurve();

		[Tooltip("Base Temperature in Winter")]
		public AnimationCurve winterBaseTemperature = new AnimationCurve();

		[Tooltip("Current temperature.")]
		[Range(-50f, 50f)]
		public float temperature;

		[Tooltip("Temperature mod used for different weather types.")]
		[Range(-50f, 50f)]
		public float temperatureWeatherMod;

		[Tooltip("Custom temperature mod for gameplay use.")]
		[Range(-50f, 50f)]
		public float temperatureCustomMod;

		[Tooltip("Temperature changing speed.")]
		public float temperatureChangingSpeed = 1f;

		[Tooltip("Current wetness for third party shader or gameplay.")]
		[Range(0f, 1f)]
		public float wetness;

		[Tooltip("Target wetness for third party shader or gameplay.")]
		[Range(0f, 1f)]
		public float wetnessTarget;

		[Tooltip("Current snow for third party shader or gameplay.")]
		[Range(0f, 1f)]
		public float snow;

		[Tooltip("Target snow for third party shader or gameplay.")]
		[Range(0f, 1f)]
		public float snowTarget;

		[Tooltip("Speed of wetness accumulation.")]
		public float wetnessAccumulationSpeed = 1f;

		[Tooltip("Speed of wetness dries.")]
		public float wetnessDrySpeed = 1f;

		[Tooltip("Speed of snow buildup.")]
		public float snowAccumulationSpeed = 1f;

		[Tooltip("Speed of how fast snow melts.")]
		public float snowMeltSpeed = 1f;

		[Tooltip("Temperature when snow starts to melt.")]
		[Range(-20f, 20f)]
		public float snowMeltingTresholdTemperature = 1f;

		[Range(-1f, 1f)]
		public float windDirectionX;

		[Range(-1f, 1f)]
		public float windDirectionY;

		[Range(0f, 1f)]
		public float windSpeed = 0.1f;

		[Range(0f, 1f)]
		public float windTurbulence = 0.1f;
	}
}
