using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class HeadBobbingSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float> OnValueChanged;

		public abstract void SetHeadBobbingIntensity(float shakeIntensity);
	}
}
