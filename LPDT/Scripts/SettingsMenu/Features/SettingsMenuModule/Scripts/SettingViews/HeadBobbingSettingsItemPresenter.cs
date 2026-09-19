using System;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class HeadBobbingSettingsItemPresenter : PresenterBehaviour<HeadBobbingSettingsItemViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public HeadBobbingSettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			Initialize();
			HeadBobbingSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Combine(view.OnValueChanged, new Action<float>(ChangeHeadBobbingIntensity));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			HeadBobbingSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Remove(view.OnValueChanged, new Action<float>(ChangeHeadBobbingIntensity));
		}

		private void Initialize()
		{
			float headBobbingIntensity = _currentSettingsModel.HeadBobbingIntensityNormalized * 100f;
			base.View.SetHeadBobbingIntensity(headBobbingIntensity);
		}

		private void ChangeHeadBobbingIntensity(float value)
		{
			_notAppliedSettingsModel.HeadBobbingIntensityNormalized = value / 100f;
		}
	}
}
