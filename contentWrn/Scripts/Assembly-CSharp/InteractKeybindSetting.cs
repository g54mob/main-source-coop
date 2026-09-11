using UnityEngine;
using Zorro.Settings;

public class InteractKeybindSetting : KeyCodeSetting, IExposedSetting, IMKbPromptProvider
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.E;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Interact";
	}

	public string GetPrompt()
	{
		return "[" + Keycode().ToString() + "]";
	}
}
