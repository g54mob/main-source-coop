using Unity.Mathematics;
using UnityEngine.Audio;
using Zorro.Settings;

public class SFXVolumeSetting : FloatSetting, IExposedSetting
{
	public AudioMixerGroup mixerGroup;

	public SFXVolumeSetting(AudioMixerGroup mixerGroup)
	{
		this.mixerGroup = mixerGroup;
	}

	protected override float GetDefaultValue()
	{
		return 1f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(0f, 1f);
	}

	public override void ApplyValue()
	{
		float value = math.log10(base.Value) * 20f;
		if (base.Value <= 0.01f)
		{
			value = -80f;
		}
		mixerGroup.audioMixer.SetFloat("SFXVolume", value);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Audio;
	}

	public string GetDisplayName()
	{
		return "SFX Volume";
	}

	public override void Update()
	{
		base.Update();
		ApplyValue();
	}
}
