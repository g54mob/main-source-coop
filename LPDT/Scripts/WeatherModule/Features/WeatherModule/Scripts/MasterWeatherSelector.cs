using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Configurations;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.WeatherModule.Scripts
{
	public sealed class MasterWeatherSelector : IMasterWeatherSelector
	{
		private readonly LevelModel _levelModel;

		private readonly WeatherModel _weatherModel;

		private readonly WeatherSelectionModel _weatherSelectionModel;

		private readonly BeachByLevelsPresetConfiguration _beachByLevelsPresetConfiguration;

		public MasterWeatherSelector(LevelModel levelModel, WeatherModel weatherModel, WeatherSelectionModel weatherSelectionModel, BeachByLevelsPresetConfiguration beachByLevelsPresetConfiguration)
		{
			_levelModel = levelModel;
			_weatherModel = weatherModel;
			_weatherSelectionModel = weatherSelectionModel;
			_beachByLevelsPresetConfiguration = beachByLevelsPresetConfiguration;
		}

		public void SelectForCurrentLevel()
		{
			if (!_weatherSelectionModel.IsAttached || !_weatherSelectionModel.IsAuthority)
			{
				return;
			}
			LevelType currentLevel = _levelModel.CurrentLevel;
			if (WeatherSelectionModel.DecodeLevel(_weatherSelectionModel.Selection.Value) == currentLevel || !_beachByLevelsPresetConfiguration.PresetsByLevel.TryGetValue(currentLevel, out var value) || value == null)
			{
				return;
			}
			BeachWeatherBehaviour beachWeatherBehaviour = FindWeatherBehaviour(value);
			if (beachWeatherBehaviour != null)
			{
				int num = beachWeatherBehaviour.PickIndex(_weatherModel);
				if (num >= 0)
				{
					_weatherSelectionModel.Selection.Value = WeatherSelectionModel.Encode(currentLevel, num);
					Debug.Log($"[Weather] Host selected weather index {num} for {currentLevel}.");
				}
			}
		}

		private static BeachWeatherBehaviour FindWeatherBehaviour(BeachPreset preset)
		{
			foreach (BeachBehaviour item in preset.GetEnabledBehavioursInApplyOrder())
			{
				if (item is BeachWeatherBehaviour result)
				{
					return result;
				}
			}
			return null;
		}
	}
}
