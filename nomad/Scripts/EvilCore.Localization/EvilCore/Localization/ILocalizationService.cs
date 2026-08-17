using System;
using System.Collections.Generic;
using TMPro;

namespace EvilCore.Localization
{
	public interface ILocalizationService
	{
		bool IsInitialized { get; }

		string CurrentLocaleCode { get; }

		IReadOnlyList<LocaleInfo> AvailableLocales { get; }

		PseudoLocalizationMode PseudoMode { get; set; }

		event Action OnLocaleChanged;

		event Action<TMP_FontAsset> OnFontChanged;

		string Localize(string key);

		string Localize(string key, params object[] args);

		string LocalizeWithParams(string key, params (string name, object value)[] namedArgs);

		string LocalizeName(string englishName);

		bool HasKey(string key);

		void SetLocale(string localeCode);

		string Pluralize(string key, int count);

		string Pluralize(string key, int count, params (string name, object value)[] namedArgs);

		string FormatNumber(double value, int decimals = 0);

		string FormatPercent(float ratio, int decimals = 0);

		string FormatDate(DateTime date);

		string FormatTime(int hours, int minutes);

		string FormatTemperature(float celsius);

		TMP_FontAsset GetLocalizedFont();

		TMP_FontAsset GetLocalizedFont(string style);

		IReadOnlyList<MissingTranslation> GetMissingTranslations();

		TranslationCoverage GetCoverage(string localeCode);

		void ClearMissingTranslations();
	}
}
