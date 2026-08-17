using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UIControllers
{
	public class OptionsController : UIController
	{
		[Header("Button")]
		public Toggle SpeedrunToggle;

		public GameObject ControlSettings;

		[Header("Resolution")]
		public List<Vector2Int> resolutionValues;

		public MultipleSelector resolution;

		public MultipleSelector languages;

		[Header("Audio Volume")]
		public AudioMixer mixer;

		public TextMeshProUGUI musicVolume;

		public Slider musicSlider;

		public TextMeshProUGUI fxVolume;

		public Slider FXSlider;

		public AudioClip clip;

		[Header("Video")]
		public Toggle fullScreenToggle;

		public Toggle vSyncToggle;

		private int _vSync;

		private bool FullScreen
		{
			get
			{
				return StaticInstance<DataBetweenScenes>.Instance.data.fullscreen;
			}
			set
			{
				StaticInstance<DataBetweenScenes>.Instance.data.fullscreen = value;
			}
		}

		private void Awake()
		{
			SetResolutionStrings();
			StartCoroutine(SetLocaleStrings());
		}

		public override void Start()
		{
			base.Start();
			resolution.SetBoundaries(-2, resolutionValues.Count);
			StartCoroutine(WaitForLoadOptions());
		}

		private IEnumerator WaitForLoadOptions()
		{
			yield return new WaitForEndOfFrame();
			LoadOptions();
		}

		private void SaveOptions()
		{
			PlayerPrefs.SetInt("SpeedrunMode", Convert.ToInt32(StaticInstance<DataBetweenScenes>.Instance.data.speedRunMode));
			PlayerPrefs.SetInt("Language", StaticInstance<DataBetweenScenes>.Instance.data.Language);
			PlayerPrefs.SetFloat("Music", StaticInstance<DataBetweenScenes>.Instance.data.musicVolume);
			PlayerPrefs.SetFloat("FX", StaticInstance<DataBetweenScenes>.Instance.data.fxVolume);
			PlayerPrefs.SetInt("Resolution", resolution.Value);
			PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(FullScreen));
			PlayerPrefs.SetInt("VSync", Convert.ToInt32(_vSync));
			PlayerPrefs.Save();
		}

		private void LoadOptions()
		{
			if (!(StaticInstance<DataBetweenScenes>.Instance == null))
			{
				SpeedrunToggle.SetIsOnWithoutNotify(StaticInstance<DataBetweenScenes>.Instance.data.speedRunMode);
				float value = StaticInstance<DataBetweenScenes>.Instance.data.fxVolume;
				FXSlider.value = value;
				float value2 = StaticInstance<DataBetweenScenes>.Instance.data.musicVolume;
				musicSlider.value = value2;
				resolution.Value = StaticInstance<DataBetweenScenes>.Instance.data.resolution;
				fullScreenToggle.SetIsOnWithoutNotify(FullScreen);
				_vSync = StaticInstance<DataBetweenScenes>.Instance.data.vsync;
				vSyncToggle.SetIsOnWithoutNotify(Convert.ToBoolean(_vSync));
			}
		}

		public void ToggleSpeedrunMode()
		{
		}

		public void ToggleFullScreen()
		{
			if (FullScreen)
			{
				Screen.fullScreenMode = FullScreenMode.Windowed;
				FullScreen = false;
			}
			else
			{
				Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
				FullScreen = true;
			}
		}

		public void ToggleVsync()
		{
			if (_vSync == 0)
			{
				_vSync = 1;
			}
			else
			{
				_vSync = 0;
			}
			QualitySettings.vSyncCount = _vSync;
			StaticInstance<DataBetweenScenes>.Instance.data.vsync = _vSync;
		}

		private void SetResolutionStrings()
		{
			for (int i = 0; i < resolutionValues.Count; i++)
			{
				resolution.optionsList.Add(resolutionValues[i].x + "x" + resolutionValues[i].y);
			}
		}

		private IEnumerator SetLocaleStrings()
		{
			yield return LocalizationSettings.InitializationOperation;
			if (LocalizationSettings.InitializationOperation.IsDone)
			{
				for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
				{
					languages.optionsList.Add(LocalizationSettings.AvailableLocales.Locales[i].LocaleName);
				}
			}
			languages.SetBoundaries(-2, languages.optionsList.Count);
			languages.Value = StaticInstance<DataBetweenScenes>.Instance.data.Language;
		}

		public void SetResolution()
		{
			Vector2Int vector2Int = resolutionValues[resolution.Value];
			Screen.SetResolution(vector2Int.x, vector2Int.y, FullScreen);
			StaticInstance<DataBetweenScenes>.Instance.data.resolution = resolution.Value;
		}

		public void SetRumbleValue(float frequency)
		{
			StaticInstance<DataBetweenScenes>.Instance.rumbleFrequency = frequency;
		}

		public void SetLocale()
		{
			int value = languages.Value;
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
			StaticInstance<DataBetweenScenes>.Instance.data.Language = languages.Value;
		}

		public void SetFXVolume(float value)
		{
			mixer.SetFloat("fxVolume", Mathf.Log10(value) * 20f);
			SetVolumeText(fxVolume, value);
			StaticInstance<DataBetweenScenes>.Instance.data.fxVolume = value;
		}

		public void SetMusicVolume(float value)
		{
			mixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
			SetVolumeText(musicVolume, value);
			StaticInstance<DataBetweenScenes>.Instance.data.musicVolume = value;
		}

		public void SetVolumeText(TextMeshProUGUI tmp, float value)
		{
			tmp.text = ((int)(value * 100f)).ToString();
		}

		public override void Exit()
		{
			base.Exit();
			if (!SkipExit())
			{
				SaveOptions();
				StaticInstance<TransitionSystem>.Instance.UnloadScene(Scenes.Options);
			}
		}
	}
}
