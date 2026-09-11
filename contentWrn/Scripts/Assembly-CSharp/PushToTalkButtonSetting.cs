using UnityEngine;
using Zorro.Settings;

public class PushToTalkButtonSetting : KeyCodeSetting, IExposedSetting, IMKbPromptProvider
{
	protected override KeyCode GetDefaultKey()
	{
		return KeyCode.V;
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Push To Talk";
	}

	public string GetPrompt()
	{
		return "[" + Keycode().ToString() + "]";
	}
}
