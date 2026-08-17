using System;
using System.Collections.Generic;
using EvilCore.Localization;
using EvilCore.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsLanguageTab : MonoBehaviour
	{
		[Header("Selectors")]
		[SerializeField]
		private OptionSelector languageSelector;

		[Inject]
		private ISettingsManager _settingsManager;

		[Inject]
		private ILocalizationService _localizationService;

		private bool _isRefreshing;

		private bool _initialized;

		private readonly List<string> _localeCodes = new List<string>();

		private void OnEnable()
		{
			if (_initialized)
			{
				Refresh();
			}
		}

		public void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				PopulateOptions();
				BindListeners();
				Refresh();
			}
		}

		private void OnDestroy()
		{
			if (languageSelector != null)
			{
				languageSelector.OnValueChanged -= OnLanguageChanged;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= Refresh;
			}
		}

		public void Refresh()
		{
			if (_settingsManager != null && !(languageSelector == null))
			{
				_isRefreshing = true;
				languageSelector.SetValueWithoutNotify(FindLocaleIndex(_settingsManager.LocaleCode));
				_isRefreshing = false;
			}
		}

		private void PopulateOptions()
		{
			if (languageSelector == null)
			{
				return;
			}
			_localeCodes.Clear();
			List<string> list = new List<string>();
			IReadOnlyList<LocaleInfo> readOnlyList = _localizationService?.AvailableLocales;
			if (readOnlyList != null)
			{
				foreach (LocaleInfo item in readOnlyList)
				{
					_localeCodes.Add(item.Code);
					list.Add(GetDisplayName(item));
				}
			}
			if (list.Count == 0)
			{
				_localeCodes.Add((_settingsManager != null) ? _settingsManager.LocaleCode : "en");
				list.Add("Default");
				languageSelector.SetOptions(list);
				languageSelector.SetInteractable(interactable: false);
			}
			else
			{
				languageSelector.SetOptions(list);
			}
		}

		private static string GetDisplayName(LocaleInfo locale)
		{
			if (!string.IsNullOrEmpty(locale.NativeName))
			{
				return locale.NativeName;
			}
			if (!string.IsNullOrEmpty(locale.DisplayName))
			{
				return locale.DisplayName;
			}
			return locale.Code;
		}

		private int FindLocaleIndex(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return 0;
			}
			for (int i = 0; i < _localeCodes.Count; i++)
			{
				if (string.Equals(_localeCodes[i], code, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return 0;
		}

		private void BindListeners()
		{
			if (languageSelector != null)
			{
				languageSelector.OnValueChanged += OnLanguageChanged;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += Refresh;
			}
		}

		private void OnLanguageChanged(int index)
		{
			if (!_isRefreshing && index >= 0 && index < _localeCodes.Count)
			{
				_settingsManager.SetLocale(_localeCodes[index]);
			}
		}
	}
}
