public class ThirdLanguageMatchmakingSetting : LanguageMatchmakingSetting, IExposedSetting
{
	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Matchmaking;
	}

	public string GetDisplayName()
	{
		return "Additional Language";
	}
}
