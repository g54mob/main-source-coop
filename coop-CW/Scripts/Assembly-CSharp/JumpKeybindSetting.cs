using UnityEngine;
using Zorro.Settings;

public class JumpKeybindSetting : KeyCodeSetting, IExposedSetting
{
	public override void ApplyValue()
	{
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.Space;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Jump";
	}
}
