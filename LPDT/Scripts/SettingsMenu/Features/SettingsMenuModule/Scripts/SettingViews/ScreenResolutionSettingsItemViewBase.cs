using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class ScreenResolutionSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<Vector2Int> OnValueChanged;

		public abstract void SetResolutionDropdownValues(List<Vector2Int> allPossibleResolutions);

		public abstract void SetDropdownResolutionValue(int currentResolutionX, int currentResolutionY);

		public abstract void SetDropdownResolutionValueIndex(int index);
	}
}
