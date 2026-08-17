using System;

namespace EvilCore.Localization
{
	internal class PluralResolver
	{
		public PluralCategory GetCategory(string localeCode, int count)
		{
			string languageCode = GetLanguageCode(localeCode);
			int num = Math.Abs(count);
			switch (languageCode)
			{
			case "tr":
			case "ja":
			case "ko":
			case "zh":
			case "vi":
			case "th":
			case "id":
				return PluralCategory.Other;
			case "en":
			case "de":
			case "nl":
			case "sv":
			case "da":
			case "no":
			case "nb":
			case "nn":
			case "it":
			case "es":
			case "pt":
			case "el":
			case "fi":
			case "et":
			case "hu":
			case "bg":
			case "he":
				return (num == 1) ? PluralCategory.One : PluralCategory.Other;
			case "fr":
			case "pt-BR":
				return (num <= 1) ? PluralCategory.One : PluralCategory.Other;
			case "ru":
			case "uk":
			case "hr":
			case "sr":
			case "bs":
				return GetSlavicCategory(num);
			case "pl":
				return GetPolishCategory(num);
			case "ar":
				return GetArabicCategory(num);
			case "cs":
			case "sk":
				return GetCzechSlovakCategory(num);
			case "ro":
				return GetRomanianCategory(num);
			default:
				return (num == 1) ? PluralCategory.One : PluralCategory.Other;
			}
		}

		public string Resolve(StringResolver resolver, string key, int count, string localeCode)
		{
			PluralCategory category = GetCategory(localeCode, count);
			string text = category.ToString().ToLowerInvariant();
			string key2 = key + "." + text;
			string text2 = resolver.Resolve(key2);
			if (text2 == null && category != PluralCategory.Other)
			{
				key2 = key + ".other";
				text2 = resolver.Resolve(key2);
			}
			return text2?.Replace("{count}", count.ToString());
		}

		private static string GetLanguageCode(string localeCode)
		{
			if (string.IsNullOrEmpty(localeCode))
			{
				return "en";
			}
			if (localeCode.Equals("pt-BR", StringComparison.OrdinalIgnoreCase))
			{
				return "pt-BR";
			}
			int num = localeCode.IndexOf('-');
			if (num <= 0)
			{
				return localeCode.ToLowerInvariant();
			}
			return localeCode.Substring(0, num).ToLowerInvariant();
		}

		private static PluralCategory GetSlavicCategory(int abs)
		{
			int num = abs % 10;
			int num2 = abs % 100;
			if (num == 1 && num2 != 11)
			{
				return PluralCategory.One;
			}
			if (num >= 2 && num <= 4 && (num2 < 12 || num2 > 14))
			{
				return PluralCategory.Few;
			}
			return PluralCategory.Many;
		}

		private static PluralCategory GetPolishCategory(int abs)
		{
			if (abs == 1)
			{
				return PluralCategory.One;
			}
			int num = abs % 10;
			int num2 = abs % 100;
			if (num >= 2 && num <= 4 && (num2 < 12 || num2 > 14))
			{
				return PluralCategory.Few;
			}
			return PluralCategory.Many;
		}

		private static PluralCategory GetArabicCategory(int abs)
		{
			switch (abs)
			{
			case 0:
				return PluralCategory.Zero;
			case 1:
				return PluralCategory.One;
			case 2:
				return PluralCategory.Two;
			default:
			{
				int num = abs % 100;
				if (num >= 3 && num <= 10)
				{
					return PluralCategory.Few;
				}
				if (num >= 11 && num <= 99)
				{
					return PluralCategory.Many;
				}
				return PluralCategory.Other;
			}
			}
		}

		private static PluralCategory GetCzechSlovakCategory(int abs)
		{
			switch (abs)
			{
			case 1:
				return PluralCategory.One;
			case 2:
			case 3:
			case 4:
				return PluralCategory.Few;
			default:
				return PluralCategory.Other;
			}
		}

		private static PluralCategory GetRomanianCategory(int abs)
		{
			if (abs == 1)
			{
				return PluralCategory.One;
			}
			int num = abs % 100;
			if (abs == 0 || (num >= 2 && num <= 19))
			{
				return PluralCategory.Few;
			}
			return PluralCategory.Other;
		}
	}
}
