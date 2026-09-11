using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.Settings;

public class VoiceChatModeSetting : EnumSetting, IExposedSetting
{
	private bool talking;

	private float lastInputTime;

	public override void ApplyValue()
	{
		talking = false;
	}

	public override int GetDefaultValue()
	{
		return 1;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Disabled);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PushToTalk);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ToggleToTalk);
		string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.VoiceDetection);
		return new List<string> { localizedString, localizedString2, localizedString3, localizedString4 };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Audio;
	}

	public string GetDisplayName()
	{
		return "Voice Chat Mode";
	}

	public bool CanTalk()
	{
		if (base.Value == 1)
		{
			if (!GlobalInputHandler.PushToTalkKey.GetKey())
			{
				if (Gamepad.current != null)
				{
					return Gamepad.current.buttonNorth.isPressed;
				}
				return false;
			}
			return true;
		}
		if (base.Value == 2)
		{
			if (GlobalInputHandler.PushToTalkKey.GetKeyDown() || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
			{
				float time = Time.time;
				if (time > lastInputTime + 0.01f)
				{
					talking = !talking;
				}
				lastInputTime = time;
			}
			return talking;
		}
		return base.Value != 0;
	}
}
