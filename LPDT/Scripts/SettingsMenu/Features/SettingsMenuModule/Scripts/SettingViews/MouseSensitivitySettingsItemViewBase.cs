using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class MouseSensitivitySettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float> OnSensitivityChanged;

		public abstract void UpdateSensitivity(float value);
	}
}
