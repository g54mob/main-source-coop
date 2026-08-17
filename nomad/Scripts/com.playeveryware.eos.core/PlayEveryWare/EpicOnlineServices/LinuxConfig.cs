namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("Linux Config", new string[] { "Linux-Specific Options", "Deployment", "Flags", "Tick Budgets", "Overlay Options" }, false)]
	public class LinuxConfig : PlatformConfig
	{
		static LinuxConfig()
		{
			Config.RegisterFactory(() => new LinuxConfig());
		}

		protected LinuxConfig()
			: base(PlatformManager.Platform.Linux)
		{
		}
	}
}
