using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class IntSettingUI : SettingUI
	{
		private IntSetting _setting;

		private ISettingHandler _handler;

		public IntSettingUI(IntSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.IntSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			IntegerField integerField = this.Q<IntegerField>();
			label.text = setting.GetType().Name;
			integerField.SetValueWithoutNotify(setting.Value);
			integerField.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<int> evt)
		{
			_setting.SetValue(evt.newValue, _handler);
		}
	}
}
