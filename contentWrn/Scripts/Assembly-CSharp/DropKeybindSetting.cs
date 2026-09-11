using UnityEngine;
using Zorro.Settings;

public class DropKeybindSetting : KeyCodeSetting, IExposedSetting, IMKbPromptProvider
{
	public override void ApplyValue()
	{
		base.ApplyValue();
		KeybindSettingUtility.RebindInputAction("DropItem", Keycode());
	}

	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.Q;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Drop";
	}

	public string GetPrompt()
	{
		return "[" + Keycode().ToString() + "]";
	}
}
