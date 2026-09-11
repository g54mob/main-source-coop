using UnityEngine;
using Zorro.Settings;

public class WalkLeftKeybindSetting : KeyCodeSetting, IExposedSetting
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.A;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Walk Left";
	}
}
