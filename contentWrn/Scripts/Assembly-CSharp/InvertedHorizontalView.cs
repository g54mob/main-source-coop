using System.Collections.Generic;
using Zorro.Settings;

public class InvertedHorizontalView : EnumSetting, IExposedSetting
{
	public override void ApplyValue()
	{
	}

	public override int GetDefaultValue()
	{
		return 0;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.No);
		return new List<string> { localizedString2, localizedString };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Controller;
	}

	public string GetDisplayName()
	{
		return "Invert Look X";
	}

	public float GetFactor()
	{
		if (base.Value == 0)
		{
			return 1f;
		}
		return -1f;
	}
}
