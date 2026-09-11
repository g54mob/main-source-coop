using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class FloatSettingUI : SettingUI
	{
		private FloatSetting _setting;

		private ISettingHandler _handler;

		public FloatSettingUI(FloatSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.FloatSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			FloatField floatField = this.Q<FloatField>();
			label.text = setting.GetType().Name;
			floatField.SetValueWithoutNotify(setting.Value);
			floatField.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<float> evt)
		{
			_setting.SetValue(evt.newValue, _handler);
		}
	}
}
