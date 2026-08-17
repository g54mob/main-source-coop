namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("MacOS Config", new string[] { "MacOS-Specific Options", "Deployment", "Flags", "Tick Budgets", "Overlay Options" }, false)]
	public class MacOSConfig : PlatformConfig
	{
		static MacOSConfig()
		{
			Config.RegisterFactory(() => new MacOSConfig());
		}

		protected MacOSConfig()
			: base(PlatformManager.Platform.macOS)
		{
		}
	}
}
