using Steamworks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSelectStartup : MonoBehaviour
{
	private static bool m_Inited;

	private void Start()
	{
		if (m_Inited)
		{
			return;
		}
		LocalizationSettings.Instance.OnSelectedLocaleChanged += OnLocaleChanged;
		string text = "en";
		if (SteamManager.Initialized)
		{
			text = SteamApps.GetCurrentGameLanguage();
			if (string.IsNullOrEmpty(text))
			{
				text = "en";
				Debug.Log("Steam Language is empty, defaulting to English");
			}
			else
			{
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
			}
			text = text.ToLower();
			Debug.Log("Startup Steam Language: " + text);
			Locale locale = null;
			foreach (Locale locale2 in LocalizationSettings.AvailableLocales.Locales)
			{
				string text2 = locale2.LocaleName.ToLower();
				VerboseDebug.Log("Current langs: " + text2);
				if (text2.Contains(text))
				{
					locale = locale2;
					break;
				}
			}
			if (locale != null)
			{
				LocalizationSettings.SelectedLocale = locale;
				Debug.Log("Changed Unity Language to: " + locale);
				LocalizationKeys.MakeLocaleStrings();
			}
			else
			{
				Debug.LogError("Language Null: Current" + LocalizationSettings.SelectedLocale.ToString());
			}
		}
		m_Inited = true;
	}

	private void OnLocaleChanged(Locale obj)
	{
		LocalizationKeys.OnLanguageSwitch();
	}
}
