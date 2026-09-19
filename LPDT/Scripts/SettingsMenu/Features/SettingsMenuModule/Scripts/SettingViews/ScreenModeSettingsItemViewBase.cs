using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class ScreenModeSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<int> OnValueChanged;

		public abstract void SetScreenModeDropdownValues(List<string> screenModeValues);

		public abstract void SetScreenModeDropdownValue(int screenMode);
	}
}
