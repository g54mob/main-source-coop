using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.GraphicsQuality;
using EvilCore.Localization;
using EvilCore.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsGraphicsTab : MonoBehaviour
	{
		[Header("Selectors")]
		[SerializeField]
		private OptionSelector qualitySelector;

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
				if (qualitySelector != null)
				{
					qualitySelector.OnDirtyStateChanged += RefreshApplyButton;
				}
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
			if (qualitySelector != null)
			{
				qualitySelector.OnValueChanged -= OnQualityChanged;
				qualitySelector.OnDirtyStateChanged -= RefreshApplyButton;
			}
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

		public void Refresh()
		{
			if (_settingsManager != null)
			{
				_isRefreshing = true;
				qualitySelector?.SetValueWithoutNotify((int)_settingsManager.GraphicsLevel);
				_isRefreshing = false;
			}
		}

		private void PopulateOptions()
		{
			qualitySelector?.SetOptions(new List<string>
			{
				Loc("@settings.quality_low", "Low"),
				Loc("@settings.quality_medium", "Medium"),
				Loc("@settings.quality_high", "High")
			});
		}

		private string Loc(string key, string english)
		{
			if (_localizationService == null)
			{
				return english;
			}
			return _localizationService.Localize(key);
		}

		private void BindListeners()
		{
			if (qualitySelector != null)
			{
				qualitySelector.OnValueChanged += OnQualityChanged;
			}
		}

		private void OnQualityChanged(int index)
		{
			if (!_isRefreshing)
			{
				GraphicsQualityLevel graphicsLevel = (GraphicsQualityLevel)index;
				_settingsManager.SetGraphicsLevel(graphicsLevel);
			}
		}

		private void RefreshApplyButton()
		{
			if (applyButton != null)
			{
				applyButton.interactable = qualitySelector != null && qualitySelector.IsDirty;
			}
		}

		private void OnApplyClicked()
		{
			PlaySound(applySound);
			if (qualitySelector != null && qualitySelector.IsDirty)
			{
				qualitySelector.ForceNotify();
			}
			RefreshApplyButton();
		}

		private void OnRestoreClicked()
		{
			PlaySound(restoreSound);
			_settingsManager?.ResetGraphicsToDefaults();
			Refresh();
			RefreshApplyButton();
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}
	}
}
