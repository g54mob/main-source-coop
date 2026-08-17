namespace PlayEveryWare.EpicOnlineServices
{
	[ConfigGroup("Android Config", new string[] { "Android-Specific Options", "Deployment", "Flags", "Tick Budgets", "Overlay Options" }, false)]
	public class AndroidConfig : PlatformConfig
	{
		[ConfigField("Google Login Client ID", ConfigFieldType.Text, "Get your project's Google Login Client ID from the Google Cloud dashboard", 0, "https://console.cloud.google.com/apis/dashboard")]
		public string GoogleLoginClientID;

		[ConfigField("Google Login Nonce", ConfigFieldType.Text, "Use a nonce to improve sign-ing security on Android.", 0, "https://developer.android.com/google/play/integrity/classic#nonce")]
		public string GoogleLoginNonce;

		static AndroidConfig()
		{
			Config.RegisterFactory(() => new AndroidConfig());
		}

		protected AndroidConfig()
			: base(PlatformManager.Platform.Android)
		{
		}
	}
}
