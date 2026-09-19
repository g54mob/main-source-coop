using System.Collections.Generic;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class MicrophoneDeviceSettingsItemView : MicrophoneDeviceSettingsItemViewBase
	{
		[SerializeField]
		private SettingsMultipleItems _microphoneDeviceSettings;

		protected override void OnEnable()
		{
			base.OnEnable();
			_microphoneDeviceSettings.OnValueChanged += OnValueChanged;
			UpdateMic();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_microphoneDeviceSettings.OnValueChanged -= OnValueChanged;
		}

		private void OnValueChanged(int index)
		{
			UpdateMic();
			OnMicrophoneChanged(CurrentItem());
		}

		public override void SetStartIndex(int index)
		{
			_microphoneDeviceSettings.SetIndex(index);
		}

		public override void SetMicrophoneSettingsOptions(List<string> values, string currentMicrophone)
		{
			_microphoneDeviceSettings.SetValues(values);
			if (values.Contains(currentMicrophone))
			{
				int index = values.IndexOf(currentMicrophone);
				_microphoneDeviceSettings.SetIndex(index);
			}
			else
			{
				_microphoneDeviceSettings.SetIndex(0);
			}
			_microphoneDeviceSettings.RefreshValues();
		}

		private void UpdateMic()
		{
			_microphoneDeviceSettings.Value.SetText(CurrentItem());
		}

		private string CurrentItem()
		{
			if (_microphoneDeviceSettings.Values.Count <= 0)
			{
				return string.Empty;
			}
			return _microphoneDeviceSettings.Values[_microphoneDeviceSettings.ValueIndex];
		}
	}
}
