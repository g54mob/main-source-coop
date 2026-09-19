using System;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts
{
	public class SettingsSliderNavigation : MonoBehaviour
	{
		[SerializeField]
		private Slider _slider;

		[SerializeField]
		private LeftRightButtonHolder _leftRightButtonHolder;

		[SerializeField]
		private float _stepSize = 10f;

		public Slider Slider => _slider;

		public event Action<float> OnValueChanged;

		private void OnEnable()
		{
			_slider.onValueChanged.AddListener(HandleSliderValueUpdated);
			_leftRightButtonHolder.OnValueDecreased += DecreaseSliderValue;
			_leftRightButtonHolder.OnValueIncreased += IncreaseSliderValue;
		}

		private void OnDisable()
		{
			_slider.onValueChanged.RemoveListener(HandleSliderValueUpdated);
			_leftRightButtonHolder.OnValueDecreased -= DecreaseSliderValue;
			_leftRightButtonHolder.OnValueIncreased -= IncreaseSliderValue;
		}

		private void HandleSliderValueUpdated(float arg0)
		{
			this.OnValueChanged?.Invoke(arg0);
		}

		private void IncreaseSliderValue()
		{
			float value = _slider.value;
			value += _stepSize;
			value = Mathf.Clamp(value, _slider.minValue, _slider.maxValue);
			_slider.value = value;
			HandleSliderValueUpdated(value);
		}

		private void DecreaseSliderValue()
		{
			float value = _slider.value;
			value -= _stepSize;
			value = Mathf.Clamp(value, _slider.minValue, _slider.maxValue);
			_slider.value = value;
			HandleSliderValueUpdated(value);
		}
	}
}
