using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zorro.Settings;

public class LanguageSetting : EnumSetting, IExposedSetting
{
	private List<string> m_languages = new List<string>
	{
		"English", "French", "Italian", "German", "Spanish", "Japanese", "Chinese (Simplified)", "Chinese (Traditional)", "Portuguese", "Russian",
		"Ukrainian", "Korean", "Swedish"
	};

	private List<string> m_choices;

	private List<Locale> m_locales;

	private int m_default;

	public LanguageSetting()
	{
		m_choices = new List<string>(m_languages.Count);
		m_locales = LocalizationSettings.AvailableLocales.Locales;
		m_locales = m_locales.OrderBy((Locale loc) => m_languages.FindIndex((string lan) => loc.LocaleName.StartsWith(lan, StringComparison.OrdinalIgnoreCase))).ToList();
		for (int num = 0; num < m_languages.Count; num++)
		{
			string text = m_languages[num].Replace(" ", "").Replace("(", "").Replace(")", "");
			if (Enum.TryParse<LocalizationKeys.Keys>("Language_" + text, ignoreCase: false, out var result))
			{
				m_choices.Add(LocalizationKeys.GetLocalizedString(m_locales[num], result));
			}
		}
		m_default = m_locales.IndexOf(LocalizationSettings.ProjectLocale);
	}

	public override void Load(ISettingsSaveLoad loader)
	{
		if (loader.TryLoadInt(GetType(), out var o))
		{
			base.Value = o;
		}
		else
		{
			LoadSteamLanguage();
		}
		Debug.Log("Loaded Language: " + m_choices[base.Value]);
	}

	private void LoadSystemLanguage()
	{
		Debug.Log("Loading System Language...");
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		bool flag = false;
		for (int i = 0; i < m_locales.Count; i++)
		{
			if (m_locales[i].Formatter is CultureInfo cultureInfo && cultureInfo.TwoLetterISOLanguageName == currentCulture.TwoLetterISOLanguageName)
			{
				base.Value = i;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogWarning("Failed Loading " + GetType().FullName + " from PlayerPrefs");
			base.Value = GetDefaultValue();
		}
	}

	private void LoadSteamLanguage()
	{
		Debug.Log("Loading Steam Language...");
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Failed Loading Steam Language");
			LoadSystemLanguage();
			return;
		}
		string text = SteamApps.GetCurrentGameLanguage();
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogError("Failed Loading Steam Language");
			LoadSystemLanguage();
			return;
		}
		switch (text)
		{
		case "brazilian":
			text = "pt-BR";
			break;
		case "tchinese":
			text = "zh-hant";
			break;
		case "schinese":
			text = "zh-hans";
			break;
		case "koreana":
			text = "ko";
			break;
		}
		text = text.ToLower();
		Debug.Log("Steam Startup Language: " + text);
		bool flag = false;
		for (int i = 0; i < m_locales.Count; i++)
		{
			string text2 = m_locales[i].LocaleName.ToLower();
			VerboseDebug.Log("Checking Language: " + text2);
			if (text2.Contains(text))
			{
				base.Value = i;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogError("Failed Loading Steam Language: " + text);
			LoadSystemLanguage();
		}
	}

	public override void ApplyValue()
	{
		LocalizationSettings.SelectedLocale = m_locales[base.Value];
		LocalizationKeys.OnLanguageSwitch();
	}

	public override int GetDefaultValue()
	{
		return m_default;
	}

	public override List<string> GetChoices()
	{
		return m_choices;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Language";
	}
}
