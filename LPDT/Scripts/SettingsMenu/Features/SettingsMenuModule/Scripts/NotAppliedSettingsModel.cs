using System;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	public class NotAppliedSettingsModel
	{
		private float _allVolume;

		private bool _isAllVolumeChanged;

		private bool _microphoneEnabled;

		private bool _isMicrophoneEnabledChanged;

		private bool _noiseSuppressionEnabled;

		private bool _isNoiseSuppressionEnabledChanged;

		private bool _pushToTalkEnabled;

		private bool _isPushToTalkEnabledChanged;

		private bool _yAxisInvertEnabled;

		private bool _isYAxisInvertEnabledChanged;

		private bool _isVSyncEnabledChanged;

		private bool _vSyncEnabled;

		private bool _isStreamerModeEnabledChanged;

		private bool _streamerModeEnabled;

		private float _musicVolume;

		private bool _isMusicVolumeChanged;

		private bool _isScreenModeChanged;

		private bool _isResolutionChanged;

		private bool _isFpsLimitChanged;

		private bool _isHeadBobbingIntensityChanged;

		private bool _isLanguageChanged;

		private float _effectsVolume;

		private bool _isEffectsVolumeChanged;

		private string _microphoneDevice;

		private bool _isMicrophoneDeviceChanged;

		private float _microphoneSensitivity;

		private float _screenShakeIntensityNormalized;

		private bool _isMicrophoneSensitivityChanged;

		private bool _isScreenShakeIntensityChanged;

		private int _qualityLevel;

		private bool _isQualityLevelChanged;

		private float _mouseSensitivity;

		private bool _isMouseSensitivityChanged;

		private float _fieldOfView;

		private bool _isFieldOfViewChanged;

		private bool _isBrightnessChanged;

		private float _brightness;

		private int _screenMode;

		private Vector2Int _resolution;

		private int _fpsLimit;

		private float _headBobbingIntensityNormalized;

		private int _language;

		public bool IsAllVolumeChanged => _isAllVolumeChanged;

		public bool IsMusicVolumeChanged => _isMusicVolumeChanged;

		public bool IsEffectsVolumeChanged => _isEffectsVolumeChanged;

		public bool IsMicrophoneDeviceChanged => _isMicrophoneDeviceChanged;

		public bool IsMicrophoneSensitivityChanged => _isMicrophoneSensitivityChanged;

		public bool IsMicrophoneEnabledChanged => _isMicrophoneEnabledChanged;

		public bool IsNoiseSuppressionEnabledChanged => _isNoiseSuppressionEnabledChanged;

		public bool IsPushToTalkEnabledChanged => _isPushToTalkEnabledChanged;

		public bool IsYAxisInvertEnabledChanged => _isYAxisInvertEnabledChanged;

		public bool IsVSyncEnabledChanged => _isVSyncEnabledChanged;

		public bool IsStreamerModeEnabledChanged => _isStreamerModeEnabledChanged;

		public bool IsQualityLevelChanged => _isQualityLevelChanged;

		public bool IsScreenModeChanged => _isScreenModeChanged;

		public bool IsMouseSensitivityChanged => _isMouseSensitivityChanged;

		public bool IsFieldOfViewChanged => _isFieldOfViewChanged;

		public bool IsBrightnessChanged => _isBrightnessChanged;

		public bool IsResolutionChanged => _isResolutionChanged;

		public bool IsFpsLimitChanged => _isFpsLimitChanged;

		public bool IsScreenShakeIntensityChanged => _isScreenShakeIntensityChanged;

		public bool IsHeadBobbingIntensityChanged => _isHeadBobbingIntensityChanged;

		public bool IsLanguageChanged => _isLanguageChanged;

		public float AllVolume
		{
			get
			{
				return _allVolume;
			}
			set
			{
				_allVolume = value;
				_isAllVolumeChanged = true;
				this.OnAllVolumeChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float MusicVolume
		{
			get
			{
				return _musicVolume;
			}
			set
			{
				_musicVolume = value;
				_isMusicVolumeChanged = true;
				this.OnMusicVolumeChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float EffectsVolume
		{
			get
			{
				return _effectsVolume;
			}
			set
			{
				_effectsVolume = value;
				_isEffectsVolumeChanged = true;
				this.OnEffectsVolumeChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public string MicrophoneDevice
		{
			get
			{
				return _microphoneDevice;
			}
			set
			{
				_microphoneDevice = value;
				_isMicrophoneDeviceChanged = true;
				this.OnMicrophoneDeviceChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float MicrophoneSensitivity
		{
			get
			{
				return _microphoneSensitivity;
			}
			set
			{
				_microphoneSensitivity = value;
				_isMicrophoneSensitivityChanged = true;
				this.OnMicrophoneSensitivityChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float ScreenShakeIntensityNormalized
		{
			get
			{
				return _screenShakeIntensityNormalized;
			}
			set
			{
				_screenShakeIntensityNormalized = value;
				_isScreenShakeIntensityChanged = true;
				this.OnScreenShakeIntensityChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool MicrophoneEnabled
		{
			get
			{
				return _microphoneEnabled;
			}
			set
			{
				_microphoneEnabled = value;
				_isMicrophoneEnabledChanged = true;
				this.OnMicrophoneEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool NoiseSuppressionEnabled
		{
			get
			{
				return _noiseSuppressionEnabled;
			}
			set
			{
				_noiseSuppressionEnabled = value;
				_isNoiseSuppressionEnabledChanged = true;
				this.OnNoiseSuppressionEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool PushToTalkEnabled
		{
			get
			{
				return _pushToTalkEnabled;
			}
			set
			{
				_pushToTalkEnabled = value;
				_isPushToTalkEnabledChanged = true;
				this.OnPushToTalkEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool YAxisInvertEnabled
		{
			get
			{
				return _yAxisInvertEnabled;
			}
			set
			{
				_yAxisInvertEnabled = value;
				_isYAxisInvertEnabledChanged = true;
				this.OnYAxisInvertEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool VSyncEnabled
		{
			get
			{
				return _vSyncEnabled;
			}
			set
			{
				_vSyncEnabled = value;
				_isVSyncEnabledChanged = true;
				this.OnVSyncEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public bool StreamerModeEnabled
		{
			get
			{
				return _streamerModeEnabled;
			}
			set
			{
				_streamerModeEnabled = value;
				_isStreamerModeEnabledChanged = true;
				this.OnStreamerModeEnabledChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public int QualityLevel
		{
			get
			{
				return _qualityLevel;
			}
			set
			{
				_qualityLevel = value;
				_isQualityLevelChanged = true;
				this.OnQualityLevelChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public int ScreenMode
		{
			get
			{
				return _screenMode;
			}
			set
			{
				_screenMode = value;
				_isScreenModeChanged = true;
				this.OnScreenModeChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float MouseSensitivity
		{
			get
			{
				return _mouseSensitivity;
			}
			set
			{
				_mouseSensitivity = value;
				_isMouseSensitivityChanged = true;
				this.OnMouseSensitivityChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float FieldOfView
		{
			get
			{
				return _fieldOfView;
			}
			set
			{
				_fieldOfView = value;
				_isFieldOfViewChanged = true;
				this.OnFieldOfViewChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float Brightness
		{
			get
			{
				return _brightness;
			}
			set
			{
				_brightness = value;
				_isBrightnessChanged = true;
				this.OnBrightnessChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public Vector2Int Resolution
		{
			get
			{
				return _resolution;
			}
			set
			{
				_resolution = value;
				_isResolutionChanged = true;
				this.OnResolutionChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public int FpsLimit
		{
			get
			{
				return _fpsLimit;
			}
			set
			{
				_fpsLimit = value;
				_isFpsLimitChanged = true;
				this.OnFpsLimitChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public float HeadBobbingIntensityNormalized
		{
			get
			{
				return _headBobbingIntensityNormalized;
			}
			set
			{
				_headBobbingIntensityNormalized = value;
				_isHeadBobbingIntensityChanged = true;
				this.OnHeadBobbingIntensityChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public int Language
		{
			get
			{
				return _language;
			}
			set
			{
				_language = value;
				_isLanguageChanged = true;
				this.OnLanguageChanged?.Invoke(value);
				this.OnSettingsChanged?.Invoke();
			}
		}

		public event Action OnSettingsChanged;

		public event Action<float> OnAllVolumeChanged;

		public event Action<float> OnMusicVolumeChanged;

		public event Action<float> OnEffectsVolumeChanged;

		public event Action<string> OnMicrophoneDeviceChanged;

		public event Action<float> OnMicrophoneSensitivityChanged;

		public event Action<float> OnScreenShakeIntensityChanged;

		public event Action<bool> OnMicrophoneEnabledChanged;

		public event Action<bool> OnNoiseSuppressionEnabledChanged;

		public event Action<bool> OnPushToTalkEnabledChanged;

		public event Action<bool> OnYAxisInvertEnabledChanged;

		public event Action<bool> OnVSyncEnabledChanged;

		public event Action<bool> OnStreamerModeEnabledChanged;

		public event Action<int> OnQualityLevelChanged;

		public event Action<float> OnMouseSensitivityChanged;

		public event Action<float> OnFieldOfViewChanged;

		public event Action<float> OnBrightnessChanged;

		public event Action<int> OnScreenModeChanged;

		public event Action<Vector2Int> OnResolutionChanged;

		public event Action<int> OnFpsLimitChanged;

		public event Action<float> OnHeadBobbingIntensityChanged;

		public event Action<int> OnLanguageChanged;

		public void Reset()
		{
			_allVolume = 0f;
			_musicVolume = 0f;
			_effectsVolume = 0f;
			_microphoneSensitivity = 0f;
			_screenShakeIntensityNormalized = 0f;
			_mouseSensitivity = 0f;
			_fieldOfView = 0f;
			_brightness = 0f;
			_headBobbingIntensityNormalized = 0f;
			_microphoneEnabled = false;
			_noiseSuppressionEnabled = false;
			_pushToTalkEnabled = false;
			_yAxisInvertEnabled = false;
			_vSyncEnabled = false;
			_streamerModeEnabled = false;
			_microphoneDevice = null;
			_qualityLevel = 0;
			_screenMode = 0;
			_resolution = Vector2Int.zero;
			_fpsLimit = 0;
			_language = 0;
			_isAllVolumeChanged = false;
			_isMusicVolumeChanged = false;
			_isEffectsVolumeChanged = false;
			_isMicrophoneSensitivityChanged = false;
			_isMicrophoneEnabledChanged = false;
			_isNoiseSuppressionEnabledChanged = false;
			_isPushToTalkEnabledChanged = false;
			_isYAxisInvertEnabledChanged = false;
			_isMicrophoneDeviceChanged = false;
			_isVSyncEnabledChanged = false;
			_isStreamerModeEnabledChanged = false;
			_isQualityLevelChanged = false;
			_isScreenModeChanged = false;
			_isResolutionChanged = false;
			_isFpsLimitChanged = false;
			_isScreenShakeIntensityChanged = false;
			_isHeadBobbingIntensityChanged = false;
			_isMouseSensitivityChanged = false;
			_isFieldOfViewChanged = false;
			_isBrightnessChanged = false;
			_isLanguageChanged = false;
		}
	}
}
