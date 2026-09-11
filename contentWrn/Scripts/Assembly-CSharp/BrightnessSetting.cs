using Unity.Mathematics;
using Zorro.Settings;

public class BrightnessSetting : FloatSetting, IExposedSetting
{
	public static float MIN_POST_EXPOSURE = -0.6f;

	public static float MAX_POST_EXPOSURE = 0.6f;

	public override void ApplyValue()
	{
	}

	protected override float GetDefaultValue()
	{
		return 0f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(-1f, 1f);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Brightness";
	}

	public float GetGamma()
	{
		return math.remap(-1f, 1f, MIN_POST_EXPOSURE, MAX_POST_EXPOSURE, base.Value);
	}
}
