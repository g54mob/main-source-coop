using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class EnumSettingsUI : SettingUI
	{
		private EnumSetting _setting;

		private ISettingHandler _handler;

		private List<string> _choices;

		private DropdownField _field;

		public EnumSettingsUI(EnumSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.EnumSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			_field = this.Q<DropdownField>();
			label.text = setting.GetType().Name;
			_choices = setting.GetChoices();
			_field.choices = _choices;
			_field.SetValueWithoutNotify(_field.choices[math.clamp(setting.Value, 0, _field.choices.Count - 1)]);
			_field.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<string> evt)
		{
			int num = FindIndex(evt.newValue);
			if (num >= 0)
			{
				if (_setting.IsValidValue(num))
				{
					_setting.SetValue(num, _handler);
					return;
				}
				Debug.LogError("Invalid value for choice " + evt.newValue);
				int defaultValue = _setting.GetDefaultValue();
				_setting.SetValue(defaultValue, _handler);
				_field.SetValueWithoutNotify(_field.choices[math.clamp(defaultValue, 0, _field.choices.Count - 1)]);
			}
			else
			{
				Debug.LogError("Failed to find index for choice " + evt.newValue);
			}
			int FindIndex(string choice)
			{
				for (int i = 0; i < _choices.Count; i++)
				{
					if (_choices[i] == choice)
					{
						return i;
					}
				}
				return -1;
			}
		}
	}
}
