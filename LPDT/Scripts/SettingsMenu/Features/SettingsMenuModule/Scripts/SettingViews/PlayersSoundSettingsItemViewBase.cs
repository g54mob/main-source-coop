using System;
using PlayerCustomization;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class PlayersSoundSettingsItemViewBase : SettingsItemViewBehaviour
	{
		public Action<float> OnValueChanged;

		public abstract Selectable PrioritySelectable { get; }

		public abstract void SetValue(float value);

		public abstract void SetPlayerInfo(PlayerCustomizationSlotData playerCustomization);
	}
}
