using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class LocalizationSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<int> OnValueChanged;

		public abstract void SetValueIndex(int index);
	}
}
