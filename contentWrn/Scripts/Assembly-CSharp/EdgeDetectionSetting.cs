using System.Collections.Generic;
using Zorro.Settings;

public class EdgeDetectionSetting : EnumSetting
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
		return new List<string> { "OFF", "ON" };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}
}
