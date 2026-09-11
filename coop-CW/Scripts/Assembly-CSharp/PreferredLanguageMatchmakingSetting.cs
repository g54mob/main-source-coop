public class PreferredLanguageMatchmakingSetting : LanguageMatchmakingSetting, IExposedSetting
{
	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Matchmaking;
	}

	public string GetDisplayName()
	{
		return "Preferred Language";
	}

	public override int GetDefaultValue()
	{
		return 9;
	}

	public override bool IsValidValue(int index)
	{
		return index != 0;
	}
}
