using Zorro.Settings;

public class ScreenResolutionSetting : ResolutionSetting, IExposedSetting
{
	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Resolution";
	}
}
