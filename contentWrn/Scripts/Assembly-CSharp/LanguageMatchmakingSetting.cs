using System;
using System.Collections.Generic;
using Zorro.Settings;

public abstract class LanguageMatchmakingSetting : EnumSetting
{
	public enum MatchmakingLanguage
	{
		None = 0,
		ArabicAlgerian = 1,
		ArabicEgyptian = 2,
		Belarusian = 3,
		Bengali = 4,
		Croatian = 5,
		Czech = 6,
		Danish = 7,
		Dutch = 8,
		English = 9,
		Farsi = 10,
		Finnish = 11,
		French = 12,
		German = 13,
		Greek = 14,
		Gujarati = 15,
		Hindi = 16,
		Hungarian = 17,
		Italian = 18,
		Japanese = 19,
		Korean = 20,
		Malayalam = 21,
		Mandarin = 22,
		Marathi = 23,
		Norwegian = 24,
		Oriya = 25,
		Panjabi = 26,
		Polish = 27,
		Portuguese = 28,
		Russian = 29,
		Spanish = 30,
		Sunda = 31,
		Swedish = 32,
		Tamil = 33,
		Telugu = 34,
		Turkish = 35,
		Ukrainian = 36,
		Urdu = 37,
		Vietnamese = 38
	}

	public override void ApplyValue()
	{
	}

	public override int GetDefaultValue()
	{
		return 0;
	}

	public override List<string> GetChoices()
	{
		MatchmakingLanguage[] obj = (MatchmakingLanguage[])Enum.GetValues(typeof(MatchmakingLanguage));
		List<string> list = new List<string>();
		MatchmakingLanguage[] array = obj;
		foreach (MatchmakingLanguage matchmakingLanguage in array)
		{
			if (Enum.TryParse<LocalizationKeys.Keys>($"MM_{matchmakingLanguage}", ignoreCase: false, out var result))
			{
				list.Add(LocalizationKeys.GetLocalizedString(result));
			}
		}
		return list;
	}

	private string TranslateLang(MatchmakingLanguage lang)
	{
		return lang switch
		{
			MatchmakingLanguage.ArabicAlgerian => "\u202aالعربية (اللهجة الجزائرية)\u202c", 
			MatchmakingLanguage.ArabicEgyptian => "\u202aالعربية (اللهجة المصرية)\u202c", 
			MatchmakingLanguage.Belarusian => "Беларуская", 
			MatchmakingLanguage.Bengali => "ব\u09be\u0982ল\u09be", 
			MatchmakingLanguage.Croatian => "Hrvatski", 
			MatchmakingLanguage.Czech => "Čeština", 
			MatchmakingLanguage.Danish => "Dansk", 
			MatchmakingLanguage.Dutch => "Nederlands", 
			MatchmakingLanguage.English => "English", 
			MatchmakingLanguage.Farsi => "\u202aفارسی\u202c", 
			MatchmakingLanguage.Finnish => "Suomi", 
			MatchmakingLanguage.French => "Français", 
			MatchmakingLanguage.German => "Deutsch", 
			MatchmakingLanguage.Greek => "Ελληνικά", 
			MatchmakingLanguage.Gujarati => "\u0ac1જર\u0abeત\u0ac0", 
			MatchmakingLanguage.Hindi => "न\u094dद\u0940", 
			MatchmakingLanguage.Hungarian => "Magyar", 
			MatchmakingLanguage.Italian => "Italiano", 
			MatchmakingLanguage.Japanese => "日本語", 
			MatchmakingLanguage.Korean => "한국어", 
			MatchmakingLanguage.Malayalam => "മലയ\u0d3eള\u0d02", 
			MatchmakingLanguage.Mandarin => "中文 (普通话)", 
			MatchmakingLanguage.Marathi => "मर\u093eठ\u0940", 
			MatchmakingLanguage.Norwegian => "Norsk", 
			MatchmakingLanguage.Oriya => "ଓଡ\u0b3c\u0b3fଆ", 
			MatchmakingLanguage.Panjabi => "ਪ\u0a70ਜ\u0a3eਬ\u0a40", 
			MatchmakingLanguage.Polish => "Polski", 
			MatchmakingLanguage.Portuguese => "Português", 
			MatchmakingLanguage.Russian => "Русский", 
			MatchmakingLanguage.Spanish => "Español", 
			MatchmakingLanguage.Sunda => "Basa Sunda", 
			MatchmakingLanguage.Swedish => "Svenska", 
			MatchmakingLanguage.Tamil => "தம\u0bbfழ\u0bcd", 
			MatchmakingLanguage.Telugu => "త\u0c46ల\u0c41గ\u0c41", 
			MatchmakingLanguage.Turkish => "Türkçe", 
			MatchmakingLanguage.Ukrainian => "Українська", 
			MatchmakingLanguage.Urdu => "\u202aاردو\u202c", 
			MatchmakingLanguage.Vietnamese => "Tiếng Việt", 
			_ => "None", 
		};
	}
}
