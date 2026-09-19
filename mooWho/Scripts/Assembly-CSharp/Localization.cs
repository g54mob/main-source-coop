using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Localization
{
	private static class PluralRules
	{
		public static string GetCategory(Language lang, int n)
		{
			int num = Math.Abs(n);
			int num2 = num % 10;
			int num3 = num % 100;
			switch (lang)
			{
			case Language.Russian:
				if (num2 == 1 && num3 != 11)
				{
					return "one";
				}
				if (num2 >= 2 && num2 <= 4 && (num3 < 12 || num3 > 14))
				{
					return "few";
				}
				return "many";
			case Language.French:
				if (num != 0 && num != 1)
				{
					return "other";
				}
				return "one";
			case Language.Arabic:
				switch (num)
				{
				case 0:
					return "zero";
				case 1:
					return "one";
				case 2:
					return "two";
				default:
					if (num3 >= 3 && num3 <= 10)
					{
						return "few";
					}
					if (num3 >= 11 && num3 <= 99)
					{
						return "many";
					}
					return "other";
				}
			case Language.Turkish:
			case Language.ChineseSimplified:
			case Language.Japanese:
			case Language.Korean:
				return "other";
			default:
				if (num != 1)
				{
					return "other";
				}
				return "one";
			}
		}
	}

	private const string PrefsKey = "Language";

	private const string ResourcePath = "Localization/strings";

	private static Language _current;

	private static Dictionary<string, string[]> _table;

	public static Language Current
	{
		get
		{
			EnsureLoaded();
			return _current;
		}
		set
		{
			EnsureLoaded();
			if (_current != value)
			{
				_current = value;
				PlayerPrefs.SetInt("Language", (int)value);
				PlayerPrefs.Save();
				Localization.OnLanguageChanged?.Invoke();
			}
		}
	}

	public static event Action OnLanguageChanged;

	private static void EnsureLoaded()
	{
		if (_table == null)
		{
			_table = new Dictionary<string, string[]>();
			TextAsset textAsset = Resources.Load<TextAsset>("Localization/strings");
			if (textAsset == null)
			{
				Debug.LogError("[Localization] Resources/Localization/strings.csv bulunamadı — tüm metinler key olarak gösterilecek.");
			}
			else
			{
				ParseCsv(textAsset.text);
			}
			int num = PlayerPrefs.GetInt("Language", -1);
			_current = ((num >= 0 && Enum.IsDefined(typeof(Language), num)) ? ((Language)num) : DetectSystemLanguage());
		}
	}

	private static void ParseCsv(string csvText)
	{
		string[] array = csvText.Split('\n');
		bool flag = false;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i].TrimEnd('\r');
			if (text.Length == 0)
			{
				continue;
			}
			if (!flag)
			{
				flag = true;
				continue;
			}
			string[] array3 = SplitCsvLine(text);
			if (array3.Length == 0)
			{
				continue;
			}
			string text2 = array3[0].Trim();
			if (text2.Length != 0)
			{
				int length = Enum.GetValues(typeof(Language)).Length;
				string[] array4 = new string[length];
				for (int j = 0; j < length; j++)
				{
					int num = j + 1;
					array4[j] = ((num < array3.Length) ? array3[num] : string.Empty);
				}
				_table[text2] = array4;
			}
		}
	}

	private static string[] SplitCsvLine(string line)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];
			if (flag)
			{
				if (c == '"')
				{
					if (i + 1 < line.Length && line[i + 1] == '"')
					{
						stringBuilder.Append('"');
						i++;
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
				continue;
			}
			switch (c)
			{
			case '"':
				flag = true;
				break;
			case ',':
				list.Add(stringBuilder.ToString());
				stringBuilder.Clear();
				break;
			default:
				stringBuilder.Append(c);
				break;
			}
		}
		list.Add(stringBuilder.ToString());
		return list.ToArray();
	}

	private static Language DetectSystemLanguage()
	{
		return Application.systemLanguage switch
		{
			SystemLanguage.Turkish => Language.Turkish, 
			SystemLanguage.ChineseSimplified => Language.ChineseSimplified, 
			SystemLanguage.Portuguese => Language.PortugueseBR, 
			SystemLanguage.Russian => Language.Russian, 
			SystemLanguage.German => Language.German, 
			SystemLanguage.Spanish => Language.Spanish, 
			SystemLanguage.French => Language.French, 
			SystemLanguage.Japanese => Language.Japanese, 
			SystemLanguage.Korean => Language.Korean, 
			SystemLanguage.Arabic => Language.Arabic, 
			_ => Language.English, 
		};
	}

	private static bool TryGetRaw(string key, out string value)
	{
		EnsureLoaded();
		if (_table.TryGetValue(key, out var value2))
		{
			value = value2[(int)_current];
			return true;
		}
		value = null;
		return false;
	}

	public static string Get(string key)
	{
		if (TryGetRaw(key, out var value))
		{
			return value;
		}
		Debug.LogWarning("[Localization] Key bulunamadı: " + key);
		return key;
	}

	public static string GetFormatted(string key, params object[] args)
	{
		string text = Get(key);
		try
		{
			return string.Format(text, args);
		}
		catch (FormatException)
		{
			return text;
		}
	}

	public static string GetPlural(string keyBase, int count, params object[] args)
	{
		string category = PluralRules.GetCategory(Current, count);
		if (!TryGetRaw(keyBase + "_" + category, out var value) && !TryGetRaw(keyBase + "_other", out value))
		{
			Debug.LogWarning("[Localization] Plural key bulunamadı: " + keyBase);
			value = keyBase;
		}
		try
		{
			return string.Format(value, args);
		}
		catch (FormatException)
		{
			return value;
		}
	}

	public static string GetAnimalName(AnimalType animal)
	{
		string text = animal switch
		{
			AnimalType.None => "ANIMAL_NONE", 
			AnimalType.Cow => "ANIMAL_COW", 
			AnimalType.Horse => "ANIMAL_HORSE", 
			AnimalType.Pig => "ANIMAL_PIG", 
			AnimalType.Sheep => "ANIMAL_SHEEP", 
			AnimalType.Chicken => "ANIMAL_CHICKEN", 
			AnimalType.Duck => "ANIMAL_DUCK", 
			AnimalType.Chick => "ANIMAL_CHICK", 
			AnimalType.Rooster => "ANIMAL_ROOSTER", 
			AnimalType.Wolf => "ANIMAL_WOLF", 
			AnimalType.Raccoon => "ANIMAL_RACCOON", 
			AnimalType.Dog => "ANIMAL_DOG", 
			AnimalType.Rabbit => "ANIMAL_RABBIT", 
			AnimalType.Goat => "ANIMAL_GOAT", 
			AnimalType.Lion => "ANIMAL_LION", 
			AnimalType.Giraffe => "ANIMAL_GIRAFFE", 
			AnimalType.Panda => "ANIMAL_PANDA", 
			AnimalType.Gorilla => "ANIMAL_GORILLA", 
			AnimalType.Deer => "ANIMAL_DEER", 
			AnimalType.Cat => "ANIMAL_CAT", 
			AnimalType.Bear => "ANIMAL_BEAR", 
			AnimalType.Fox => "ANIMAL_FOX", 
			AnimalType.Bull => "ANIMAL_BULL", 
			_ => null, 
		};
		if (text == null)
		{
			return animal.ToString();
		}
		return Get(text);
	}

	public static string GetNativeLanguageName(Language lang)
	{
		return lang switch
		{
			Language.English => "English", 
			Language.Turkish => "Türkçe", 
			Language.ChineseSimplified => "简体中文", 
			Language.PortugueseBR => "Português (BR)", 
			Language.Russian => "Русский", 
			Language.German => "Deutsch", 
			Language.Spanish => "Español", 
			Language.French => "Français", 
			Language.Japanese => "日本語", 
			Language.Korean => "한국어", 
			Language.Arabic => "العربية", 
			_ => lang.ToString(), 
		};
	}
}
