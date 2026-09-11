using Unity.Mathematics;
using UnityEngine;
using Zorro.Settings;

public class MaxFramerateSetting : FloatSetting, IExposedSetting
{
	public MaxFramerateSetting()
	{
		base.SliderAmount = 10f;
	}

	public override void ApplyValue()
	{
		Application.targetFrameRate = Mathf.RoundToInt(base.Value);
	}

	protected override float GetDefaultValue()
	{
		return 200f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(30f, 480f);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Max Framerate";
	}

	public override float Clamp(float value)
	{
		return Mathf.RoundToInt(base.Clamp(value));
	}

	public override string Expose(float result)
	{
		return Mathf.RoundToInt(result).ToString();
	}
}
