using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class YAxisInvertSettingsView : YAxisInvertSettingsViewBase
	{
		[SerializeField]
		private Toggle _enabledToggle;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnYAxisInvertEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnYAxisInvertEnabledChanged);
		}

		private void InvokeOnYAxisInvertEnabledChanged(bool yAxisInvertEnabled)
		{
			OnYAxisInvertEnabledChanged?.Invoke(yAxisInvertEnabled);
		}

		public override void UpdateToggleVisual(bool yAxisInvertEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(yAxisInvertEnabled);
		}
	}
}
