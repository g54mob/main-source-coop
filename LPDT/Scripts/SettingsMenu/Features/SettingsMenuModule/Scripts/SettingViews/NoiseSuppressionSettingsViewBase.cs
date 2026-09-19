using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class NoiseSuppressionSettingsViewBase : ViewBehaviour
	{
		public Action<bool> OnNoiseSuppressionEnabledChanged;

		public abstract void UpdateToggleVisual(bool noiseSuppressionEnabled);
	}
}
