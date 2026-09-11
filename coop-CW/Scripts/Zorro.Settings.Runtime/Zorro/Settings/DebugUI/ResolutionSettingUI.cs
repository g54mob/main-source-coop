using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	public class ResolutionSettingUI : SettingUI
	{
		private ResolutionSetting _setting;

		private ISettingHandler _handler;

		private List<string> _choices;

		public ResolutionSettingUI(ResolutionSetting setting, ISettingHandler settingHandler)
		{
			_setting = setting;
			_handler = settingHandler;
			SingletonAsset<SettingUxmls>.Instance.EnumSettingUxml.CloneTree(this);
			Label label = this.Q<Label>("SettingName");
			DropdownField dropdownField = this.Q<DropdownField>();
			label.text = setting.GetType().Name;
			_choices = setting.GetChoices();
			int currentChoice = setting.GetCurrentChoice();
			dropdownField.choices = _choices;
			dropdownField.SetValueWithoutNotify(dropdownField.choices[math.clamp(currentChoice, 0, dropdownField.choices.Count - 1)]);
			dropdownField.RegisterValueChangedCallback(Callback);
		}

		private void Callback(ChangeEvent<string> evt)
		{
			int num = FindIndex(evt.newValue);
			if (num >= 0)
			{
				Resolution newValue = _setting.GetResolutions()[num];
				_setting.SetValue(newValue, _handler);
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
