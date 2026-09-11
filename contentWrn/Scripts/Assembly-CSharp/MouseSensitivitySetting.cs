using Unity.Mathematics;
using Zorro.Settings;

public class MouseSensitivitySetting : FloatSetting, IExposedSetting
{
	public override void ApplyValue()
	{
	}

	protected override float GetDefaultValue()
	{
		return 1f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(0.1f, 10f);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.MouseKeyboard;
	}

	public string GetDisplayName()
	{
		return "Mouse Sensitivity";
	}
}
