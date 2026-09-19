using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Features.SettingsMenuModule.Scripts
{
	public interface ISettingsService
	{
		void SetAllVolume(float value);

		void SetMusicVolume(float value);

		void SetEffectsVolume(float value);

		void SetMicrophone(string micName);

		void SetMicrophoneSensitivity(float value);

		void SetEnabledMicrophone(bool value);

		void SetNoiseSuppression(bool value);

		void SetPushToTalk(bool value);

		void SetYAxisInvert(bool value);

		void SetEnabledVSync(bool value);

		void SetQualityLevel(int qualityLevel);

		void SetMouseSensitivity(float mouseSensitivity);

		void SetFieldOfView(float fieldOfView);

		void SetBrightness(float brightness);

		void SetScreenMode(ScreenMode screenMode);

		void SetResolution(int x, int y);

		void SetFrameRateLimit(int result);

		void SetScreenShakeIntensity(float normalizedIntensity);

		void SetHeadBobbingIntensity(float normalizedIntensity);

		void SetLanguage(Language language);
	}
}
