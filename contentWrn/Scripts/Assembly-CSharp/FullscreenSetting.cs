using System.Collections.Generic;
using UnityEngine.Device;
using Zorro.Settings;

public class FullscreenSetting : EnumSetting, IExposedSetting
{
	public override void ApplyValue()
	{
		if (base.Value == 0)
		{
			Screen.fullScreen = true;
		}
		else
		{
			Screen.fullScreen = false;
		}
	}

	public override int GetDefaultValue()
	{
		return 0;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.FullScreenMode);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.WindowedMode);
		return new List<string> { localizedString, localizedString2 };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Window mode";
	}
}
