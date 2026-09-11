using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.Core;
using Zorro.Settings;

public class SettingsCell : MonoBehaviour
{
	public Transform inputContainer;

	public TextMeshProUGUI title;

	public void Setup(Setting setting, ISettingHandler settingHandler)
	{
		string text = "Error";
		if (setting is IExposedSetting exposedSetting)
		{
			string value = ((IExposedSetting)setting).GetType().ToString();
			text = ((!Enum.TryParse(typeof(LocalizationKeys.Keys), value, out var _)) ? exposedSetting.GetDisplayName() : LocalizationKeys.GetLocalizedString((LocalizationKeys.Keys)Enum.Parse(typeof(LocalizationKeys.Keys), value)));
		}
		title.SetText((setting is IExposedSetting) ? text : setting.GetType().Name);
		UnityEngine.Object.Instantiate(setting.GetSettingUICell(), inputContainer).GetComponent<SettingInputUICell>().Setup(setting, settingHandler);
	}

	public GameObject GetSelection()
	{
		if (inputContainer.TryGetComponentInChildren<TMP_Dropdown>(out var component))
		{
			return component.gameObject;
		}
		if (inputContainer.TryGetComponentInChildren<Slider>(out var component2))
		{
			return component2.gameObject;
		}
		if (inputContainer.TryGetComponentInChildren<Button>(out var component3))
		{
			return component3.gameObject;
		}
		return null;
	}
}
