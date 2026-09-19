using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class MouseSensitivitySettingsItemPresenter : PresenterBehaviour<MouseSensitivitySettingsItemViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public MouseSensitivitySettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			MouseSensitivitySettingsItemViewBase view = base.View;
			view.OnSensitivityChanged = (Action<float>)Delegate.Combine(view.OnSensitivityChanged, new Action<float>(ChangeMouseSensitivity));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			MouseSensitivitySettingsItemViewBase view = base.View;
			view.OnSensitivityChanged = (Action<float>)Delegate.Remove(view.OnSensitivityChanged, new Action<float>(ChangeMouseSensitivity));
		}

		private void Initialize()
		{
			float mouseSensitivity = _currentSettingsModel.MouseSensitivity;
			base.View.UpdateSensitivity(mouseSensitivity);
		}

		private void ChangeMouseSensitivity(float value)
		{
			_notAppliedSettingsModel.MouseSensitivity = value;
		}
	}
}
