using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleDropdown : MonoBehaviour
{
	public TMP_Dropdown dropdown;

	private IEnumerator Start()
	{
		yield return LocalizationSettings.InitializationOperation;
		List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
		int value = 0;
		for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
		{
			Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
			if (LocalizationSettings.SelectedLocale == locale)
			{
				value = i;
			}
			list.Add(new TMP_Dropdown.OptionData(locale.name));
		}
		dropdown.options = list;
		dropdown.value = value;
		dropdown.onValueChanged.AddListener(LocaleSelected);
	}

	private static void LocaleSelected(int index)
	{
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
		LocalizationKeys.OnLanguageSwitch();
	}
}
