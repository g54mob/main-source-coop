using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class QualitySettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<int> OnValueChanged;

		public abstract void SetQualityLocalizationKeys(List<string> qualityLocalizationKeys);

		public abstract void SetQualityIndex(int index);
	}
}
