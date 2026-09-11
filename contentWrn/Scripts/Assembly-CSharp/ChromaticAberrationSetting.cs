using System.Collections.Generic;
using Zorro.Settings;

public class ChromaticAberrationSetting : EnumSetting, IExposedSetting
{
	public override void ApplyValue()
	{
	}

	public override int GetDefaultValue()
	{
		return 1;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.OnSetting);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.OffSetting);
		return new List<string> { localizedString2, localizedString };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Chromatic Aberration";
	}
}
