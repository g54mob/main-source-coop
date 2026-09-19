using System;
using System.Collections.Generic;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsMultipleItems : MonoBehaviour
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
		private LeftRightButtonHolder _leftRightButtonHolder;

		public TMP_Text Value => _value;

		public TMP_Text Header => _header;

		public List<string> Values => _leftRightButtonHolder.Values;

		public int ValueIndex
		{
			get
			{
				if (!(_leftRightButtonHolder == null))
				{
					return _leftRightButtonHolder.CurrentValueIndex;
				}
				return 0;
			}
		}

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

		public void SetValues(List<string> values)
		{
			_leftRightButtonHolder.Values = values;
		}

		public void SetIndex(int index)
		{
			_leftRightButtonHolder.SetIndex(index);
		}

		public void RefreshValues()
		{
			_leftRightButtonHolder.SetIndex(_leftRightButtonHolder.CurrentValueIndex);
		}
	}
}
