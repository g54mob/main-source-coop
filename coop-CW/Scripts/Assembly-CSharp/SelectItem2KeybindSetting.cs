using UnityEngine;
using Zorro.Settings;

public class SelectItem2KeybindSetting : KeyCodeSetting, IExposedSetting
{
	public override void ApplyValue()
	{
		base.ApplyValue();
		KeybindSettingUtility.RebindInputAction("SelectItem2", Keycode());
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.Alpha2;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Select Item 2";
	}
}
