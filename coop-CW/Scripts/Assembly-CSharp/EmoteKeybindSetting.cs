using UnityEngine;
using Zorro.Settings;

public class EmoteKeybindSetting : KeyCodeSetting, IExposedSetting
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.T;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Emote";
	}
}
