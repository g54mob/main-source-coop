using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.LevelModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.WeatherModule.Scripts
{
	[Serializable]
	public class BeachWeatherBehaviour : BeachBehaviour, IBeachRenderStateEnforcer
	{
		[Tooltip("Weather presets available to this beach preset.")]
		[SerializeField]
		private List<WeatherPreset> _weatherPresetPool = new List<WeatherPreset>();

		[Tooltip("Apply one randomly selected weather preset from the pool instead of applying the whole pool.")]
		[SerializeField]
		private bool _randomizeWeather;

		[Tooltip("Allow the same weather preset to appear more than once in the pool.")]
		[SerializeField]
		private bool _allowDuplicateWeather = true;

		private DiContainer _diContainer;

		private WeatherModel _weatherModel;

		private LevelModel _levelModel;

		private WeatherSelectionModel _weatherSelectionModel;

		private BeachPresetRuntimeContext _activeContext;

		private WeatherPreset _appliedWeatherPreset;

		private Material _enforcedSkybox;

		[Inject]
		public void InjectDependencies(DiContainer diContainer, WeatherModel weatherModel, LevelModel levelModel, WeatherSelectionModel weatherSelectionModel)
		{
			_diContainer = diContainer;
			_weatherModel = weatherModel;
			_levelModel = levelModel;
			_weatherSelectionModel = weatherSelectionModel;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			_appliedWeatherPreset = null;
			_enforcedSkybox = null;
			_activeContext = context;
			_weatherSelectionModel.Selection.Changed -= OnSelectionChanged;
			_weatherSelectionModel.Selection.Changed += OnSelectionChanged;
			_weatherSelectionModel.AttachmentChanged -= AttachmentChanged;
			_weatherSelectionModel.AttachmentChanged += AttachmentChanged;
			_levelModel.OnCurrentLevelChanged -= OnCurrentLevelChanged;
			_levelModel.OnCurrentLevelChanged += OnCurrentLevelChanged;
			ApplySelection();
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			_weatherSelectionModel.Selection.Changed -= OnSelectionChanged;
			_weatherSelectionModel.AttachmentChanged -= AttachmentChanged;
			_levelModel.OnCurrentLevelChanged -= OnCurrentLevelChanged;
			if (_appliedWeatherPreset != null)
			{
				_appliedWeatherPreset.Clear(context);
			}
			_appliedWeatherPreset = null;
			_enforcedSkybox = null;
			_activeContext = null;
		}

		public void EnforceRenderState()
		{
			if (_activeContext != null && !(_appliedWeatherPreset == null) && !(RenderSettings.skybox == _enforcedSkybox))
			{
				ApplyAppliedPreset();
			}
		}

		public int PickIndex(WeatherModel weatherModel)
		{
			int num = SelectWeatherIndex(weatherModel);
			if (!_allowDuplicateWeather && num >= 0 && num < _weatherPresetPool.Count)
			{
				weatherModel.LastUsedWeather = _weatherPresetPool[num];
			}
			return num;
		}

		private void OnSelectionChanged(long _)
		{
			ApplySelection();
		}

		private void AttachmentChanged(bool _)
		{
			ApplySelection();
		}

		private void OnCurrentLevelChanged(LevelType _)
		{
			ApplySelection();
		}

		private void ApplySelection()
		{
			if (_activeContext == null)
			{
				return;
			}
			long value = _weatherSelectionModel.Selection.Value;
			if (WeatherSelectionModel.DecodeLevel(value) != _levelModel.CurrentLevel)
			{
				return;
			}
			int num = WeatherSelectionModel.DecodeIndex(value);
			if (num < 0 || num >= _weatherPresetPool.Count)
			{
				return;
			}
			WeatherPreset weatherPreset = _weatherPresetPool[num];
			if (!(weatherPreset == _appliedWeatherPreset))
			{
				if (_appliedWeatherPreset != null)
				{
					_appliedWeatherPreset.Clear(_activeContext);
				}
				_appliedWeatherPreset = weatherPreset;
				ApplyAppliedPreset();
			}
		}

		private void ApplyAppliedPreset()
		{
			_appliedWeatherPreset.Apply(_activeContext, _diContainer);
			_enforcedSkybox = RenderSettings.skybox;
			_weatherModel.DebugUsedWeather = _appliedWeatherPreset;
		}

		private int SelectWeatherIndex(WeatherModel weatherModel)
		{
			if (_weatherPresetPool.Count == 0)
			{
				return -1;
			}
			if (!_randomizeWeather)
			{
				return 0;
			}
			if (_weatherPresetPool.Count <= 1 && _weatherPresetPool.Contains(weatherModel.LastUsedWeather))
			{
				Debug.LogError($"Applying duplicate weather {_weatherPresetPool[0]} because pool is exceeded");
				return 0;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < _weatherPresetPool.Count; i++)
			{
				if (_weatherPresetPool[i] != weatherModel.LastUsedWeather)
				{
					list.Add(i);
				}
			}
			return list[weatherModel.Random.Next(0, list.Count)];
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_weatherPresetPool == null || _weatherPresetPool.Count == 0)
			{
				beachValidationResult.AddError("BeachWeatherBehaviour weather preset pool cannot be empty.");
				return beachValidationResult;
			}
			HashSet<WeatherPreset> hashSet = new HashSet<WeatherPreset>();
			for (int i = 0; i < _weatherPresetPool.Count; i++)
			{
				WeatherPreset weatherPreset = _weatherPresetPool[i];
				if (weatherPreset == null)
				{
					beachValidationResult.AddError($"Weather preset pool entry #{i + 1} is empty.");
				}
				else if (!_allowDuplicateWeather && !hashSet.Add(weatherPreset))
				{
					beachValidationResult.AddError("Weather preset '" + weatherPreset.name + "' is duplicated but duplicate weather is disabled.");
				}
				else
				{
					ValidateWeatherPreset(weatherPreset, beachValidationResult);
				}
			}
			return beachValidationResult;
		}

		private static void ValidateWeatherPreset(WeatherPreset weatherPreset, BeachValidationResult result)
		{
			result.Merge(weatherPreset.ValidateAll());
		}
	}
}
