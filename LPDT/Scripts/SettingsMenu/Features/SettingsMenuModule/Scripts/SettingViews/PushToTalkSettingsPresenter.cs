using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class PushToTalkSettingsPresenter : PresenterBehaviour<PushToTalkSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public PushToTalkSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			PushToTalkSettingsViewBase view = base.View;
			view.OnPushToTalkEnabledChanged = (Action<bool>)Delegate.Combine(view.OnPushToTalkEnabledChanged, new Action<bool>(SavePushToTalkEnabledStatus));
		}

		protected override void OnDisposed()
		{
			PushToTalkSettingsViewBase view = base.View;
			view.OnPushToTalkEnabledChanged = (Action<bool>)Delegate.Remove(view.OnPushToTalkEnabledChanged, new Action<bool>(SavePushToTalkEnabledStatus));
		}

		private void SavePushToTalkEnabledStatus(bool value)
		{
			_notAppliedSettingsModel.PushToTalkEnabled = value;
		}

		private void Initialize()
		{
			bool pushToTalkEnabled = _currentSettingsModel.PushToTalkEnabled;
			base.View.UpdateToggleVisual(pushToTalkEnabled);
		}
	}
}
