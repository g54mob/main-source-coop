using UnityEngine.UI;

namespace Zorro.Settings.UI
{
	public class BoolSettingUI : SettingInputUICell
	{
		public Toggle toggle;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			BoolSetting boolSetting = setting as BoolSetting;
			if (boolSetting != null)
			{
				toggle.SetIsOnWithoutNotify(boolSetting.Value);
				toggle.onValueChanged.AddListener(OnChanged);
			}
			void OnChanged(bool v)
			{
				boolSetting.SetValue(v, settingHandler);
			}
		}
	}
}
