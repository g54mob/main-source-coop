namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("Windows Config", new string[] { "Windows-Specific Options", "Deployment", "Flags", "Tick Budgets", "Overlay Options" }, false)]
	public class WindowsConfig : PlatformConfig
	{
		static WindowsConfig()
		{
			Config.RegisterFactory(() => new WindowsConfig());
		}

		protected WindowsConfig()
			: base(PlatformManager.Platform.Windows)
		{
		}
	}
}
