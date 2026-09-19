using System.Collections.Generic;
using System.Linq;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.SerializableDictionary;
using UnityEngine;

namespace Global.Modules.Localization_Module.Scripts
{
	[CreateAssetMenu(fileName = "LanguagesLocalizationConfiguration_Default", menuName = "Configurations/LocalizationModule/LanguagesLocalizationConfiguration")]
	public class LanguagesLocalizationConfiguration : ScriptableObject
	{
		public SerializableDictionary<Language, LocalizationKey> LanguageLocalizationKeys;

		public SerializableDictionary<string, Language> SteamLanguageCodes;

		public SerializableDictionary<SystemLanguage, Language> SystemLanguageCodes;

		public List<Language> ArabicLanguages;

		public bool TryGetLanguageBySteamCode(string steamCode, out Language language)
		{
			return SteamLanguageCodes.TryGetValue(steamCode, out language);
		}

		public bool TryGetLanguageBySystemLanguage(SystemLanguage systemLanguage, out Language language)
		{
			return SystemLanguageCodes.TryGetValue(systemLanguage, out language);
		}

		public Language GetLanguageByIndex(int index)
		{
			if (LanguageLocalizationKeys.Keys.Count < index)
			{
				return LanguageLocalizationKeys.Keys.ElementAt(0);
			}
			return LanguageLocalizationKeys.Keys.ElementAt(index);
		}

		public List<LocalizationKey> GetLocalizationKeysForLanguages()
		{
			return LanguageLocalizationKeys.Values.ToList();
		}

		public int GetLanguageIndex(Language language)
		{
			return LanguageLocalizationKeys.Keys.ToList().IndexOf(language);
		}
	}
}
