using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ShakeIntensitySettingsItemView : ShakeIntensitySettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _shakeIntensitySlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_shakeIntensitySlider.OnValueChanged += OnShakeIntensityChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_shakeIntensitySlider.OnValueChanged -= OnShakeIntensityChanged;
		}

		private void OnShakeIntensityChanged(float volume)
		{
			OnValueChanged?.Invoke(volume);
			UpdateShakeIntensityText(volume);
		}

		private void UpdateShakeIntensityText(float value)
		{
			_shakeIntensitySlider.Value.SetText((int)value + "%");
		}

		public override void SetHeadBobbingIntensity(float shakeIntensity)
		{
			_shakeIntensitySlider.Slider.maxValue = 100f;
			_shakeIntensitySlider.Slider.value = shakeIntensity;
			UpdateShakeIntensityText(shakeIntensity);
		}
	}
}
