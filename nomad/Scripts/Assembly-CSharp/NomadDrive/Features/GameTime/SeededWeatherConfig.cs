using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.GameTime
{
	[CreateAssetMenu(menuName = "NomadDrive/GameTime/Seeded Weather Config", fileName = "SeededWeatherConfig")]
	public class SeededWeatherConfig : ScriptableObject
	{
		[Serializable]
		public class WeatherChance
		{
			[Tooltip("Enviro EnviroWeatherType asset name — must match EXACTLY. E.g. \"Clear Sky\", \"Cloudy 1\", \"Foggy\".")]
			public string weatherTypeName;

			[Tooltip("Relative weight. Higher = more likely. Entries with weight 0 are ignored.")]
			[Min(0f)]
			public float weight = 1f;

			[Tooltip("Optional time-of-day bias (DynamicWeatherDirector only). X = in-game hour (0–24), Y = weight multiplier (>= 0). Default flat 1 = no time effect. E.g. a curve peaking around 5–7 makes this weather likelier at dawn.")]
			public AnimationCurve hourlyWeight = AnimationCurve.Constant(0f, 24f, 1f);
		}

		[Tooltip("Weighted pool of weather presets. The world deterministically picks one at world-gen time.")]
		public List<WeatherChance> candidates = new List<WeatherChance>
		{
			new WeatherChance
			{
				weatherTypeName = "Clear Sky",
				weight = 3f
			},
			new WeatherChance
			{
				weatherTypeName = "Cloudy 1",
				weight = 2f
			},
			new WeatherChance
			{
				weatherTypeName = "Foggy",
				weight = 1f
			}
		};

		[Tooltip("Sub-seed identifier passed to SeedManager.CreateRandom. Keep stable — it defines the draw.")]
		public string seedIdentifier = "InitialWeather";

		[Header("Dynamic Cycling")]
		[Tooltip("ON: DynamicWeatherDirector cycles weather over time between candidates. OFF: one seeded pick that never changes (legacy static behaviour).")]
		public bool enableDynamicCycling = true;

		[Tooltip("Don't pick the same weather two segments in a row, so each interval is a visible change.")]
		public bool avoidImmediateRepeat = true;

		[Tooltip("Minimum length of one weather segment, in in-game minutes (60 = 1 in-game hour).")]
		[Min(1f)]
		public int minSegmentMinutes = 60;

		[Tooltip("Maximum length of one weather segment, in in-game minutes (180 = 3 in-game hours).")]
		[Min(1f)]
		public int maxSegmentMinutes = 180;

		[Tooltip("Blend smoothness — DynamicWeatherDirector pushes this into all of Enviro's weather transition-speed channels at startup. LOWER = slower/smoother (0.1 ≈ ~30s, 0.05 ≈ ~60s; Enviro default 1 ≈ ~3s and reads as sharp). Set ≤ 0 to leave Enviro's own values untouched.")]
		public float weatherTransitionSpeed = 0.1f;
	}
}
