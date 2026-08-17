using System;
using System.Linq;
using Enviro;
using EvilCore.Networking;
using NomadDrive.Features.WorldGeneration;
using NomadDrive.Features.WorldGeneration.Utilities;
using UnityEngine;

namespace NomadDrive.Features.GameTime
{
	public class SeededWeatherBridge : MonoBehaviour
	{
		[Tooltip("Weighted pool of weather presets the world deterministically picks one from.")]
		[SerializeField]
		private SeededWeatherConfig config;

		private EnviroManager _enviroManager;

		private bool _applied;

		private void Update()
		{
			if (_applied || config == null)
			{
				return;
			}
			if (_enviroManager == null)
			{
				_enviroManager = EnviroManager.instance;
				if (_enviroManager == null)
				{
					return;
				}
			}
			if (!(_enviroManager.Weather == null) && _enviroManager.Weather.Settings != null && _enviroManager.Weather.Settings.weatherTypes != null && _enviroManager.Weather.Settings.weatherTypes.Count != 0)
			{
				WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
				if (!(instance == null) && instance.IsReady && instance.SeedManager != null)
				{
					ApplySeededWeather(instance);
					_applied = true;
				}
			}
		}

		private void ApplySeededWeather(WorldGenerator worldGenerator)
		{
			SeededWeatherConfig.WeatherChance[] array = config.candidates.Where((SeededWeatherConfig.WeatherChance c) => c != null && c.weight > 0f && _enviroManager.Weather.Settings.weatherTypes.Exists((EnviroWeatherType w) => w != null && w.name == c.weatherTypeName)).ToArray();
			if (array.Length != 0)
			{
				System.Random random = worldGenerator.SeedManager.CreateRandom(config.seedIdentifier);
				SeededWeatherConfig.WeatherChance weatherChance = WeightedRandom.Select(array, (SeededWeatherConfig.WeatherChance c) => c.weight, random);
				_enviroManager.Weather.globalAutoWeatherChange = false;
				_enviroManager.Weather.ChangeWeatherInstant(weatherChance.weatherTypeName);
			}
		}
	}
}
