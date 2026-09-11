using System.Linq;
using TMPro;

namespace Zorro.Settings.UI
{
	public class EnumSettingUI : SettingInputUICell
	{
		public TMP_Dropdown dropdown;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			EnumSetting enumSetting = setting as EnumSetting;
			if (enumSetting == null)
			{
				return;
			}
			dropdown.options = (from s in enumSetting.GetChoices()
				select new TMP_Dropdown.OptionData(s)).ToList();
			dropdown.SetValueWithoutNotify(enumSetting.Value);
			dropdown.onValueChanged.AddListener(delegate(int i)
			{
				if (enumSetting.IsValidValue(i))
				{
					enumSetting.SetValue(i, settingHandler);
				}
				else
				{
					int defaultValue = enumSetting.GetDefaultValue();
					enumSetting.SetValue(defaultValue, settingHandler);
					dropdown.SetValueWithoutNotify(defaultValue);
				}
			});
		}
	}
}
