using Unity.Mathematics;
using UnityEngine.Audio;
using Zorro.Settings;

public class VoiceVolumeSetting : FloatSetting, IExposedSetting
{
	public AudioMixerGroup mixerGroup;

	public VoiceVolumeSetting(AudioMixerGroup mixerGroup)
	{
		this.mixerGroup = mixerGroup;
	}

	public override void ApplyValue()
	{
		float value = math.log10(base.Value) * 20f;
		if (base.Value <= 0.01f)
		{
			value = -80f;
		}
		mixerGroup.audioMixer.SetFloat("VoiceVolume", value);
	}

	protected override float GetDefaultValue()
	{
		return 1f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(0f, 1f);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Audio;
	}

	public override void Update()
	{
		base.Update();
		ApplyValue();
	}

	public string GetDisplayName()
	{
		return "Voice Volume";
	}
}
