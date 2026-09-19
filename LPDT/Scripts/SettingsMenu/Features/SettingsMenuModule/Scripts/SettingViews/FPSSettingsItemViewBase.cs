using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class FPSSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<int> OnValueChanged;

		public abstract void SetValueIndex(int index);

		public abstract void SetFpsValues(List<string> fpsValues);

		public abstract void SetFpsLimitText(int value);
	}
}
