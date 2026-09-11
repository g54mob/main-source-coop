using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.CLI;
using Zorro.Settings;

public class SettingsHandler : ISettingHandler, IDisposable
{
	private List<Setting> settings;

	private ISettingsSaveLoad _settingsSaveLoad;

	public SettingsHandler()
	{
		settings = new List<Setting>(30);
		_settingsSaveLoad = new DefaultSettingsSaveLoad();
		AddSetting(new LanguageSetting());
		AddSetting(new ScreenResolutionSetting());
		AddSetting(new FullscreenSetting());
		AddSetting(new VSyncSetting());
		AddSetting(new MaxFramerateSetting());
		AddSetting(new AmbientOcclusionSetting());
		AddSetting(new ChromaticAberrationSetting());
		AddSetting(new EdgeDetectionSetting());
		AddSetting(new WorldShaderSetting());
		AddSetting(new ShadowQualitySetting());
		AddSetting(new BrightnessSetting());
		AddSetting(new VoiceSetting());
		AddSetting(new VoiceChatModeSetting());
		AddSetting(new PushToTalkButtonSetting());
		AddSetting(new MouseSensitivitySetting());
		AddSetting(new ControllerSensitivitySetting());
		AddSetting(new WalkForwardKeybindSetting());
		AddSetting(new WalkBackwardKeybindSetting());
		AddSetting(new WalkLeftKeybindSetting());
		AddSetting(new WalkRightKeybindSetting());
		AddSetting(new SprintKeybindSetting());
		AddSetting(new JumpKeybindSetting());
		AddSetting(new CrouchKeybindSetting());
		AddSetting(new InteractKeybindSetting());
		AddSetting(new DropKeybindSetting());
		AddSetting(new EmoteKeybindSetting());
		AddSetting(new ToggleSelfieModeKeybindSetting());
		AddSetting(new SelectItem1KeybindSetting());
		AddSetting(new SelectItem2KeybindSetting());
		AddSetting(new SelectItem3KeybindSetting());
		AddSetting(new InvertedMouseSetting());
		AddSetting(new InvertedVerticalView());
		AddSetting(new InvertedHorizontalView());
		AddSetting(new MasterVolumeSetting(SingletonAsset<MixerHolder>.Instance.masterMixer));
		AddSetting(new SFXVolumeSetting(SingletonAsset<MixerHolder>.Instance.sfxMixer));
		AddSetting(new VoiceVolumeSetting(SingletonAsset<MixerHolder>.Instance.voiceMixer));
		AddSetting(new PreferredLanguageMatchmakingSetting());
		AddSetting(new SecondLanguageMatchmakingSetting());
		AddSetting(new ThirdLanguageMatchmakingSetting());
		if (PluginHandler.AllPlugins.Count == 0)
		{
			AddSetting(new CrossPlatformSetting());
		}
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			foreach (Type type in types)
			{
				if (!typeof(Setting).IsAssignableFrom(type) || type.GetCustomAttribute<ContentWarningSetting>() == null)
				{
					continue;
				}
				try
				{
					Setting setting = (Setting)Activator.CreateInstance(type);
					if (setting == null)
					{
						Debug.LogError($"Failed to load mod setting {type}: Activator.CreateInstance returned null");
					}
					else
					{
						AddSetting(setting);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					Debug.LogError($"Failed to load mod setting {type} due to the above exception");
				}
			}
		}
	}

	public void AddSetting(Setting setting)
	{
		settings.Add(setting);
		setting.Load(_settingsSaveLoad);
		setting.ApplyValue();
	}

	public void RegisterPage()
	{
		Singleton<DebugUIHandler>.Instance.RegisterPage("Settings", () => new SettingsPage(settings, this));
	}

	public void SaveSetting(Setting setting)
	{
		setting.Save(_settingsSaveLoad);
		_settingsSaveLoad.WriteToDisk();
	}

	public void Update()
	{
		foreach (Setting setting in settings)
		{
			setting.Update();
		}
	}

	public List<Setting> GetAllSettingsNonAlloc()
	{
		return settings;
	}

	public T GetSetting<T>() where T : Setting
	{
		foreach (Setting setting in settings)
		{
			if (setting is T result)
			{
				return result;
			}
		}
		return null;
	}

	public void Dispose()
	{
		foreach (Setting setting in settings)
		{
			setting.Dispose();
		}
	}

	public List<Setting> GetSettings(SettingCategory category)
	{
		List<Setting> list = new List<Setting>();
		foreach (Setting setting in settings)
		{
			if (setting is IExposedSetting exposedSetting && exposedSetting.GetSettingCategory() == category)
			{
				list.Add(setting);
			}
		}
		return list;
	}
}
