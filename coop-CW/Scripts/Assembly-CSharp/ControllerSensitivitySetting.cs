using Unity.Mathematics;
using Zorro.Settings;

public class ControllerSensitivitySetting : FloatSetting, IExposedSetting
{
	public ControllerSensitivitySetting()
	{
		base.SliderAmount = 0.1f;
	}

	public override void ApplyValue()
	{
	}

	protected override float GetDefaultValue()
	{
		return 2f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(0.1f, 5f);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Controller;
	}

	public string GetDisplayName()
	{
		return "Controller Look Sensitivity";
	}
}
