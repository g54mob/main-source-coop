using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class GammaDebugViewBase : ViewBehaviour
	{
		[SerializeField]
		private Slider _gammaSlider;

		[SerializeField]
		private TMP_Text _gammaText;

		public event Action<float> OnValueChanged;

		protected override void OnEnable()
		{
			_gammaSlider.onValueChanged.AddListener(OnGammaSliderValueChanged);
		}

		protected override void OnDisable()
		{
			_gammaSlider.onValueChanged.RemoveListener(OnGammaSliderValueChanged);
		}

		public virtual void SetValue(float value)
		{
			UpdateSensitivityText(value * 100f);
			_gammaSlider.SetValueWithoutNotify(value);
		}

		public virtual void OnGammaSliderValueChanged(float value)
		{
			UpdateSensitivityText(value * 100f);
			this.OnValueChanged?.Invoke(value);
		}

		private void UpdateSensitivityText(float value)
		{
			_gammaText.SetText((int)value + "%");
		}
	}
}
