using System.Linq;
using TMPro;
using UnityEngine;

namespace Zorro.Settings.UI
{
	public class ResolutionSettingUI : SettingInputUICell
	{
		public TMP_Dropdown dropdown;

		public ResolutionSetting ResolutionSetting;

		public ISettingHandler settingHandler;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			this.settingHandler = settingHandler;
			if (setting is ResolutionSetting resolutionSetting)
			{
				ResolutionSetting = resolutionSetting;
				dropdown.options = (from s in resolutionSetting.GetChoices()
					select new TMP_Dropdown.OptionData(s)).ToList();
				dropdown.SetValueWithoutNotify(resolutionSetting.GetCurrentChoice());
				dropdown.onValueChanged.AddListener(Apply);
			}
		}

		private void Apply(int index)
		{
			Resolution newValue = ResolutionSetting.GetResolutions()[index];
			ResolutionSetting.SetValue(newValue, settingHandler);
		}
	}
}
