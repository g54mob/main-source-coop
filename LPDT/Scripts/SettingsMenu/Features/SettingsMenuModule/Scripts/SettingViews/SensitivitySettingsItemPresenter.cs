using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class SensitivitySettingsItemPresenter : PresenterBehaviour<SensitivitySettingsItemViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public SensitivitySettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			SensitivitySettingsItemViewBase view = base.View;
			view.OnMicrophoneSensitivityChanged = (Action<float>)Delegate.Combine(view.OnMicrophoneSensitivityChanged, new Action<float>(ChangeMicrophoneSensitivity));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			SensitivitySettingsItemViewBase view = base.View;
			view.OnMicrophoneSensitivityChanged = (Action<float>)Delegate.Remove(view.OnMicrophoneSensitivityChanged, new Action<float>(ChangeMicrophoneSensitivity));
		}

		private void Initialize()
		{
			float microphoneSensitivity = _currentSettingsModel.MicrophoneSensitivity;
			base.View.UpdateMicrophoneSensitivity(microphoneSensitivity);
		}

		private void ChangeMicrophoneSensitivity(float value)
		{
			_notAppliedSettingsModel.MicrophoneSensitivity = value;
		}
	}
}
