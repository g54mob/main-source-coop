namespace Features.SettingsMenuModule.Scripts
{
	public interface IApplySettingsService
	{
		void ApplySettings();

		void ApplyMicrophoneEnabled();

		void ApplyNoiseSuppressionEnabled();

		void ApplyPushToTalkEnabled();

		void ApplyYAxisInvertEnabled();

		void ApplyMicrophoneSensitivity();

		void ApplyMicrophoneDevice();

		void ApplyEffectsVolume();

		void ApplyMusicVolume();

		void ApplyAllVolume();

		void ApplyBrightness();

		void ResetAllSettings();

		bool HasNotAppliedSettings();

		void ApplyVSyncEnabled();

		void ApplyQualityLevel();

		void ApplyMouseSensitivity();

		void ApplyScreenMode();

		void ApplyResolution();

		void ApplyFpsLimit();

		void ApplyScreenShakeIntensity();

		void ApplyHeadBobbingIntensity();

		void ApplyLanguage();
	}
}
