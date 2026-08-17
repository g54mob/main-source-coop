using System;
using EvilCore.Audio;
using EvilCore.EvilSave;
using EvilCore.GraphicsQuality;
using EvilCore.Localization;
using EvilCore.Managers;
using EvilCore.Networking;
using UnityEngine;
using VContainer;

namespace EvilCore.Settings
{
	public class SettingsManager : MonoBehaviour, ISettingsManager
	{
		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IGraphicsQualityManager _graphicsQualityManager;

		[Inject]
		private IGameManager _gameManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IVoiceChatManager _voiceChatManager;

		[Tooltip("First-launch only: maps the player's hardware to a default graphics tier. If unassigned, falls back to DefaultGraphicsLevel.")]
		[SerializeField]
		private HardwareQualityConfig hardwareQualityConfig;

		public ResolutionType DisplayMode { get; private set; }

		public int ResolutionWidth { get; private set; }

		public int ResolutionHeight { get; private set; }

		public FpsLimit FpsLimit { get; private set; }

		public bool VSync { get; private set; }

		public GraphicsQualityLevel GraphicsLevel { get; private set; }

		public float MasterVolume { get; private set; }

		public float MusicVolume { get; private set; }

		public float SfxVolume { get; private set; }

		public float AmbienceVolume { get; private set; }

		public bool VoiceMuted { get; private set; }

		public string InputDeviceName { get; private set; }

		public string OutputDeviceName { get; private set; }

		public float SensitivityX { get; private set; }

		public float SensitivityY { get; private set; }

		public float Fov { get; private set; }

		public bool InvertMouseY { get; private set; }

		public bool MotionBlur { get; private set; }

		public string LocaleCode { get; private set; }

		public event Action OnSettingsChanged;

		private void Start()
		{
			LoadAndApplyAll();
			if (_graphicsQualityManager != null)
			{
				_graphicsQualityManager.OnQualityChanged += OnGraphicsQualityChanged;
			}
		}

		private void OnDestroy()
		{
			if (_graphicsQualityManager != null)
			{
				_graphicsQualityManager.OnQualityChanged -= OnGraphicsQualityChanged;
			}
		}

		private void OnGraphicsQualityChanged(GraphicsQualityLevel level)
		{
			ReassertVSyncAndFps();
		}

		public void LoadAndApplyAll()
		{
			LoadAllValues();
			ApplyAllValues();
		}

		public void ResetToDefaults()
		{
			(int width, int height) defaultResolution = GetDefaultResolution();
			int item = defaultResolution.width;
			int item2 = defaultResolution.height;
			DisplayMode = ResolutionType.Fullscreen;
			ResolutionWidth = item;
			ResolutionHeight = item2;
			FpsLimit = FpsLimit.Unlimited;
			VSync = false;
			GraphicsLevel = GraphicsQualityLevel.High;
			MasterVolume = 0.7f;
			MusicVolume = 1f;
			SfxVolume = 1f;
			AmbienceVolume = 1f;
			VoiceMuted = false;
			InputDeviceName = "";
			OutputDeviceName = "";
			SensitivityX = 5f;
			SensitivityY = 5f;
			Fov = 75f;
			InvertMouseY = false;
			MotionBlur = false;
			LocaleCode = "en";
			SaveAllValues();
			ApplyAllValues();
			this.OnSettingsChanged?.Invoke();
		}

		public void ResetDisplayToDefaults()
		{
			var (width, height) = GetDefaultResolution();
			SetDisplay(ResolutionType.Fullscreen, width, height);
			SetFpsLimit(FpsLimit.Unlimited);
			SetVSync(enabled: false);
		}

		public void ResetGraphicsToDefaults()
		{
			SetGraphicsLevel(GraphicsQualityLevel.High);
		}

		public void SetDisplay(ResolutionType mode, int width, int height)
		{
			DisplayMode = mode;
			ResolutionWidth = width;
			ResolutionHeight = height;
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.mode", (int)mode);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.resWidth", width);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.resHeight", height);
			_gameManager?.SetResolution(mode, width, height);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetFpsLimit(FpsLimit limit)
		{
			FpsLimit = limit;
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.fpsLimit", (int)limit);
			_gameManager?.SetFpsLimit(limit);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetVSync(bool enabled)
		{
			VSync = enabled;
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.display.vsync", enabled);
			QualitySettings.vSyncCount = (enabled ? 1 : 0);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetGraphicsLevel(GraphicsQualityLevel level)
		{
			GraphicsLevel = level;
			_graphicsQualityManager?.SetQuality(level);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.graphics.level", (int)level);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetMasterVolume(float volume01)
		{
			MasterVolume = volume01;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.master", volume01);
			_audioManager?.SetMasterVolume(volume01);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetMusicVolume(float volume01)
		{
			MusicVolume = volume01;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.music", volume01);
			_audioManager?.SetMusicVolume(volume01);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetSfxVolume(float volume01)
		{
			SfxVolume = volume01;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.sfx", volume01);
			_audioManager?.SetSfxVolume(volume01);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetAmbienceVolume(float volume01)
		{
			AmbienceVolume = volume01;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.ambience", volume01);
			_audioManager?.SetAmbienceVolume(volume01);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetVoiceMuted(bool muted)
		{
			VoiceMuted = muted;
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.audio.voiceMuted", muted);
			_voiceChatManager?.SetMuted(muted);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetInputDeviceName(string deviceName)
		{
			InputDeviceName = deviceName ?? "";
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.audio.inputDevice", InputDeviceName);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetOutputDeviceName(string deviceName)
		{
			OutputDeviceName = deviceName ?? "";
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.audio.outputDevice", OutputDeviceName);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetSensitivity(float x, float y)
		{
			SensitivityX = x;
			SensitivityY = y;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.sensitivityX", x);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.sensitivityY", y);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetFov(float fov)
		{
			Fov = fov;
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.fov", fov);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetInvertMouseY(bool inverted)
		{
			InvertMouseY = inverted;
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.gameplay.invertMouseY", inverted);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetMotionBlur(bool enabled)
		{
			MotionBlur = enabled;
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.gameplay.motionBlur", enabled);
			_graphicsQualityManager?.SetMotionBlurEnabled(enabled);
			this.OnSettingsChanged?.Invoke();
		}

		public void SetLocale(string code)
		{
			LocaleCode = code ?? "en";
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.language.locale", LocaleCode);
			_localizationService?.SetLocale(LocaleCode);
			this.OnSettingsChanged?.Invoke();
		}

		private (int width, int height) GetDefaultResolution()
		{
			int num = Display.main.systemWidth;
			int num2 = Display.main.systemHeight;
			if (num <= 0 || num2 <= 0)
			{
				Resolution currentResolution = Screen.currentResolution;
				num = currentResolution.width;
				num2 = currentResolution.height;
			}
			if (num <= 0 || num2 <= 0)
			{
				return (width: -1, height: -1);
			}
			return (width: num, height: num2);
		}

		private void LoadAllValues()
		{
			if (!EvilCore.EvilSave.EvilSave.Prefs.HasKey("settings.initialized"))
			{
				(int width, int height) defaultResolution = GetDefaultResolution();
				int item = defaultResolution.width;
				int item2 = defaultResolution.height;
				DisplayMode = ResolutionType.Fullscreen;
				ResolutionWidth = item;
				ResolutionHeight = item2;
				FpsLimit = FpsLimit.Unlimited;
				VSync = false;
				GraphicsLevel = ((hardwareQualityConfig != null) ? HardwareQualityDetector.DetectFromSystemInfo(hardwareQualityConfig) : GraphicsQualityLevel.High);
				MasterVolume = 0.7f;
				MusicVolume = 1f;
				SfxVolume = 1f;
				AmbienceVolume = 1f;
				VoiceMuted = false;
				InputDeviceName = "";
				OutputDeviceName = "";
				SensitivityX = 5f;
				SensitivityY = 5f;
				Fov = 75f;
				InvertMouseY = false;
				MotionBlur = false;
				LocaleCode = "en";
				SaveAllValues();
				return;
			}
			DisplayMode = (ResolutionType)EvilCore.EvilSave.EvilSave.Prefs.GetInt("settings.display.mode");
			ResolutionWidth = EvilCore.EvilSave.EvilSave.Prefs.GetInt("settings.display.resWidth", -1);
			ResolutionHeight = EvilCore.EvilSave.EvilSave.Prefs.GetInt("settings.display.resHeight", -1);
			FpsLimit = (FpsLimit)EvilCore.EvilSave.EvilSave.Prefs.GetInt("settings.display.fpsLimit", 4);
			VSync = EvilCore.EvilSave.EvilSave.Prefs.GetBool("settings.display.vsync");
			GraphicsLevel = (GraphicsQualityLevel)EvilCore.EvilSave.EvilSave.Prefs.GetInt("settings.graphics.level", 2);
			MasterVolume = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.audio.master", 0.7f);
			MusicVolume = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.audio.music", 1f);
			SfxVolume = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.audio.sfx", 1f);
			AmbienceVolume = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.audio.ambience", 1f);
			VoiceMuted = EvilCore.EvilSave.EvilSave.Prefs.GetBool("settings.audio.voiceMuted");
			InputDeviceName = EvilCore.EvilSave.EvilSave.Prefs.GetString("settings.audio.inputDevice");
			OutputDeviceName = EvilCore.EvilSave.EvilSave.Prefs.GetString("settings.audio.outputDevice");
			SensitivityX = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.gameplay.sensitivityX", 5f);
			SensitivityY = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.gameplay.sensitivityY", 5f);
			if (SensitivityX < 1f)
			{
				SensitivityX = 5f;
			}
			if (SensitivityY < 1f)
			{
				SensitivityY = 5f;
			}
			Fov = EvilCore.EvilSave.EvilSave.Prefs.GetFloat("settings.gameplay.fov", 75f);
			InvertMouseY = EvilCore.EvilSave.EvilSave.Prefs.GetBool("settings.gameplay.invertMouseY");
			MotionBlur = EvilCore.EvilSave.EvilSave.Prefs.GetBool("settings.gameplay.motionBlur");
			LocaleCode = EvilCore.EvilSave.EvilSave.Prefs.GetString("settings.language.locale", "en");
		}

		private void SaveAllValues()
		{
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.mode", (int)DisplayMode);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.resWidth", ResolutionWidth);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.resHeight", ResolutionHeight);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.display.fpsLimit", (int)FpsLimit);
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.display.vsync", VSync);
			EvilCore.EvilSave.EvilSave.Prefs.SetInt("settings.graphics.level", (int)GraphicsLevel);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.master", MasterVolume);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.music", MusicVolume);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.sfx", SfxVolume);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.audio.ambience", AmbienceVolume);
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.audio.voiceMuted", VoiceMuted);
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.audio.inputDevice", InputDeviceName ?? "");
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.audio.outputDevice", OutputDeviceName ?? "");
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.sensitivityX", SensitivityX);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.sensitivityY", SensitivityY);
			EvilCore.EvilSave.EvilSave.Prefs.SetFloat("settings.gameplay.fov", Fov);
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.gameplay.invertMouseY", InvertMouseY);
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.gameplay.motionBlur", MotionBlur);
			EvilCore.EvilSave.EvilSave.Prefs.SetString("settings.language.locale", LocaleCode ?? "en");
			EvilCore.EvilSave.EvilSave.Prefs.SetBool("settings.initialized", value: true);
		}

		private void ApplyAllValues()
		{
			_gameManager?.SetResolution(DisplayMode, ResolutionWidth, ResolutionHeight);
			_graphicsQualityManager?.SetQuality(GraphicsLevel);
			ReassertVSyncAndFps();
			_graphicsQualityManager?.SetMotionBlurEnabled(MotionBlur);
			_audioManager?.SetMasterVolume(MasterVolume);
			_audioManager?.SetMusicVolume(MusicVolume);
			_audioManager?.SetSfxVolume(SfxVolume);
			_audioManager?.SetAmbienceVolume(AmbienceVolume);
			_voiceChatManager?.SetMuted(VoiceMuted);
			_localizationService?.SetLocale(LocaleCode);
		}

		private void ReassertVSyncAndFps()
		{
			QualitySettings.vSyncCount = (VSync ? 1 : 0);
			_gameManager?.SetFpsLimit(FpsLimit);
		}
	}
}
