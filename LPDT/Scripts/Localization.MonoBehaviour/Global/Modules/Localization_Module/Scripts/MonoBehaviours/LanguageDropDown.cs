using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts.MonoBehaviours
{
	public class LanguageDropDown : EnumDropDown<Language>
	{
		private LanguagesLocalizationConfiguration _languageLocalizationConfigurationReference;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		[Inject]
		private void InjectDependencies(LanguagesLocalizationConfiguration languageLocalizationConfiguration, ILocalizationService localizationService, ILanguageService languageService)
		{
			_languageService = languageService;
			_languageLocalizationConfigurationReference = languageLocalizationConfiguration;
			_localizationService = localizationService;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_languageService != null)
			{
				ILanguageService languageService = _languageService;
				languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(base.FillWithEnumValues));
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (_languageService != null)
			{
				ILanguageService languageService = _languageService;
				languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(base.FillWithEnumValues));
			}
		}

		protected override string GetDisplayName(Language enumValue)
		{
			if (_localizationService != null)
			{
				return _localizationService.GetLocalizedString(_languageLocalizationConfigurationReference.LanguageLocalizationKeys[enumValue]);
			}
			return enumValue.ToString();
		}
	}
}
