using Features.AudioDevicesModule.Scripts;
using Features.AudioVolumeModule.Scripts;
using Features.BrightnessModule.Scripts;
using Features.CameraModelModule;
using Features.CameraModuleRotation.Scripts;
using Features.ScreenShakeModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Features.SettingsMenuModule.Scripts.Services;
using Features.VSyncServiceModule.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsService : ISettingsService
	{
		private readonly IMicrophoneService _microphoneService;

		private readonly IVsyncService _vsyncService;

		private readonly IQualitySettingsService _qualitySettingsService;

		private readonly CameraModel _cameraModel;

		private readonly FieldOfViewModel _fieldOfViewModel;

		private readonly BrightnessModel _brightnessModel;

		private readonly MicrophoneModel _microphoneModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly IFmodAudioVolumeService _fmodAudioVolumeService;

		private readonly CameraLocalScreenShakeModel _cameraLocalScreenShakeModel;

		private readonly HeadBobbingModel _headBobbingModel;

		private readonly ILanguageService _languageService;

		public SettingsService(IMicrophoneService microphoneService, IVsyncService vsyncService, IQualitySettingsService qualitySettingsService, CameraModel cameraModel, FieldOfViewModel fieldOfViewModel, BrightnessModel brightnessModel, MicrophoneModel microphoneModel, CurrentSettingsModel currentSettingsModel, IFmodAudioVolumeService fmodAudioVolumeService, CameraLocalScreenShakeModel cameraLocalScreenShakeModel, HeadBobbingModel headBobbingModel, ILanguageService languageService)
		{
			_microphoneService = microphoneService;
			_vsyncService = vsyncService;
			_qualitySettingsService = qualitySettingsService;
			_cameraModel = cameraModel;
			_fieldOfViewModel = fieldOfViewModel;
			_brightnessModel = brightnessModel;
			_microphoneModel = microphoneModel;
			_currentSettingsModel = currentSettingsModel;
			_fmodAudioVolumeService = fmodAudioVolumeService;
			_cameraLocalScreenShakeModel = cameraLocalScreenShakeModel;
			_headBobbingModel = headBobbingModel;
			_languageService = languageService;
		}

		public void SetAllVolume(float value)
		{
			_fmodAudioVolumeService.SetAllBusValue(value);
		}

		public void SetMusicVolume(float value)
		{
			_fmodAudioVolumeService.SetMusicValue(value);
		}

		public void SetEffectsVolume(float value)
		{
			_fmodAudioVolumeService.SetEffectValue(value);
		}

		public void SetMicrophone(string micName)
		{
			_microphoneModel.SetMicrophoneName(micName);
		}

		public void SetMicrophoneSensitivity(float value)
		{
			_microphoneService.ChangeMicrophoneSensitivity(value);
		}

		public void SetEnabledMicrophone(bool value)
		{
			_microphoneModel.IsMicrophoneEnabled = value;
		}

		public void SetNoiseSuppression(bool value)
		{
			_microphoneModel.IsNoiseSuppressionEnabled = value;
		}

		public void SetPushToTalk(bool value)
		{
			_microphoneModel.IsPushToTalkEnabled = value;
		}

		public void SetYAxisInvert(bool value)
		{
			_cameraModel.IsYAxisInverted = value;
		}

		public void SetEnabledVSync(bool value)
		{
			_vsyncService.SetVSync(value);
		}

		public void SetQualityLevel(int qualityLevel)
		{
			_qualitySettingsService.SetQualityLevel(qualityLevel);
		}

		public void SetMouseSensitivity(float mouseSensitivity)
		{
			_cameraModel.Sensitivity = mouseSensitivity;
		}

		public void SetFieldOfView(float fieldOfView)
		{
			_fieldOfViewModel.FieldOfView = fieldOfView;
		}

		public void SetBrightness(float brightness)
		{
			_brightnessModel.Brightness = brightness;
		}

		public void SetScreenMode(ScreenMode screenMode)
		{
			FullScreenMode fullScreenMode = GetFullScreenMode(screenMode);
			ChangeFullScreenMode(screenMode, fullScreenMode);
			ChangeCursorMode(screenMode == ScreenMode.FullScreen);
		}

		public void SetResolution(int x, int y)
		{
			FullScreenMode fullScreenMode = GetFullScreenMode(_currentSettingsModel.ScreenMode);
			if (Screen.width != x || Screen.height != y || Screen.fullScreenMode != fullScreenMode)
			{
				Screen.SetResolution(x, y, fullScreenMode);
			}
		}

		public void SetFrameRateLimit(int result)
		{
			Application.targetFrameRate = result;
		}

		public void SetScreenShakeIntensity(float normalizedIntensity)
		{
			_cameraLocalScreenShakeModel.ScreenShakeIntensityNormalized = normalizedIntensity;
		}

		public void SetHeadBobbingIntensity(float normalizedIntensity)
		{
			_headBobbingModel.HeadBobbingIntensityNormalized = normalizedIntensity;
		}

		public void SetLanguage(Language language)
		{
			_languageService.SetCurrentLanguage(language);
		}

		private FullScreenMode GetFullScreenMode(ScreenMode screenMode)
		{
			return screenMode switch
			{
				ScreenMode.FullScreen => FullScreenMode.ExclusiveFullScreen, 
				ScreenMode.WindowedWithBorders => FullScreenMode.Windowed, 
				ScreenMode.WindowedWithoutBorders => FullScreenMode.FullScreenWindow, 
				_ => FullScreenMode.ExclusiveFullScreen, 
			};
		}

		private void ChangeFullScreenMode(ScreenMode screenMode, FullScreenMode fullScreenMode)
		{
			if (Screen.fullScreenMode != fullScreenMode)
			{
				Screen.fullScreenMode = fullScreenMode;
				Screen.fullScreen = screenMode switch
				{
					ScreenMode.FullScreen => true, 
					ScreenMode.WindowedWithBorders => false, 
					ScreenMode.WindowedWithoutBorders => true, 
					_ => Screen.fullScreen, 
				};
			}
		}

		private void ChangeCursorMode(bool isFullScreen)
		{
			if (isFullScreen && IsDeviceRequireNotChangingCursorMode())
			{
				Cursor.lockState = CursorLockMode.Confined;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}

		private bool IsDeviceRequireNotChangingCursorMode()
		{
			return !Application.isEditor;
		}
	}
}
