using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class HeadBobbingSettingsItemView : HeadBobbingSettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _headBobbingSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_headBobbingSlider.OnValueChanged += OnHeadBobbingChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_headBobbingSlider.OnValueChanged -= OnHeadBobbingChanged;
		}

		private void OnHeadBobbingChanged(float volume)
		{
			OnValueChanged?.Invoke(volume);
			UpdateHeadBobbingIntensityText(volume);
		}

		private void UpdateHeadBobbingIntensityText(float value)
		{
			_headBobbingSlider.Value.SetText((int)value + "%");
		}

		public override void SetHeadBobbingIntensity(float shakeIntensity)
		{
			_headBobbingSlider.Slider.maxValue = 100f;
			_headBobbingSlider.Slider.value = shakeIntensity;
			UpdateHeadBobbingIntensityText(shakeIntensity);
		}
	}
}
