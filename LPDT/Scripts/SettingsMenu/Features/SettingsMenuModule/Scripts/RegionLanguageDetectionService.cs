using Features.SteamImplementationModule.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	public class RegionLanguageDetectionService : IRegionLanguageDetectionService
	{
		private readonly LanguagesLocalizationConfiguration _languagesConfig;

		private readonly ISteamLanguageProvider _steamLanguageProvider;

		public RegionLanguageDetectionService(LanguagesLocalizationConfiguration languagesConfig, ISteamLanguageProvider steamLanguageProvider)
		{
			_languagesConfig = languagesConfig;
			_steamLanguageProvider = steamLanguageProvider;
		}

		public Language DetectLanguage()
		{
			if (TryGetSteamLanguage(out var language) && IsSupported(language))
			{
				return language;
			}
			if (TryGetSystemLanguage(out var language2) && IsSupported(language2))
			{
				return language2;
			}
			return Language.English;
		}

		private bool IsSupported(Language language)
		{
			return _languagesConfig.LanguageLocalizationKeys.ContainsKey(language);
		}

		private bool TryGetSteamLanguage(out Language language)
		{
			language = Language.English;
			if (!_steamLanguageProvider.IsAvailable)
			{
				return false;
			}
			string steamUILanguage = _steamLanguageProvider.GetSteamUILanguage();
			if (string.IsNullOrEmpty(steamUILanguage))
			{
				return false;
			}
			return _languagesConfig.TryGetLanguageBySteamCode(steamUILanguage, out language);
		}

		private bool TryGetSystemLanguage(out Language language)
		{
			language = Language.English;
			return _languagesConfig.TryGetLanguageBySystemLanguage(Application.systemLanguage, out language);
		}
	}
}
