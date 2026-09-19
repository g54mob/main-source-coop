using System;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class GammaSettingsViewBase : SettingsItemViewBehaviour
	{
		[SerializeField]
		protected SettingsSlider _gammaSlider;

		public event Action<float> OnValueChanged;

		protected override void OnEnable()
		{
			_gammaSlider.OnValueChanged += OnGammaSliderValueChanged;
		}

		protected override void OnDisable()
		{
			_gammaSlider.OnValueChanged -= OnGammaSliderValueChanged;
		}

		public virtual void SetValue(float value)
		{
			UpdateSensitivityText(value * 100f);
			_gammaSlider.Slider.SetValueWithoutNotify(value);
		}

		public virtual void OnGammaSliderValueChanged(float value)
		{
			UpdateSensitivityText(value * 100f);
			this.OnValueChanged?.Invoke(value);
		}

		private void UpdateSensitivityText(float value)
		{
			_gammaSlider.Value.SetText((int)value + "%");
		}
	}
}
