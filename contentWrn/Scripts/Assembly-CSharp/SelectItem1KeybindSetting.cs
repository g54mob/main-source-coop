using UnityEngine;
using Zorro.Settings;

public class SelectItem1KeybindSetting : KeyCodeSetting, IExposedSetting
{
	public override void ApplyValue()
	{
		base.ApplyValue();
		KeybindSettingUtility.RebindInputAction("SelectItem1", Keycode());
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.Alpha1;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Select Item 1";
	}
}
