using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Localization;
using EvilCore.Managers;
using EvilCore.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsDisplayTab : MonoBehaviour
	{
		[Header("Selectors")]
		[SerializeField]
		private OptionSelector displayModeSelector;

		[SerializeField]
		private OptionSelector resolutionSelector;

		[SerializeField]
		private OptionSelector fpsLimitSelector;

		[SerializeField]
		private OptionSelector vsyncSelector;

		[Header("Buttons")]
		[SerializeField]
		private Button applyButton;

		[SerializeField]
		private Button restoreButton;

		[Header("Sound")]
		[SerializeField]
		private SoundID applySound;

		[SerializeField]
		private SoundID restoreSound;

		[Inject]
		private ISettingsManager _settingsManager;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private ILocalizationService _localizationService;

		private bool _isRefreshing;

		private bool _initialized;

		private List<(int width, int height)> _availableResolutions = new List<(int, int)>();

		private static readonly (int w, int h)[] CommonResolutions = new(int, int)[8]
		{
			(3840, 2160),
			(2560, 1440),
			(1920, 1080),
			(1680, 1050),
			(1600, 900),
			(1440, 900),
			(1366, 768),
			(1280, 720)
		};

		private void OnEnable()
		{
			if (_initialized)
			{
				Refresh();
				RefreshApplyButton();
			}
		}

		public void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				PopulateOptions();
				BindListeners();
				applyButton?.onClick.AddListener(OnApplyClicked);
				restoreButton?.onClick.AddListener(OnRestoreClicked);
				SubscribeDirty();
				if (_localizationService != null)
				{
					_localizationService.OnLocaleChanged += OnLocaleChanged;
				}
				Refresh();
				RefreshApplyButton();
			}
		}

		private void OnDestroy()
		{
			if (displayModeSelector != null)
			{
				displayModeSelector.OnValueChanged -= OnDisplayModeChanged;
			}
			if (resolutionSelector != null)
			{
				resolutionSelector.OnValueChanged -= OnResolutionChanged;
			}
			if (fpsLimitSelector != null)
			{
				fpsLimitSelector.OnValueChanged -= OnFpsLimitChanged;
			}
			if (vsyncSelector != null)
			{
				vsyncSelector.OnValueChanged -= OnVSyncChanged;
			}
			UnsubscribeDirty();
			applyButton?.onClick.RemoveAllListeners();
			restoreButton?.onClick.RemoveAllListeners();
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= OnLocaleChanged;
			}
		}

		private void OnLocaleChanged()
		{
			PopulateOptions();
			Refresh();
		}

		private List<string> Localized(params (string key, string english)[] items)
		{
			List<string> list = new List<string>(items.Length);
			for (int i = 0; i < items.Length; i++)
			{
				var (key, text) = items[i];
				list.Add((_localizationService != null) ? _localizationService.Localize(key) : text);
			}
			return list;
		}

		public void Refresh()
		{
			if (_settingsManager != null)
			{
				_isRefreshing = true;
				displayModeSelector?.SetValueWithoutNotify((int)_settingsManager.DisplayMode);
				resolutionSelector?.SetValueWithoutNotify(FindResolutionIndex(_settingsManager.ResolutionWidth, _settingsManager.ResolutionHeight));
				fpsLimitSelector?.SetValueWithoutNotify((int)_settingsManager.FpsLimit);
				vsyncSelector?.SetValueWithoutNotify(_settingsManager.VSync ? 1 : 0);
				UpdateResolutionInteractable();
				_isRefreshing = false;
			}
		}

		private void PopulateOptions()
		{
			displayModeSelector?.SetOptions(Localized(("@settings.fullscreen", "Fullscreen"), ("@settings.windowed", "Windowed"), ("@settings.borderless", "Borderless")));
			PopulateResolutions();
			fpsLimitSelector?.SetOptions(Localized(("@settings.fps_60", "60 FPS"), ("@settings.fps_90", "90 FPS"), ("@settings.fps_120", "120 FPS"), ("@settings.fps_144", "144 FPS"), ("@settings.fps_unlimited", "Unlimited")));
			vsyncSelector?.SetOptions(Localized(("@settings.off", "Off"), ("@settings.on", "On")));
		}

		private void PopulateResolutions()
		{
			if (resolutionSelector == null)
			{
				return;
			}
			_availableResolutions.Clear();
			int num = Display.main.systemWidth;
			int num2 = Display.main.systemHeight;
			if (num <= 0 || num2 <= 0)
			{
				Resolution currentResolution = Screen.currentResolution;
				num = currentResolution.width;
				num2 = currentResolution.height;
			}
			HashSet<(int, int)> hashSet = new HashSet<(int, int)>();
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution resolution = resolutions[i];
				hashSet.Add((resolution.width, resolution.height));
			}
			List<string> list = new List<string>();
			bool flag = false;
			(int, int)[] commonResolutions = CommonResolutions;
			for (int i = 0; i < commonResolutions.Length; i++)
			{
				var (num3, num4) = commonResolutions[i];
				if (num3 <= num && num4 <= num2 && hashSet.Contains((num3, num4)))
				{
					_availableResolutions.Add((num3, num4));
					list.Add($"{num3}x{num4}");
					if (num3 == num && num4 == num2)
					{
						flag = true;
					}
				}
			}
			if (!flag && num > 0 && num2 > 0)
			{
				_availableResolutions.Insert(0, (num, num2));
				list.Insert(0, $"{num}x{num2}");
			}
			if (_availableResolutions.Count == 0)
			{
				_availableResolutions.Add((num, num2));
				list.Add($"{num}x{num2}");
			}
			resolutionSelector.SetOptions(list);
		}

		private int FindResolutionIndex(int width, int height)
		{
			if (width <= 0 || height <= 0)
			{
				return 0;
			}
			for (int i = 0; i < _availableResolutions.Count; i++)
			{
				var (num, num2) = _availableResolutions[i];
				if (num == width && num2 == height)
				{
					return i;
				}
			}
			return 0;
		}

		private void BindListeners()
		{
			if (displayModeSelector != null)
			{
				displayModeSelector.OnValueChanged += OnDisplayModeChanged;
			}
			if (resolutionSelector != null)
			{
				resolutionSelector.OnValueChanged += OnResolutionChanged;
			}
			if (fpsLimitSelector != null)
			{
				fpsLimitSelector.OnValueChanged += OnFpsLimitChanged;
			}
			if (vsyncSelector != null)
			{
				vsyncSelector.OnValueChanged += OnVSyncChanged;
			}
		}

		private void OnDisplayModeChanged(int index)
		{
			if (!_isRefreshing)
			{
				var (width, height) = GetSelectedResolution();
				_settingsManager.SetDisplay((ResolutionType)index, width, height);
				UpdateResolutionInteractable();
			}
		}

		private void OnResolutionChanged(int index)
		{
			if (!_isRefreshing)
			{
				var (width, height) = GetSelectedResolution();
				_settingsManager.SetDisplay(_settingsManager.DisplayMode, width, height);
			}
		}

		private void OnFpsLimitChanged(int index)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetFpsLimit((FpsLimit)index);
			}
		}

		private void OnVSyncChanged(int index)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetVSync(index == 1);
			}
		}

		private void SubscribeDirty()
		{
			if (displayModeSelector != null)
			{
				displayModeSelector.OnDirtyStateChanged += RefreshApplyButton;
			}
			if (resolutionSelector != null)
			{
				resolutionSelector.OnDirtyStateChanged += RefreshApplyButton;
			}
			if (fpsLimitSelector != null)
			{
				fpsLimitSelector.OnDirtyStateChanged += RefreshApplyButton;
			}
			if (vsyncSelector != null)
			{
				vsyncSelector.OnDirtyStateChanged += RefreshApplyButton;
			}
		}

		private void UnsubscribeDirty()
		{
			if (displayModeSelector != null)
			{
				displayModeSelector.OnDirtyStateChanged -= RefreshApplyButton;
			}
			if (resolutionSelector != null)
			{
				resolutionSelector.OnDirtyStateChanged -= RefreshApplyButton;
			}
			if (fpsLimitSelector != null)
			{
				fpsLimitSelector.OnDirtyStateChanged -= RefreshApplyButton;
			}
			if (vsyncSelector != null)
			{
				vsyncSelector.OnDirtyStateChanged -= RefreshApplyButton;
			}
		}

		private void RefreshApplyButton()
		{
			if (applyButton != null)
			{
				applyButton.interactable = IsAnyDirty();
			}
		}

		private bool IsAnyDirty()
		{
			if ((!(displayModeSelector != null) || !displayModeSelector.IsDirty) && (!(resolutionSelector != null) || !resolutionSelector.IsDirty) && (!(fpsLimitSelector != null) || !fpsLimitSelector.IsDirty))
			{
				if (vsyncSelector != null)
				{
					return vsyncSelector.IsDirty;
				}
				return false;
			}
			return true;
		}

		private void OnApplyClicked()
		{
			PlaySound(applySound);
			ForceNotifyIfDirty(displayModeSelector);
			ForceNotifyIfDirty(resolutionSelector);
			ForceNotifyIfDirty(fpsLimitSelector);
			ForceNotifyIfDirty(vsyncSelector);
			RefreshApplyButton();
		}

		private void OnRestoreClicked()
		{
			PlaySound(restoreSound);
			_settingsManager?.ResetDisplayToDefaults();
			Refresh();
			RefreshApplyButton();
		}

		private static void ForceNotifyIfDirty(OptionSelector selector)
		{
			if (selector != null && selector.IsDirty)
			{
				selector.ForceNotify();
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}

		private (int width, int height) GetSelectedResolution()
		{
			if (resolutionSelector == null || _availableResolutions.Count == 0)
			{
				return (width: 0, height: 0);
			}
			int index = Mathf.Clamp(resolutionSelector.CurrentIndex, 0, _availableResolutions.Count - 1);
			return _availableResolutions[index];
		}

		private void UpdateResolutionInteractable()
		{
			if (!(displayModeSelector == null) && !(resolutionSelector == null))
			{
				resolutionSelector.SetInteractable(displayModeSelector.CurrentIndex != 2);
			}
		}
	}
}
