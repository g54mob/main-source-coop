using UnityEngine;
using Zorro.Settings;

public class SprintKeybindSetting : KeyCodeSetting, IExposedSetting
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.LeftShift;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Sprint";
	}
}
