using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class GammaSettingsViewStartPopup : GammaSettingsViewBase
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private float _minValueAlpha = 0.1f;

		[SerializeField]
		private float _maxValueAlpha = 1f;

		public override void SetValue(float value)
		{
			base.SetValue(value);
			_canvasGroup.alpha = Mathf.Lerp(_minValueAlpha, _maxValueAlpha, value);
		}

		public override void OnGammaSliderValueChanged(float value)
		{
			base.OnGammaSliderValueChanged(value);
			_canvasGroup.alpha = Mathf.Lerp(_minValueAlpha, _maxValueAlpha, value);
		}
	}
}
