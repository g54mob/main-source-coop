namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("EOS Config", new string[] { "iOS-Specific Options", "Deployment", "Flags", "Tick Budgets", "Overlay Options" }, false)]
	public class IOSConfig : PlatformConfig
	{
		static IOSConfig()
		{
			Config.RegisterFactory(() => new IOSConfig());
		}

		protected IOSConfig()
			: base(PlatformManager.Platform.iOS)
		{
		}
	}
}
