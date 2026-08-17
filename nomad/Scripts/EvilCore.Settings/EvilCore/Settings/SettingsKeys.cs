using EvilCore.GraphicsQuality;
using EvilCore.Managers;

namespace EvilCore.Settings
{
	public static class SettingsKeys
	{
		public const string DisplayMode = "settings.display.mode";

		public const string ResolutionWidth = "settings.display.resWidth";

		public const string ResolutionHeight = "settings.display.resHeight";

		public const string FpsLimit = "settings.display.fpsLimit";

		public const string VSync = "settings.display.vsync";

		public const string GraphicsLevel = "settings.graphics.level";

		public const string MasterVolume = "settings.audio.master";

		public const string MusicVolume = "settings.audio.music";

		public const string SfxVolume = "settings.audio.sfx";

		public const string AmbienceVolume = "settings.audio.ambience";

		public const string VoiceMuted = "settings.audio.voiceMuted";

		public const string InputDevice = "settings.audio.inputDevice";

		public const string OutputDevice = "settings.audio.outputDevice";

		public const string SensitivityX = "settings.gameplay.sensitivityX";

		public const string SensitivityY = "settings.gameplay.sensitivityY";

		public const string Fov = "settings.gameplay.fov";

		public const string InvertMouseY = "settings.gameplay.invertMouseY";

		public const string MotionBlur = "settings.gameplay.motionBlur";

		public const string Locale = "settings.language.locale";

		public const string Initialized = "settings.initialized";

		public const ResolutionType DefaultDisplayMode = ResolutionType.Fullscreen;

		public const int DefaultResolutionWidth = -1;

		public const int DefaultResolutionHeight = -1;

		public const FpsLimit DefaultFpsLimit = EvilCore.Managers.FpsLimit.Unlimited;

		public const bool DefaultVSync = false;

		public const GraphicsQualityLevel DefaultGraphicsLevel = GraphicsQualityLevel.High;

		public const float DefaultMasterVolume = 0.7f;

		public const float DefaultMusicVolume = 1f;

		public const float DefaultSfxVolume = 1f;

		public const float DefaultAmbienceVolume = 1f;

		public const bool DefaultVoiceMuted = false;

		public const float DefaultSensitivityX = 5f;

		public const float DefaultSensitivityY = 5f;

		public const float DefaultFov = 75f;

		public const bool DefaultInvertMouseY = false;

		public const bool DefaultMotionBlur = false;

		public const string DefaultLocale = "en";
	}
}
