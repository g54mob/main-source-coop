using System;
using System.Collections.Generic;
using System.Linq;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class LocalizationSettingsItemView : LocalizationSettingsItemViewBase
	{
		[SerializeField]
		private SettingsMultipleItems _localizationSettings;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		private LanguagesLocalizationConfiguration _languageLocalizationConfiguration;

		private List<LocalizationKey> _localizationKeysForLanguages;

		[Inject]
		public void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService, LanguagesLocalizationConfiguration languageLocalizationConfiguration)
		{
			_languageLocalizationConfiguration = languageLocalizationConfiguration;
			_languageService = languageService;
			_localizationService = localizationService;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_localizationSettings.OnValueChanged += OnValueChangedHandler;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(UpdateLocalization));
			_localizationKeysForLanguages = _languageLocalizationConfiguration.GetLocalizationKeysForLanguages();
			_localizationSettings.SetValues(_localizationKeysForLanguages.Select((LocalizationKey key) => _localizationService.GetLocalizedString(key)).ToList());
			UpdateLocalization();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_localizationSettings.OnValueChanged -= OnValueChanged;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(UpdateLocalization));
		}

		public override void SetValueIndex(int index)
		{
			_localizationSettings.SetIndex(index);
		}

		private void OnValueChangedHandler(int index)
		{
			OnValueChanged?.Invoke(index);
			UpdateLocalization();
		}

		private void UpdateLocalization()
		{
			_localizationSettings.Value.SetText(_localizationService.GetLocalizedString(_localizationKeysForLanguages[_localizationSettings.ValueIndex]));
		}
	}
}
