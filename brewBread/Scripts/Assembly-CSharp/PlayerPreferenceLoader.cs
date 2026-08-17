using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Localization.Settings;

public class PlayerPreferenceLoader : StaticInstance<PlayerPreferenceLoader>
{
	public AudioMixer mixer;

	public Vector2Int[] resolutions;

	public InputActionAsset[] inputActions;

	public DataBetweenScenes data;

	public OptionData LoadPlayerPrefs()
	{
		bool speedRunMode = Convert.ToBoolean(PlayerPrefs.GetInt("SpeedrunMode", 0));
		int num = 0;
		if (PlayerPrefs.HasKey("Language"))
		{
			num = PlayerPrefs.GetInt("Language");
			StartCoroutine(SetLocale(num));
		}
		else
		{
			StartCoroutine(SetLocaleIndex());
		}
		float num2 = PlayerPrefs.GetFloat("FX", 1f);
		float num3 = PlayerPrefs.GetFloat("Music", 1f);
		StartCoroutine(SetVolume(num2, num3));
		bool fullscreen = (Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1)));
		int num4 = PlayerPrefs.GetInt("Resolution", 0);
		Screen.SetResolution(resolutions[num4].x, resolutions[num4].y, fullscreen);
		int vsync = (QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync", 1));
		return new OptionData(speedRunMode, num, num2, num3, num4, fullscreen, vsync);
	}

	public void LoadInputs()
	{
		for (int i = 0; i < inputActions.Length; i++)
		{
			foreach (InputActionMap actionMap in inputActions[i].actionMaps)
			{
				ReadOnlyArray<InputBinding> bindings = actionMap.bindings;
				for (int j = 0; j < bindings.Count; j++)
				{
					string text = PlayerPrefs.GetString(bindings[j].id.ToString(), null);
					if (!string.IsNullOrEmpty(text))
					{
						actionMap.ApplyBindingOverride(j, new InputBinding
						{
							overridePath = text
						});
					}
				}
			}
		}
	}

	private IEnumerator SetLocale(int index)
	{
		yield return LocalizationSettings.InitializationOperation;
		if (LocalizationSettings.InitializationOperation.IsDone)
		{
			yield return new WaitForEndOfFrame();
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
		}
	}

	private IEnumerator SetLocaleIndex()
	{
		yield return LocalizationSettings.InitializationOperation;
		if (!LocalizationSettings.InitializationOperation.IsDone)
		{
			yield break;
		}
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
		{
			if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
			{
				data.data.Language = i;
			}
		}
	}

	private IEnumerator SetVolume(float fxValue, float musicValue)
	{
		yield return new WaitForEndOfFrame();
		mixer.SetFloat("fxVolume", Mathf.Log10(fxValue) * 20f);
		mixer.SetFloat("musicVolume", Mathf.Log10(musicValue) * 20f);
	}
}
