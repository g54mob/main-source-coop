using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ScreenModeSettingsItemVew : ScreenModeSettingsItemViewBase
	{
		[SerializeField]
		private SettingsMultipleItems _screenModeSettings;

		private List<string> _screenModeValues = new List<string>();

		protected override void OnEnable()
		{
			base.OnEnable();
			_screenModeSettings.OnValueChanged += OnValueChangedHandler;
			UpdateLocalization();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_screenModeSettings.OnValueChanged -= OnValueChangedHandler;
		}

		private void UpdateLocalization()
		{
			if (_screenModeValues.Any())
			{
				_screenModeSettings.Value.SetText(_screenModeValues[_screenModeSettings.ValueIndex]);
			}
		}

		private void OnValueChangedHandler(int index)
		{
			OnValueChanged?.Invoke(index);
			UpdateLocalization();
		}

		public override void SetScreenModeDropdownValues(List<string> screenModeValues)
		{
			_screenModeValues = screenModeValues;
			_screenModeSettings.SetValues(_screenModeValues);
			UpdateLocalization();
		}

		public override void SetScreenModeDropdownValue(int screenMode)
		{
			_screenModeSettings.SetIndex(screenMode);
			UpdateLocalization();
		}
	}
}
