using UnityEngine;
using Zorro.Settings;

public class WalkRightKeybindSetting : KeyCodeSetting, IExposedSetting
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.D;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Walk Right";
	}
}
