using Features.SettingsMenuModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class GammaDebugPresenter : PresenterBehaviour<GammaDebugViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly IApplySettingsService _applySettingsService;

		public GammaDebugPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel, IApplySettingsService applySettingsService)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
			_applySettingsService = applySettingsService;
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
			_applySettingsService.ApplyBrightness();
		}
	}
}
