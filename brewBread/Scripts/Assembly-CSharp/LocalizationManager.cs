using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
	public void ChangeLocale(Locales local)
	{
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)local];
	}

	public Locale GetCurrentLocal()
	{
		return LocalizationSettings.SelectedLocale;
	}

	public string GetCurrentLocalName()
	{
		return LocalizationSettings.SelectedLocale.name;
	}
}
