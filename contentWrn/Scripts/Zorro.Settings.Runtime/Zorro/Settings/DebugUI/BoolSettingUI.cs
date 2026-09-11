using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class BoolSettingUI : SettingUI
	{
		private BoolSetting _setting;

		private ISettingHandler _handler;

		public BoolSettingUI(BoolSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.BoolSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			Toggle toggle = this.Q<Toggle>();
			label.text = setting.GetType().Name;
			toggle.SetValueWithoutNotify(setting.Value);
			toggle.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<bool> evt)
		{
			_setting.SetValue(evt.newValue, _handler);
		}
	}
}
