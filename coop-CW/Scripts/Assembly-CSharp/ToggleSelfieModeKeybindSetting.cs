using UnityEngine;
using Zorro.Settings;

public class ToggleSelfieModeKeybindSetting : KeyCodeSetting, IExposedSetting, IMKbPromptProvider
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.R;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Toggle Selfie Mode";
	}

	public string GetPrompt()
	{
		return "[" + Keycode().ToString() + "]";
	}
}
