using System;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ShakeIntensitySettingsItemPresenter : PresenterBehaviour<ShakeIntensitySettingsItemViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public ShakeIntensitySettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			Initialize();
			ShakeIntensitySettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Combine(view.OnValueChanged, new Action<float>(ChangeScreenShakeIntensity));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			ShakeIntensitySettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Remove(view.OnValueChanged, new Action<float>(ChangeScreenShakeIntensity));
		}

		private void Initialize()
		{
			float headBobbingIntensity = _currentSettingsModel.ScreenShakeIntensityNormalized * 100f;
			base.View.SetHeadBobbingIntensity(headBobbingIntensity);
		}

		private void ChangeScreenShakeIntensity(float value)
		{
			_notAppliedSettingsModel.ScreenShakeIntensityNormalized = value / 100f;
		}
	}
}
