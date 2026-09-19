using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class SensitivitySettingsItemView : SensitivitySettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _sensitivityVolumeSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_sensitivityVolumeSlider.OnValueChanged += OnSensitivityChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_sensitivityVolumeSlider.OnValueChanged -= OnSensitivityChanged;
		}

		public override void UpdateMicrophoneSensitivity(float value)
		{
			_sensitivityVolumeSlider.Slider.maxValue = 100f;
			_sensitivityVolumeSlider.Slider.value = value;
			UpdateSensitivityText(value);
		}

		private void OnSensitivityChanged(float volume)
		{
			OnMicrophoneSensitivityChanged?.Invoke(volume);
			UpdateSensitivityText(volume);
		}

		private void UpdateSensitivityText(float value)
		{
			_sensitivityVolumeSlider.Value.SetText((int)value + "%");
		}
	}
}
