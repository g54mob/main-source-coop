using System;
using System.Collections.Generic;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsFillableMultipleValues : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _header;

		[SerializeField]
		private TMP_Text _value;

		[SerializeField]
		private Button _leftButton;

		[SerializeField]
		private Button _rightButton;

		[SerializeField]
		private Image _fillImage;

		[SerializeField]
		private LeftRightButtonHolder _leftRightButtonHolder;

		public TMP_Text Value => _value;

		public TMP_Text Header => _header;

		public int ValueIndex => _leftRightButtonHolder.CurrentValueIndex;

		public event Action<int> OnValueChanged;

		private void OnEnable()
		{
			_leftRightButtonHolder.OnValueChanged += HandleValueChanged;
			_leftButton.onClick.AddListener(_leftRightButtonHolder.DecreaseValue);
			_rightButton.onClick.AddListener(_leftRightButtonHolder.IncreaseValue);
		}

		private void OnDisable()
		{
			_leftRightButtonHolder.OnValueChanged -= HandleValueChanged;
			_leftButton.onClick.RemoveListener(_leftRightButtonHolder.DecreaseValue);
			_rightButton.onClick.RemoveListener(_leftRightButtonHolder.IncreaseValue);
		}

		private void HandleValueChanged()
		{
			this.OnValueChanged?.Invoke(_leftRightButtonHolder.CurrentValueIndex);
		}

		public void SetFillAmount(float result)
		{
			_fillImage.fillAmount = result;
		}

		public void SetValues(List<string> fpsValues)
		{
			_leftRightButtonHolder.Values = fpsValues;
		}

		public void SetIndex(int fpsValuesCount)
		{
			_leftRightButtonHolder.SetIndex(fpsValuesCount);
		}

		private void SetDefaultHeader(string header)
		{
			_header.text = header;
		}

		private void SetDefaultValue(string value)
		{
			_value.text = value;
		}
	}
}
