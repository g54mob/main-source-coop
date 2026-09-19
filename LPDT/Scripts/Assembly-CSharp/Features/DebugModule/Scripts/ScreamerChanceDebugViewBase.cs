using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public abstract class ScreamerChanceDebugViewBase : ViewBehaviour
	{
		[SerializeField]
		private Slider _chanceSlider;

		[SerializeField]
		private TMP_Text _chanceText;

		[SerializeField]
		private Button _applyButton;

		public event Action<float> OnValueChanged;

		public event Action OnApplyClicked;

		protected override void OnEnable()
		{
			_chanceSlider.onValueChanged.AddListener(OnChanceSliderValueChanged);
			_applyButton.onClick.AddListener(OnApplyButtonClicked);
		}

		protected override void OnDisable()
		{
			_chanceSlider.onValueChanged.RemoveListener(OnChanceSliderValueChanged);
			_applyButton.onClick.RemoveListener(OnApplyButtonClicked);
		}

		public virtual void SetValue(float value)
		{
			UpdateChanceText(value);
			_chanceSlider.SetValueWithoutNotify(value);
		}

		public virtual void OnChanceSliderValueChanged(float value)
		{
			UpdateChanceText(value);
			this.OnValueChanged?.Invoke(value);
		}

		private void OnApplyButtonClicked()
		{
			this.OnApplyClicked?.Invoke();
		}

		private void UpdateChanceText(float value)
		{
			_chanceText.SetText($"{Mathf.RoundToInt(value * 100f)}%");
		}
	}
}
