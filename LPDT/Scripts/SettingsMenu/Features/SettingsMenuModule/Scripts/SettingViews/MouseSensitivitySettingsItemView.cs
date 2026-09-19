using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class MouseSensitivitySettingsItemView : MouseSensitivitySettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _sensitivityVolumeSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_sensitivityVolumeSlider.OnValueChanged += InvokeOnSensitivityChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_sensitivityVolumeSlider.OnValueChanged -= InvokeOnSensitivityChanged;
		}

		public override void UpdateSensitivity(float value)
		{
			_sensitivityVolumeSlider.Slider.maxValue = 100f;
			_sensitivityVolumeSlider.Slider.value = value;
			UpdateSensitivityText(value);
		}

		private void InvokeOnSensitivityChanged(float volume)
		{
			OnSensitivityChanged?.Invoke(volume);
			UpdateSensitivityText(volume);
		}

		private void UpdateSensitivityText(float value)
		{
			_sensitivityVolumeSlider.Value.SetText((int)value + "%");
		}
	}
}
