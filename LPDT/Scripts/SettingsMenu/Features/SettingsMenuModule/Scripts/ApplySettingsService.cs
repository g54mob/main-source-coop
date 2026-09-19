using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	public class ApplySettingsService : IApplySettingsService
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly ISettingsService _settingsService;

		private readonly LanguagesLocalizationConfiguration _languageLocalizationConfiguration;

		public ApplySettingsService(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel, ISettingsService settingsService, LanguagesLocalizationConfiguration languageLocalizationConfiguration)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
			_settingsService = settingsService;
			_languageLocalizationConfiguration = languageLocalizationConfiguration;
		}

		public void ApplySettings()
		{
			ApplyAllVolume();
			ApplyMusicVolume();
			ApplyEffectsVolume();
			ApplyMicrophoneDevice();
			ApplyMicrophoneSensitivity();
			ApplyMicrophoneEnabled();
			ApplyNoiseSuppressionEnabled();
			ApplyPushToTalkEnabled();
			ApplyYAxisInvertEnabled();
			ApplyVSyncEnabled();
			ApplyStreamerModeEnabled();
			ApplyQualityLevel();
			ApplyScreenMode();
			ApplyResolution();
			ApplyFpsLimit();
			ApplyScreenShakeIntensity();
			ApplyHeadBobbingIntensity();
			ApplyLanguage();
			ApplyMouseSensitivity();
			ApplyFieldOfView();
			ApplyBrightness();
		}

		public void ApplyMicrophoneEnabled()
		{
			if (_notAppliedSettingsModel.IsMicrophoneEnabledChanged)
			{
				_settingsService.SetEnabledMicrophone(_notAppliedSettingsModel.MicrophoneEnabled);
				_currentSettingsModel.MicrophoneEnabled = _notAppliedSettingsModel.MicrophoneEnabled;
			}
		}

		public void ApplyNoiseSuppressionEnabled()
		{
			if (_notAppliedSettingsModel.IsNoiseSuppressionEnabledChanged)
			{
				_settingsService.SetNoiseSuppression(_notAppliedSettingsModel.NoiseSuppressionEnabled);
				_currentSettingsModel.NoiseSuppressionEnabled = _notAppliedSettingsModel.NoiseSuppressionEnabled;
			}
		}

		public void ApplyPushToTalkEnabled()
		{
			if (_notAppliedSettingsModel.IsPushToTalkEnabledChanged)
			{
				_settingsService.SetPushToTalk(_notAppliedSettingsModel.PushToTalkEnabled);
				_currentSettingsModel.PushToTalkEnabled = _notAppliedSettingsModel.PushToTalkEnabled;
			}
		}

		public void ApplyYAxisInvertEnabled()
		{
			if (_notAppliedSettingsModel.IsYAxisInvertEnabledChanged)
			{
				_settingsService.SetYAxisInvert(_notAppliedSettingsModel.YAxisInvertEnabled);
				_currentSettingsModel.YAxisInvertEnabled = _notAppliedSettingsModel.YAxisInvertEnabled;
			}
		}

		public void ApplyVSyncEnabled()
		{
			if (_notAppliedSettingsModel.IsVSyncEnabledChanged)
			{
				_settingsService.SetEnabledVSync(_notAppliedSettingsModel.VSyncEnabled);
				_currentSettingsModel.VSyncEnabled = _notAppliedSettingsModel.VSyncEnabled;
			}
		}

		public void ApplyStreamerModeEnabled()
		{
			if (_notAppliedSettingsModel.IsStreamerModeEnabledChanged)
			{
				_currentSettingsModel.StreamerModeEnabled = _notAppliedSettingsModel.StreamerModeEnabled;
			}
		}

		public void ApplyMicrophoneSensitivity()
		{
			if (_notAppliedSettingsModel.IsMicrophoneSensitivityChanged)
			{
				_settingsService.SetMicrophoneSensitivity(_notAppliedSettingsModel.MicrophoneSensitivity);
				_currentSettingsModel.MicrophoneSensitivity = _notAppliedSettingsModel.MicrophoneSensitivity;
			}
		}

		public void ApplyMicrophoneDevice()
		{
			if (_notAppliedSettingsModel.IsMicrophoneDeviceChanged)
			{
				_settingsService.SetMicrophone(_notAppliedSettingsModel.MicrophoneDevice);
				_currentSettingsModel.MicrophoneDevice = _notAppliedSettingsModel.MicrophoneDevice;
			}
		}

		public void ApplyEffectsVolume()
		{
			if (_notAppliedSettingsModel.IsEffectsVolumeChanged)
			{
				_settingsService.SetEffectsVolume(_notAppliedSettingsModel.EffectsVolume);
				_currentSettingsModel.EffectsVolume = _notAppliedSettingsModel.EffectsVolume;
			}
		}

		public void ApplyMusicVolume()
		{
			if (_notAppliedSettingsModel.IsMusicVolumeChanged)
			{
				_settingsService.SetMusicVolume(_notAppliedSettingsModel.MusicVolume);
				_currentSettingsModel.MusicVolume = _notAppliedSettingsModel.MusicVolume;
			}
		}

		public void ApplyAllVolume()
		{
			if (_notAppliedSettingsModel.IsAllVolumeChanged)
			{
				_settingsService.SetAllVolume(_notAppliedSettingsModel.AllVolume);
				_currentSettingsModel.AllVolume = _notAppliedSettingsModel.AllVolume;
			}
		}

		public void ApplyBrightness()
		{
			if (_notAppliedSettingsModel.IsBrightnessChanged)
			{
				_settingsService.SetBrightness(_notAppliedSettingsModel.Brightness);
				_currentSettingsModel.Brightness = _notAppliedSettingsModel.Brightness;
			}
		}

		public void ApplyQualityLevel()
		{
			if (_notAppliedSettingsModel.IsQualityLevelChanged)
			{
				_settingsService.SetQualityLevel(_notAppliedSettingsModel.QualityLevel);
				_currentSettingsModel.QualityLevel = _notAppliedSettingsModel.QualityLevel;
			}
		}

		public void ApplyMouseSensitivity()
		{
			if (_notAppliedSettingsModel.IsMouseSensitivityChanged)
			{
				_settingsService.SetMouseSensitivity(_notAppliedSettingsModel.MouseSensitivity);
				_currentSettingsModel.MouseSensitivity = _notAppliedSettingsModel.MouseSensitivity;
			}
		}

		public void ApplyFieldOfView()
		{
			if (_notAppliedSettingsModel.IsFieldOfViewChanged)
			{
				_settingsService.SetFieldOfView(_notAppliedSettingsModel.FieldOfView);
				_currentSettingsModel.FieldOfView = _notAppliedSettingsModel.FieldOfView;
			}
		}

		public void ApplyScreenMode()
		{
			if (_notAppliedSettingsModel.IsScreenModeChanged)
			{
				_settingsService.SetScreenMode((ScreenMode)_notAppliedSettingsModel.ScreenMode);
				_currentSettingsModel.ScreenMode = (ScreenMode)_notAppliedSettingsModel.ScreenMode;
			}
		}

		public void ApplyResolution()
		{
			if (_notAppliedSettingsModel.IsResolutionChanged)
			{
				Vector2Int resolution = _notAppliedSettingsModel.Resolution;
				_settingsService.SetResolution(resolution.x, resolution.y);
				_currentSettingsModel.Resolution = resolution;
			}
		}

		public void ApplyFpsLimit()
		{
			if (_notAppliedSettingsModel.IsFpsLimitChanged)
			{
				_settingsService.SetFrameRateLimit(_notAppliedSettingsModel.FpsLimit);
				_currentSettingsModel.FPSLimit = _notAppliedSettingsModel.FpsLimit;
			}
		}

		public void ApplyScreenShakeIntensity()
		{
			if (_notAppliedSettingsModel.IsScreenShakeIntensityChanged)
			{
				_settingsService.SetScreenShakeIntensity(_notAppliedSettingsModel.ScreenShakeIntensityNormalized);
				_currentSettingsModel.ScreenShakeIntensityNormalized = _notAppliedSettingsModel.ScreenShakeIntensityNormalized;
			}
		}

		public void ApplyHeadBobbingIntensity()
		{
			if (_notAppliedSettingsModel.IsHeadBobbingIntensityChanged)
			{
				_settingsService.SetHeadBobbingIntensity(_notAppliedSettingsModel.HeadBobbingIntensityNormalized);
				_currentSettingsModel.HeadBobbingIntensityNormalized = _notAppliedSettingsModel.HeadBobbingIntensityNormalized;
			}
		}

		public void ApplyLanguage()
		{
			if (_notAppliedSettingsModel.IsLanguageChanged)
			{
				Language languageByIndex = _languageLocalizationConfiguration.GetLanguageByIndex(_notAppliedSettingsModel.Language);
				if (languageByIndex != _currentSettingsModel.Language)
				{
					_settingsService.SetLanguage(languageByIndex);
					_currentSettingsModel.Language = languageByIndex;
				}
			}
		}

		public void ResetAllSettings()
		{
			_notAppliedSettingsModel.Reset();
		}

		public bool HasNotAppliedSettings()
		{
			if ((!_notAppliedSettingsModel.IsAllVolumeChanged || !(Mathf.Abs(_currentSettingsModel.AllVolume - _notAppliedSettingsModel.AllVolume) > 0.0001f)) && (!_notAppliedSettingsModel.IsMusicVolumeChanged || !(Mathf.Abs(_currentSettingsModel.MusicVolume - _notAppliedSettingsModel.MusicVolume) > 0.0001f)) && (!_notAppliedSettingsModel.IsEffectsVolumeChanged || !(Mathf.Abs(_currentSettingsModel.EffectsVolume - _notAppliedSettingsModel.EffectsVolume) > 0.0001f)) && (!_notAppliedSettingsModel.IsMicrophoneSensitivityChanged || !(Mathf.Abs(_currentSettingsModel.MicrophoneSensitivity - _notAppliedSettingsModel.MicrophoneSensitivity) > 0.0001f)) && (!_notAppliedSettingsModel.IsMicrophoneEnabledChanged || _notAppliedSettingsModel.MicrophoneEnabled == _currentSettingsModel.MicrophoneEnabled) && (!_notAppliedSettingsModel.IsNoiseSuppressionEnabledChanged || _notAppliedSettingsModel.NoiseSuppressionEnabled == _currentSettingsModel.NoiseSuppressionEnabled) && (!_notAppliedSettingsModel.IsPushToTalkEnabledChanged || _notAppliedSettingsModel.PushToTalkEnabled == _currentSettingsModel.PushToTalkEnabled) && (!_notAppliedSettingsModel.IsYAxisInvertEnabledChanged || _notAppliedSettingsModel.YAxisInvertEnabled == _currentSettingsModel.YAxisInvertEnabled) && (!_notAppliedSettingsModel.IsMicrophoneDeviceChanged || !(_notAppliedSettingsModel.MicrophoneDevice != _currentSettingsModel.MicrophoneDevice)) && (!_notAppliedSettingsModel.IsVSyncEnabledChanged || _notAppliedSettingsModel.VSyncEnabled == _currentSettingsModel.VSyncEnabled) && (!_notAppliedSettingsModel.IsStreamerModeEnabledChanged || _notAppliedSettingsModel.StreamerModeEnabled == _currentSettingsModel.StreamerModeEnabled) && (!_notAppliedSettingsModel.IsQualityLevelChanged || _notAppliedSettingsModel.QualityLevel == _currentSettingsModel.QualityLevel) && (!_notAppliedSettingsModel.IsFpsLimitChanged || _currentSettingsModel.FPSLimit == _notAppliedSettingsModel.FpsLimit) && (!_notAppliedSettingsModel.IsResolutionChanged || !(_currentSettingsModel.Resolution != _notAppliedSettingsModel.Resolution)) && (!_notAppliedSettingsModel.IsLanguageChanged || _currentSettingsModel.SettingsDataHolder.Language == (int)_languageLocalizationConfiguration.GetLanguageByIndex(_notAppliedSettingsModel.Language)) && (!_notAppliedSettingsModel.IsScreenModeChanged || _currentSettingsModel.ScreenMode == (ScreenMode)_notAppliedSettingsModel.ScreenMode) && (!_notAppliedSettingsModel.IsHeadBobbingIntensityChanged || !(Mathf.Abs(_currentSettingsModel.HeadBobbingIntensityNormalized - _notAppliedSettingsModel.HeadBobbingIntensityNormalized) > 0.0001f)) && (!_notAppliedSettingsModel.IsScreenShakeIntensityChanged || !(Mathf.Abs(_currentSettingsModel.ScreenShakeIntensityNormalized - _notAppliedSettingsModel.ScreenShakeIntensityNormalized) > 0.0001f)) && (!_notAppliedSettingsModel.IsMouseSensitivityChanged || !(Mathf.Abs(_currentSettingsModel.MouseSensitivity - _notAppliedSettingsModel.MouseSensitivity) > 0.0001f)) && (!_notAppliedSettingsModel.IsFieldOfViewChanged || !(Mathf.Abs(_currentSettingsModel.FieldOfView - _notAppliedSettingsModel.FieldOfView) > 0.0001f)))
			{
				if (_notAppliedSettingsModel.IsBrightnessChanged)
				{
					return Mathf.Abs(_currentSettingsModel.Brightness - _notAppliedSettingsModel.Brightness) > 0.0001f;
				}
				return false;
			}
			return true;
		}
	}
}
