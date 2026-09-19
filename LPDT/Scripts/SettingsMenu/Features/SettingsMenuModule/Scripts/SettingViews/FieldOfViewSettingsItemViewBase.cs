using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class FieldOfViewSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float> OnFieldOfViewChanged;

		public abstract void UpdateFieldOfView(float value);
	}
}
