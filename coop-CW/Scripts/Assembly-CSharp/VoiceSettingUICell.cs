using System.Linq;
using TMPro;
using Zorro.Settings;

public class VoiceSettingUICell : SettingInputUICell
{
	public TMP_Dropdown dropdown;

	public VoiceSetting VoiceSetting;

	public ISettingHandler settingHandler;

	public override void Setup(Setting setting, ISettingHandler settingHandler)
	{
		this.settingHandler = settingHandler;
		if (setting is VoiceSetting voiceSetting)
		{
			VoiceSetting = voiceSetting;
			dropdown.options = (from s in voiceSetting.GetChoices()
				select new TMP_Dropdown.OptionData(s.name)).ToList();
			dropdown.SetValueWithoutNotify(voiceSetting.GetCurrentChoice());
			dropdown.onValueChanged.AddListener(Apply);
		}
	}

	private void Apply(int index)
	{
		VoiceSetting.MicrophoneInfo device = VoiceSetting.GetChoices()[index];
		VoiceSetting.SetValue(device, settingHandler);
	}
}
