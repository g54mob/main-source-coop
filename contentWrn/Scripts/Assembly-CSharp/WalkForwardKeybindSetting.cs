using UnityEngine;
using Zorro.Settings;

public class WalkForwardKeybindSetting : KeyCodeSetting, IExposedSetting
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.W;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Walk Forward";
	}
}
