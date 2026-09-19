using System;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsSlider : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _header;

		[SerializeField]
		private TMP_Text _value;

		[SerializeField]
		private Slider _slider;

		[SerializeField]
		private Button _leftButton;

		[SerializeField]
		private Button _rightButton;

		[SerializeField]
		private LeftRightButtonHolder _leftRightButtonHolder;

		[SerializeField]
		private float _stepSize = 10f;

		public Slider Slider => _slider;

		public TMP_Text Value => _value;

		public TMP_Text Header => _header;

		public event Action<float> OnValueChanged;

		private void OnEnable()
		{
			_slider.onValueChanged.AddListener(HandleSliderValueUpdated);
			_leftButton.onClick.AddListener(_leftRightButtonHolder.DecreaseValue);
			_rightButton.onClick.AddListener(_leftRightButtonHolder.IncreaseValue);
			_leftRightButtonHolder.OnValueDecreased += DecreaseSliderValue;
			_leftRightButtonHolder.OnValueIncreased += IncreaseSliderValue;
		}

		private void OnDisable()
		{
			_slider.onValueChanged.RemoveListener(HandleSliderValueUpdated);
			_leftButton.onClick.RemoveListener(_leftRightButtonHolder.DecreaseValue);
			_rightButton.onClick.RemoveListener(_leftRightButtonHolder.IncreaseValue);
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
