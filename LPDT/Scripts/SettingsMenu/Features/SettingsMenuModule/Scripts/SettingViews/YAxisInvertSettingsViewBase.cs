using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class YAxisInvertSettingsViewBase : ViewBehaviour
	{
		public Action<bool> OnYAxisInvertEnabledChanged;

		public abstract void UpdateToggleVisual(bool yAxisInvertEnabled);
	}
}
