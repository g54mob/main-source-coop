using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class FieldOfViewSettingsItemView : FieldOfViewSettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _fieldOfViewSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_fieldOfViewSlider.OnValueChanged += InvokeOnFieldOfViewChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_fieldOfViewSlider.OnValueChanged -= InvokeOnFieldOfViewChanged;
		}

		public override void UpdateFieldOfView(float value)
		{
			_fieldOfViewSlider.Slider.minValue = 60f;
			_fieldOfViewSlider.Slider.maxValue = 90f;
			_fieldOfViewSlider.Slider.value = value;
			UpdateFieldOfViewText(value);
		}

		private void InvokeOnFieldOfViewChanged(float value)
		{
			OnFieldOfViewChanged?.Invoke(value);
			UpdateFieldOfViewText(value);
		}

		private void UpdateFieldOfViewText(float value)
		{
			_fieldOfViewSlider.Value.SetText(((int)value).ToString() ?? "");
		}
	}
}
