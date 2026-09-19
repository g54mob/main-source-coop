using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class FieldOfViewSettingsItemPresenter : PresenterBehaviour<FieldOfViewSettingsItemViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public FieldOfViewSettingsItemPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			FieldOfViewSettingsItemViewBase view = base.View;
			view.OnFieldOfViewChanged = (Action<float>)Delegate.Combine(view.OnFieldOfViewChanged, new Action<float>(ChangeFieldOfView));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			FieldOfViewSettingsItemViewBase view = base.View;
			view.OnFieldOfViewChanged = (Action<float>)Delegate.Remove(view.OnFieldOfViewChanged, new Action<float>(ChangeFieldOfView));
		}

		private void Initialize()
		{
			float num = _currentSettingsModel.FieldOfView;
			if (num < 60f)
			{
				num = 60f;
			}
			base.View.UpdateFieldOfView(num);
		}

		private void ChangeFieldOfView(float fieldOfView)
		{
			_notAppliedSettingsModel.FieldOfView = fieldOfView;
		}
	}
}
