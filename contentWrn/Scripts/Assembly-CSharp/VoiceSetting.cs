using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;
using Zorro.Settings;
using Zorro.Settings.DebugUI;

public class VoiceSetting : Setting, IExposedSetting
{
	private struct XboxDeviceInfo
	{
		public string epid;

		public string desc;

		public string ulid;
	}

	public struct MicrophoneInfo
	{
		public string id;

		public string name;
	}

	public MicrophoneInfo Value;

	public int ChoiceCount
	{
		get
		{
			if (Microphone.devices == null)
			{
				return 0;
			}
			return Microphone.devices.Length;
		}
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Audio;
	}

	public string GetDisplayName()
	{
		return "Microphone";
	}

	public override void Load(ISettingsSaveLoad loader)
	{
		if (loader.TryGetString(GetType(), out var value))
		{
			List<MicrophoneInfo> choices = GetChoices();
			Value = choices.Find((MicrophoneInfo x) => x.id == value);
			if (string.IsNullOrEmpty(Value.id))
			{
				Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs. Value not found in choices.");
				Value = GetDefaultValue();
			}
		}
		else
		{
			Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
			Value = GetDefaultValue();
		}
	}

	private MicrophoneInfo GetDefaultValue()
	{
		if (GetChoices().Count == 0)
		{
			Debug.LogError("No voice devices found.");
			return default(MicrophoneInfo);
		}
		return GetChoices().First();
	}

	public override void Save(ISettingsSaveLoad saver)
	{
		saver.SaveString(GetType(), Value.id);
	}

	public override void ApplyValue()
	{
		Debug.Log("Voice setting applied: " + Value.id + " " + Value.name);
	}

	public override SettingUI GetDebugUI(ISettingHandler settingHandler)
	{
		return new VoiceSettingDebugUI(this, settingHandler);
	}

	public override GameObject GetSettingUICell()
	{
		return SingletonAsset<GameSpecificInputCellMapper>.Instance.VoiceCell;
	}

	public List<MicrophoneInfo> GetChoices()
	{
		string[] devices = Microphone.devices;
		List<MicrophoneInfo> list = new List<MicrophoneInfo>();
		string[] array = devices;
		foreach (string text in array)
		{
			list.Add(new MicrophoneInfo
			{
				id = text,
				name = text
			});
		}
		return list;
	}

	public int GetCurrentChoice()
	{
		return GetChoices().IndexOf(Value);
	}

	public void SetValue(MicrophoneInfo device, ISettingHandler settingHandler)
	{
		Value = device;
		ApplyValue();
		settingHandler.SaveSetting(this);
	}
}
