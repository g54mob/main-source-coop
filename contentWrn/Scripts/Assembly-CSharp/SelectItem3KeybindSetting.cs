using UnityEngine;
using Zorro.Settings;

public class SelectItem3KeybindSetting : KeyCodeSetting, IExposedSetting
{
	public override void ApplyValue()
	{
		base.ApplyValue();
		KeybindSettingUtility.RebindInputAction("SelectItem3", Keycode());
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.Alpha3;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Select Item 3";
	}
}
