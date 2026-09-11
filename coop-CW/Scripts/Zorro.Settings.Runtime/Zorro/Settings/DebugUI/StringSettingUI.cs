using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class StringSettingUI : SettingUI
	{
		private StringSetting _setting;

		private ISettingHandler _handler;

		public StringSettingUI(StringSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.StringSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			TextField textField = this.Q<TextField>();
			label.text = setting.GetType().Name;
			textField.SetValueWithoutNotify(setting.Value);
			textField.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<string> evt)
		{
			_setting.SetValue(evt.newValue, _handler);
		}
	}
}
