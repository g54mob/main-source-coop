using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class MicrophoneEnabledSettingsPresenter : PresenterBehaviour<MicrophoneEnabledSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public MicrophoneEnabledSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			MicrophoneEnabledSettingsViewBase view = base.View;
			view.OnMicrophoneEnabledChanged = (Action<bool>)Delegate.Combine(view.OnMicrophoneEnabledChanged, new Action<bool>(SaveMicrophoneEnabledStatus));
		}

		protected override void OnDisposed()
		{
			MicrophoneEnabledSettingsViewBase view = base.View;
			view.OnMicrophoneEnabledChanged = (Action<bool>)Delegate.Remove(view.OnMicrophoneEnabledChanged, new Action<bool>(SaveMicrophoneEnabledStatus));
		}

		private void SaveMicrophoneEnabledStatus(bool value)
		{
			_notAppliedSettingsModel.MicrophoneEnabled = value;
		}

		private void Initialize()
		{
			bool microphoneEnabled = _currentSettingsModel.MicrophoneEnabled;
			base.View.UpdateToggleVisual(microphoneEnabled);
		}
	}
}
