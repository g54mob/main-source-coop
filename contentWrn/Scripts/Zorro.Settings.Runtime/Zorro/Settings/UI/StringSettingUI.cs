using TMPro;

namespace Zorro.Settings.UI
{
	public class StringSettingUI : SettingInputUICell
	{
		public TMP_InputField inputField;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			StringSetting stringSetting = setting as StringSetting;
			if (stringSetting != null)
			{
				inputField.SetTextWithoutNotify(stringSetting.Value);
				inputField.onValueChanged.AddListener(OnChanged);
			}
			void OnChanged(string str)
			{
				stringSetting.SetValue(str, settingHandler);
			}
		}
	}
}
