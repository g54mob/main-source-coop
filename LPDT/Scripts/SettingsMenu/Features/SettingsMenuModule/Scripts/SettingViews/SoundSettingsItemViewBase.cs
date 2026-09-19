using System;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class SoundSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float, SoundSettingsType> OnValueChanged;

		public abstract void SetValue(float value);

		public abstract SoundSettingsType GetSoundType();
	}
}
