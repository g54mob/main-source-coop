using System;
using EvilCore.GraphicsQuality;
using EvilCore.Managers;

namespace EvilCore.Settings
{
	public interface ISettingsManager
	{
		ResolutionType DisplayMode { get; }

		int ResolutionWidth { get; }

		int ResolutionHeight { get; }

		FpsLimit FpsLimit { get; }

		bool VSync { get; }

		GraphicsQualityLevel GraphicsLevel { get; }

		float MasterVolume { get; }

		float MusicVolume { get; }

		float SfxVolume { get; }

		float AmbienceVolume { get; }

		bool VoiceMuted { get; }

		string InputDeviceName { get; }

		string OutputDeviceName { get; }

		float SensitivityX { get; }

		float SensitivityY { get; }

		float Fov { get; }

		bool InvertMouseY { get; }

		bool MotionBlur { get; }

		string LocaleCode { get; }

		event Action OnSettingsChanged;

		void SetDisplay(ResolutionType mode, int width, int height);

		void SetFpsLimit(FpsLimit limit);

		void SetVSync(bool enabled);

		void SetGraphicsLevel(GraphicsQualityLevel level);

		void SetMasterVolume(float volume01);

		void SetMusicVolume(float volume01);

		void SetSfxVolume(float volume01);

		void SetAmbienceVolume(float volume01);

		void SetVoiceMuted(bool muted);

		void SetInputDeviceName(string deviceName);

		void SetOutputDeviceName(string deviceName);

		void SetSensitivity(float x, float y);

		void SetFov(float fov);

		void SetInvertMouseY(bool inverted);

		void SetMotionBlur(bool enabled);

		void SetLocale(string code);

		void LoadAndApplyAll();

		void ResetToDefaults();

		void ResetDisplayToDefaults();

		void ResetGraphicsToDefaults();
	}
}
