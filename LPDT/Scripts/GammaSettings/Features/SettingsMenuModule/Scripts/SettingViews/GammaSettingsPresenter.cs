using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class GammaSettingsPresenter : PresenterBehaviour<GammaSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public GammaSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.View.SetValue(Mathf.InverseLerp(-1f, 1f, _currentSettingsModel.Brightness));
			base.View.OnValueChanged += UpdateBrightness;
		}

		protected override void OnDisposed()
		{
			base.View.OnValueChanged -= UpdateBrightness;
		}

		private void UpdateBrightness(float value)
		{
			float brightness = Mathf.Lerp(-1f, 1f, value);
			_notAppliedSettingsModel.Brightness = brightness;
		}
	}
}
