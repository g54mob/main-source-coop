using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class NoiseSuppressionSettingsView : NoiseSuppressionSettingsViewBase
	{
		[SerializeField]
		private Toggle _enabledToggle;

		protected override void OnEnable()
		{
			base.OnEnable();
			_enabledToggle.onValueChanged.AddListener(InvokeOnNoiseSuppressionEnabledChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_enabledToggle.onValueChanged.RemoveListener(InvokeOnNoiseSuppressionEnabledChanged);
		}

		private void InvokeOnNoiseSuppressionEnabledChanged(bool noiseSuppressionEnabled)
		{
			OnNoiseSuppressionEnabledChanged?.Invoke(noiseSuppressionEnabled);
		}

		public override void UpdateToggleVisual(bool noiseSuppressionEnabled)
		{
			_enabledToggle.SetIsOnWithoutNotify(noiseSuppressionEnabled);
		}
	}
}
