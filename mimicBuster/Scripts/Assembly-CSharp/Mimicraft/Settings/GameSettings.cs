using System;
using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Settings
{
	public static class GameSettings
	{
		private const string Prefix = "Mimicraft.Settings.";

		private const string QualityLevelKey = "QualityLevel.v2";

		public const string DefaultLanguageId = "en";

		public const int MinHoverRadius = 0;

		public const int MaxHoverRadius = 5;

		public const float MinFieldOfView = 50f;

		public const float MaxFieldOfView = 110f;

		public const float DefaultFieldOfView = 70f;

		public const float MinSensitivity = 0.1f;

		public const float MaxSensitivity = 3f;

		private static bool loaded;

		public static int DisplayIndex { get; private set; }

		public static string AspectRatioId { get; private set; } = "";

		public static int ResolutionWidth { get; private set; }

		public static int ResolutionHeight { get; private set; }

		public static FullScreenMode WindowMode { get; private set; }

		public static int VSyncCount { get; private set; } = 1;

		public static int FrameRateLimit { get; private set; }

		public static int QualityLevel { get; private set; } = -1;

		public static float FieldOfView { get; private set; }

		public static float MasterVolume { get; private set; } = 1f;

		public static float SfxVolume { get; private set; } = 1f;

		public static float MusicVolume { get; private set; } = 1f;

		public static bool MuteWhenUnfocused { get; private set; } = true;

		public static bool VoiceEnabled { get; private set; } = true;

		public static float VoiceVolume { get; private set; } = 1f;

		public static string MicDevice { get; private set; } = "";

		public static float MicVolume { get; private set; } = 1f;

		public static float FpsSensitivity { get; private set; } = 1f;

		public static float TpsSensitivity { get; private set; } = 1f;

		public static float ScopeSensitivity { get; private set; } = 1f;

		public static bool InvertLookY { get; private set; }

		public static bool ToggleCrouch { get; private set; }

		public static bool ToggleScope { get; private set; }

		public static bool ToggleAds { get; private set; }

		public static bool DeveloperConsole { get; private set; }

		public static bool SafeChat { get; private set; } = true;

		public static bool AnalyticsConsent { get; private set; }

		public static float ScreenShake { get; private set; } = 1f;

		public static float EditorZoomSensitivity { get; private set; } = 1f;

		public static Color ExtrudeGizmoColor { get; private set; } = EditorPalette.DefaultExtrudeGizmo;

		public static Color TransformGizmoColor { get; private set; } = EditorPalette.DefaultTransformGizmo;

		public static Color EditorGridColor { get; private set; } = EditorPalette.DefaultGrid;

		public static bool EditorUnlit { get; private set; } = true;

		public static bool EditorShowDimensions { get; private set; }

		public static bool EditorShowGrid { get; private set; }

		public static bool EditorShowHistory { get; private set; } = true;

		public static bool EditorGlobalSpace { get; private set; }

		public static float EditorSnapMove { get; private set; } = 1f;

		public static float EditorSnapRotate { get; private set; } = 15f;

		public static int EditorHoverRadius { get; private set; } = 1;

		public static string LanguageId { get; private set; } = "en";

		public static bool HasSavedLanguage => PlayerPrefs.HasKey("Mimicraft.Settings.LanguageId");

		public static string InputBindings { get; private set; } = "";

		public static event Action Changed;

		public static void SetDisplayIndex(int value)
		{
			SetInt(value, DisplayIndex, "DisplayIndex", delegate(int v)
			{
				DisplayIndex = v;
			});
		}

		public static void SetAspectRatioId(string value)
		{
			SetString(value ?? "", AspectRatioId, "AspectRatioId", delegate(string v)
			{
				AspectRatioId = v;
			});
		}

		public static void SetResolution(int width, int height)
		{
			if (width != ResolutionWidth || height != ResolutionHeight)
			{
				ResolutionWidth = width;
				ResolutionHeight = height;
				PlayerPrefs.SetInt("Mimicraft.Settings.ResolutionWidth", width);
				PlayerPrefs.SetInt("Mimicraft.Settings.ResolutionHeight", height);
				PlayerPrefs.Save();
				Raise();
			}
		}

		public static void SetWindowMode(FullScreenMode value)
		{
			SetInt((int)value, (int)WindowMode, "WindowMode", delegate(int v)
			{
				WindowMode = (FullScreenMode)v;
			});
		}

		public static void SetMasterVolume(float value)
		{
			SetFloat(Mathf.Clamp01(value), MasterVolume, "MasterVolume", delegate(float v)
			{
				MasterVolume = v;
			});
		}

		public static void SetSfxVolume(float value)
		{
			SetFloat(Mathf.Clamp01(value), SfxVolume, "SfxVolume", delegate(float v)
			{
				SfxVolume = v;
			});
		}

		public static void SetMusicVolume(float value)
		{
			SetFloat(Mathf.Clamp01(value), MusicVolume, "MusicVolume", delegate(float v)
			{
				MusicVolume = v;
			});
		}

		public static void SetVSyncCount(int value)
		{
			SetInt(Mathf.Clamp(value, 0, 2), VSyncCount, "VSyncCount", delegate(int v)
			{
				VSyncCount = v;
			});
		}

		public static void SetFrameRateLimit(int value)
		{
			SetInt(Mathf.Max(0, value), FrameRateLimit, "FrameRateLimit", delegate(int v)
			{
				FrameRateLimit = v;
			});
		}

		public static void SetQualityLevel(int value)
		{
			SetInt(value, QualityLevel, "QualityLevel.v2", delegate(int v)
			{
				QualityLevel = v;
			});
		}

		public static void SetFieldOfView(float value)
		{
			SetFloat(Mathf.Clamp(value, 50f, 110f), FieldOfView, "FieldOfView", delegate(float v)
			{
				FieldOfView = v;
			});
		}

		public static void SetMuteWhenUnfocused(bool value)
		{
			SetBool(value, MuteWhenUnfocused, "MuteWhenUnfocused", delegate(bool v)
			{
				MuteWhenUnfocused = v;
			});
		}

		public static void SetVoiceEnabled(bool value)
		{
			SetBool(value, VoiceEnabled, "VoiceEnabled", delegate(bool v)
			{
				VoiceEnabled = v;
			});
		}

		public static void SetVoiceVolume(float value)
		{
			SetFloat(Mathf.Clamp01(value), VoiceVolume, "VoiceVolume", delegate(float v)
			{
				VoiceVolume = v;
			});
		}

		public static void SetMicDevice(string value)
		{
			SetString(value ?? "", MicDevice, "MicDevice", delegate(string v)
			{
				MicDevice = v;
			});
		}

		public static void SetMicVolume(float value)
		{
			SetFloat(Mathf.Clamp(value, 0f, 2f), MicVolume, "MicVolume", delegate(float v)
			{
				MicVolume = v;
			});
		}

		public static void SetFpsSensitivity(float value)
		{
			SetFloat(Sensitivity(value), FpsSensitivity, "FpsSensitivity", delegate(float v)
			{
				FpsSensitivity = v;
			});
		}

		public static void SetTpsSensitivity(float value)
		{
			SetFloat(Sensitivity(value), TpsSensitivity, "TpsSensitivity", delegate(float v)
			{
				TpsSensitivity = v;
			});
		}

		public static void SetScopeSensitivity(float value)
		{
			SetFloat(Sensitivity(value), ScopeSensitivity, "ScopeSensitivity", delegate(float v)
			{
				ScopeSensitivity = v;
			});
		}

		public static void SetInvertLookY(bool value)
		{
			SetBool(value, InvertLookY, "InvertLookY", delegate(bool v)
			{
				InvertLookY = v;
			});
		}

		public static void SetToggleCrouch(bool value)
		{
			SetBool(value, ToggleCrouch, "ToggleCrouch", delegate(bool v)
			{
				ToggleCrouch = v;
			});
		}

		public static void SetAnalyticsConsent(bool value)
		{
			SetBool(value, AnalyticsConsent, "AnalyticsConsent", delegate(bool v)
			{
				AnalyticsConsent = v;
			});
		}

		public static void SetSafeChat(bool value)
		{
			SetBool(value, SafeChat, "SafeChat", delegate(bool v)
			{
				SafeChat = v;
			});
		}

		public static void SetDeveloperConsole(bool value)
		{
			SetBool(value, DeveloperConsole, "DeveloperConsole", delegate(bool v)
			{
				DeveloperConsole = v;
			});
		}

		public static void SetToggleScope(bool value)
		{
			SetBool(value, ToggleScope, "ToggleScope", delegate(bool v)
			{
				ToggleScope = v;
			});
		}

		public static void SetToggleAds(bool value)
		{
			SetBool(value, ToggleAds, "ToggleAds", delegate(bool v)
			{
				ToggleAds = v;
			});
		}

		public static void SetScreenShake(float value)
		{
			SetFloat(Mathf.Clamp01(value), ScreenShake, "ScreenShake", delegate(float v)
			{
				ScreenShake = v;
			});
		}

		public static void SetEditorZoomSensitivity(float value)
		{
			SetFloat(Sensitivity(value), EditorZoomSensitivity, "EditorZoomSensitivity", delegate(float v)
			{
				EditorZoomSensitivity = v;
			});
		}

		public static void SetExtrudeGizmoColor(Color value)
		{
			SetColor(value, ExtrudeGizmoColor, "ExtrudeGizmoColor", delegate(Color v)
			{
				ExtrudeGizmoColor = v;
			});
		}

		public static void SetTransformGizmoColor(Color value)
		{
			SetColor(value, TransformGizmoColor, "TransformGizmoColor", delegate(Color v)
			{
				TransformGizmoColor = v;
			});
		}

		public static void SetEditorGridColor(Color value)
		{
			SetColor(value, EditorGridColor, "EditorGridColor", delegate(Color v)
			{
				EditorGridColor = v;
			});
		}

		public static void SetEditorUnlit(bool value)
		{
			SetBool(value, EditorUnlit, "EditorUnlit", delegate(bool v)
			{
				EditorUnlit = v;
			});
		}

		public static void SetEditorShowDimensions(bool value)
		{
			SetBool(value, EditorShowDimensions, "EditorShowDimensions", delegate(bool v)
			{
				EditorShowDimensions = v;
			});
		}

		public static void SetEditorShowGrid(bool value)
		{
			SetBool(value, EditorShowGrid, "EditorShowGrid", delegate(bool v)
			{
				EditorShowGrid = v;
			});
		}

		public static void SetEditorShowHistory(bool value)
		{
			SetBool(value, EditorShowHistory, "EditorShowHistory", delegate(bool v)
			{
				EditorShowHistory = v;
			});
		}

		public static void SetEditorGlobalSpace(bool value)
		{
			SetBool(value, EditorGlobalSpace, "EditorGlobalSpace", delegate(bool v)
			{
				EditorGlobalSpace = v;
			});
		}

		public static void SetEditorSnapMove(float value)
		{
			SetFloat(Mathf.Clamp(value, 0.01f, 16f), EditorSnapMove, "EditorSnapMove", delegate(float v)
			{
				EditorSnapMove = v;
			});
		}

		public static void SetEditorHoverRadius(int value)
		{
			SetInt(Mathf.Clamp(value, 0, 5), EditorHoverRadius, "EditorHoverRadius", delegate(int v)
			{
				EditorHoverRadius = v;
			});
		}

		public static void SetEditorSnapRotate(float value)
		{
			SetFloat(Mathf.Clamp(value, 1f, 90f), EditorSnapRotate, "EditorSnapRotate", delegate(float v)
			{
				EditorSnapRotate = v;
			});
		}

		public static void SetInputBindings(string value)
		{
			SetString(value ?? "", InputBindings, "InputBindings", delegate(string v)
			{
				InputBindings = v;
			});
		}

		public static void SetLanguageId(string value)
		{
			SetString(string.IsNullOrWhiteSpace(value) ? "en" : value, LanguageId, "LanguageId", delegate(string v)
			{
				LanguageId = v;
			});
		}

		public static void AdoptDetectedLanguage(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				LanguageId = value;
			}
		}

		public static void ForgetLanguage()
		{
			PlayerPrefs.DeleteKey("Mimicraft.Settings.LanguageId");
			PlayerPrefs.Save();
		}

		private static float Sensitivity(float value)
		{
			return Mathf.Clamp(value, 0.1f, 3f);
		}

		public static void Load()
		{
			if (!loaded)
			{
				loaded = true;
				DisplayIndex = PlayerPrefs.GetInt("Mimicraft.Settings.DisplayIndex", DisplayIndex);
				AspectRatioId = PlayerPrefs.GetString("Mimicraft.Settings.AspectRatioId", AspectRatioId);
				ResolutionWidth = PlayerPrefs.GetInt("Mimicraft.Settings.ResolutionWidth", ResolutionWidth);
				ResolutionHeight = PlayerPrefs.GetInt("Mimicraft.Settings.ResolutionHeight", ResolutionHeight);
				WindowMode = (FullScreenMode)PlayerPrefs.GetInt("Mimicraft.Settings.WindowMode", (int)Screen.fullScreenMode);
				VSyncCount = PlayerPrefs.GetInt("Mimicraft.Settings.VSyncCount", VSyncCount);
				FrameRateLimit = PlayerPrefs.GetInt("Mimicraft.Settings.FrameRateLimit", FrameRateLimit);
				QualityLevel = PlayerPrefs.GetInt("Mimicraft.Settings.QualityLevel.v2", QualityLevel);
				FieldOfView = PlayerPrefs.GetFloat("Mimicraft.Settings.FieldOfView", FieldOfView);
				MasterVolume = PlayerPrefs.GetFloat("Mimicraft.Settings.MasterVolume", MasterVolume);
				SfxVolume = PlayerPrefs.GetFloat("Mimicraft.Settings.SfxVolume", SfxVolume);
				MusicVolume = PlayerPrefs.GetFloat("Mimicraft.Settings.MusicVolume", MusicVolume);
				MuteWhenUnfocused = GetBool("MuteWhenUnfocused", MuteWhenUnfocused);
				VoiceEnabled = GetBool("VoiceEnabled", VoiceEnabled);
				VoiceVolume = PlayerPrefs.GetFloat("Mimicraft.Settings.VoiceVolume", VoiceVolume);
				MicVolume = PlayerPrefs.GetFloat("Mimicraft.Settings.MicVolume", MicVolume);
				MicDevice = PlayerPrefs.GetString("Mimicraft.Settings.MicDevice", MicDevice);
				FpsSensitivity = PlayerPrefs.GetFloat("Mimicraft.Settings.FpsSensitivity", FpsSensitivity);
				TpsSensitivity = PlayerPrefs.GetFloat("Mimicraft.Settings.TpsSensitivity", TpsSensitivity);
				ScopeSensitivity = PlayerPrefs.GetFloat("Mimicraft.Settings.ScopeSensitivity", ScopeSensitivity);
				InvertLookY = GetBool("InvertLookY", InvertLookY);
				AnalyticsConsent = GetBool("AnalyticsConsent", AnalyticsConsent);
				SafeChat = GetBool("SafeChat", SafeChat);
				ToggleCrouch = GetBool("ToggleCrouch", ToggleCrouch);
				ToggleScope = GetBool("ToggleScope", ToggleScope);
				ToggleAds = GetBool("ToggleAds", ToggleAds);
				DeveloperConsole = GetBool("DeveloperConsole", DeveloperConsole);
				ScreenShake = PlayerPrefs.GetFloat("Mimicraft.Settings.ScreenShake", ScreenShake);
				EditorZoomSensitivity = PlayerPrefs.GetFloat("Mimicraft.Settings.EditorZoomSensitivity", EditorZoomSensitivity);
				ExtrudeGizmoColor = GetColor("ExtrudeGizmoColor", ExtrudeGizmoColor);
				TransformGizmoColor = GetColor("TransformGizmoColor", TransformGizmoColor);
				EditorGridColor = GetColor("EditorGridColor", EditorGridColor);
				EditorUnlit = GetBool("EditorUnlit", EditorUnlit);
				EditorShowDimensions = GetBool("EditorShowDimensions", EditorShowDimensions);
				EditorShowGrid = GetBool("EditorShowGrid", EditorShowGrid);
				EditorShowHistory = GetBool("EditorShowHistory", EditorShowHistory);
				EditorGlobalSpace = GetBool("EditorGlobalSpace", EditorGlobalSpace);
				EditorSnapMove = PlayerPrefs.GetFloat("Mimicraft.Settings.EditorSnapMove", EditorSnapMove);
				EditorSnapRotate = PlayerPrefs.GetFloat("Mimicraft.Settings.EditorSnapRotate", EditorSnapRotate);
				EditorHoverRadius = PlayerPrefs.GetInt("Mimicraft.Settings.EditorHoverRadius", EditorHoverRadius);
				LanguageId = PlayerPrefs.GetString("Mimicraft.Settings.LanguageId", LanguageId);
				InputBindings = PlayerPrefs.GetString("Mimicraft.Settings.InputBindings", InputBindings);
			}
		}

		private static void SetInt(int value, int current, string key, Action<int> assign)
		{
			if (value != current)
			{
				assign(value);
				PlayerPrefs.SetInt("Mimicraft.Settings." + key, value);
				PlayerPrefs.Save();
				Raise();
			}
		}

		private static void SetString(string value, string current, string key, Action<string> assign)
		{
			if (!(value == current))
			{
				assign(value);
				PlayerPrefs.SetString("Mimicraft.Settings." + key, value);
				PlayerPrefs.Save();
				Raise();
			}
		}

		private static void SetFloat(float value, float current, string key, Action<float> assign)
		{
			if (!(Mathf.Abs(value - current) < 0.0005f))
			{
				assign(value);
				PlayerPrefs.SetFloat("Mimicraft.Settings." + key, value);
				PlayerPrefs.Save();
				Raise();
			}
		}

		private static void SetBool(bool value, bool current, string key, Action<bool> assign)
		{
			if (value != current)
			{
				assign(value);
				PlayerPrefs.SetInt("Mimicraft.Settings." + key, value ? 1 : 0);
				PlayerPrefs.Save();
				Raise();
			}
		}

		private static void SetColor(Color value, Color current, string key, Action<Color> assign)
		{
			if (!(value == current))
			{
				assign(value);
				PlayerPrefs.SetString("Mimicraft.Settings." + key, "#" + ColorUtility.ToHtmlStringRGBA(value));
				PlayerPrefs.Save();
				Raise();
			}
		}

		private static bool GetBool(string key, bool fallback)
		{
			return PlayerPrefs.GetInt("Mimicraft.Settings." + key, fallback ? 1 : 0) != 0;
		}

		private static Color GetColor(string key, Color fallback)
		{
			if (!ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Mimicraft.Settings." + key, ""), out var color))
			{
				return fallback;
			}
			return color;
		}

		private static void Raise()
		{
			GameSettings.Changed?.Invoke();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			loaded = false;
			GameSettings.Changed = null;
		}
	}
}
