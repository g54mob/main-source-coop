using UnityEngine;
using Zorro.Settings;

public class CrouchKeybindSetting : KeyCodeSetting, IExposedSetting
{
	public override void ApplyValue()
	{
		base.ApplyValue();
		KeybindSettingUtility.RebindInputAction("Crouch", Keycode());
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.LeftControl;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Crouch";
	}
}
