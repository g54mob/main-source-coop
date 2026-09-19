using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class SensitivitySettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float> OnMicrophoneSensitivityChanged;

		public abstract void UpdateMicrophoneSensitivity(float value);
	}
}
