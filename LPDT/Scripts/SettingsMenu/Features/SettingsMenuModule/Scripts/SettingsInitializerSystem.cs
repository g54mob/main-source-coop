using System;
using System.Collections.Generic;
using System.Linq;
using Features.AudioDevicesModule.Scripts;
using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.SettingsMenuModule.Scripts.Data;
using Features.SettingsMenuModule.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsInitializerSystem : IInitializable, IDisposable
	{
		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly ISettingsService _settingsService;

		private readonly InitialSettingsConfiguration _initialSettingsConfiguration;

		private readonly IPerformanceDetectionService _performanceDetectionService;

		private readonly QualitySettingsOptionsConfiguration _qualitySettingsOptionsConfiguration;

		private readonly MicrophoneModel _microphoneModel;

		private readonly IDeviceService _deviceService;

		private readonly ISavingService _savingService;

		public SettingsInitializerSystem(CurrentSettingsModel currentSettingsModel, ISettingsService settingsService, InitialSettingsConfiguration initialSettingsConfiguration, IPerformanceDetectionService performanceDetectionService, QualitySettingsOptionsConfiguration qualitySettingsOptionsConfiguration, MicrophoneModel microphoneModel, IDeviceService deviceService, ISavingService savingService)
		{
			_currentSettingsModel = currentSettingsModel;
			_settingsService = settingsService;
			_initialSettingsConfiguration = initialSettingsConfiguration;
			_performanceDetectionService = performanceDetectionService;
			_qualitySettingsOptionsConfiguration = qualitySettingsOptionsConfiguration;
			_microphoneModel = microphoneModel;
			_deviceService = deviceService;
			_savingService = savingService;
		}

		public void Initialize()
		{
			_savingService.LoadDataForGroup(SavingGroup.Settings);
			if (!_currentSettingsModel.HasSavedSettings)
			{
				SetInitialSettings();
			}
			SetSettings();
			_microphoneModel.OnRecorderRegistered += SetSettings;
		}

		public void Dispose()
		{
			_microphoneModel.OnRecorderRegistered -= SetSettings;
		}

		private void SetInitialSettings()
		{
			SettingsDataHolder settingsDataHolder = _currentSettingsModel.SettingsDataHolder;
			settingsDataHolder.AllVolume = _initialSettingsConfiguration.AllVolume;
			settingsDataHolder.MusicVolume = _initialSettingsConfiguration.MusicVolume;
			settingsDataHolder.EffectsVolume = _initialSettingsConfiguration.EffectsVolume;
			settingsDataHolder.MicrophoneSensitivity = _initialSettingsConfiguration.MicrophoneSensitivity;
			settingsDataHolder.MicrophoneEnabled = _initialSettingsConfiguration.MicrophoneEnabled;
			settingsDataHolder.NoiseSuppressionEnabled = _initialSettingsConfiguration.NoiseSuppressionEnabled;
			settingsDataHolder.PushToTalkEnabled = _initialSettingsConfiguration.PushToTalkEnabled;
			settingsDataHolder.YAxisInvertEnabled = _initialSettingsConfiguration.YAxisInvertEnabled;
			settingsDataHolder.VSyncEnabled = _initialSettingsConfiguration.VSyncEnabled;
			PerformanceLevel performanceLevel = _performanceDetectionService.DeterminePerformanceLevel();
			Features.DeviceModule.Scripts.DeviceData.DeviceType currentDevice = _deviceService.GetCurrentDevice();
			settingsDataHolder.QualityLevel = _qualitySettingsOptionsConfiguration.GetQualitySettingByPerformanceLevel(performanceLevel, currentDevice)?.QualityOption ?? _initialSettingsConfiguration.QualityLevel;
			settingsDataHolder.MouseSensitivity = _initialSettingsConfiguration.MouseSensitivity;
			settingsDataHolder.FieldOfView = _initialSettingsConfiguration.FieldOfView;
			settingsDataHolder.Brightness = _initialSettingsConfiguration.Brightness;
			settingsDataHolder.ScreenMode = (int)_initialSettingsConfiguration.ScreenMode;
			settingsDataHolder.Resolution = GetInitialResolution();
			settingsDataHolder.FPSLimit = _qualitySettingsOptionsConfiguration.AllFpsLimits[Mathf.Clamp(_initialSettingsConfiguration.FPSLimitLevel, 0, _qualitySettingsOptionsConfiguration.AllFpsLimits.Count - 1)];
			settingsDataHolder.ScreenShakeIntensityNormalized = _initialSettingsConfiguration.ScreenShakeIntensityNormalized;
			settingsDataHolder.HeadBobbingIntensityNormalized = _initialSettingsConfiguration.HeadBobbingIntensityNormalized;
			settingsDataHolder.Language = (int)_initialSettingsConfiguration.Language;
		}

		private Vector2Int GetInitialResolution()
		{
			List<Vector2Int> list = Screen.resolutions.Select((Resolution res) => new Vector2Int(res.width, res.height)).ToList();
			Vector2Int vector2Int = new Vector2Int(Screen.width, Screen.height);
			if (list.Contains(vector2Int))
			{
				return vector2Int;
			}
			if (list.Count > 0)
			{
				return list[list.Count - 1];
			}
			return vector2Int;
		}

		private void SetSettings()
		{
			_settingsService.SetAllVolume(_currentSettingsModel.AllVolume);
			_settingsService.SetMusicVolume(_currentSettingsModel.MusicVolume);
			_settingsService.SetEffectsVolume(_currentSettingsModel.EffectsVolume);
			_settingsService.SetMicrophone(_currentSettingsModel.MicrophoneDevice);
			_settingsService.SetMicrophoneSensitivity(_currentSettingsModel.MicrophoneSensitivity);
			_settingsService.SetEnabledMicrophone(_currentSettingsModel.MicrophoneEnabled);
			_settingsService.SetNoiseSuppression(_currentSettingsModel.NoiseSuppressionEnabled);
			_settingsService.SetPushToTalk(_currentSettingsModel.PushToTalkEnabled);
			_settingsService.SetYAxisInvert(_currentSettingsModel.YAxisInvertEnabled);
			_settingsService.SetEnabledVSync(_currentSettingsModel.VSyncEnabled);
			_settingsService.SetQualityLevel(_currentSettingsModel.QualityLevel);
			_settingsService.SetMouseSensitivity(_currentSettingsModel.MouseSensitivity);
			_settingsService.SetFieldOfView(_currentSettingsModel.FieldOfView);
			_settingsService.SetBrightness(_currentSettingsModel.Brightness);
			_settingsService.SetScreenMode(_currentSettingsModel.ScreenMode);
			_settingsService.SetResolution(_currentSettingsModel.Resolution.x, _currentSettingsModel.Resolution.y);
			_settingsService.SetFrameRateLimit(_currentSettingsModel.FPSLimit);
			_settingsService.SetScreenShakeIntensity(_currentSettingsModel.ScreenShakeIntensityNormalized);
			_settingsService.SetHeadBobbingIntensity(_currentSettingsModel.HeadBobbingIntensityNormalized);
			_settingsService.SetLanguage(_currentSettingsModel.Language);
		}
	}
}
