using TMPro;

namespace Zorro.Settings.UI
{
	public class IntSettingUI : SettingInputUICell
	{
		public TMP_InputField inputField;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			IntSetting intSetting = setting as IntSetting;
			if (intSetting != null)
			{
				inputField.SetTextWithoutNotify(intSetting.Expose(intSetting.Value));
				inputField.onValueChanged.AddListener(OnChanged);
			}
			void OnChanged(string str)
			{
				if (int.TryParse(str, out var result))
				{
					inputField.SetTextWithoutNotify(intSetting.Expose(result));
					intSetting.SetValue(result, settingHandler);
				}
			}
		}
	}
}
