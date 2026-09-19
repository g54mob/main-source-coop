using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class YAxisInvertSettingsPresenter : PresenterBehaviour<YAxisInvertSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public YAxisInvertSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			YAxisInvertSettingsViewBase view = base.View;
			view.OnYAxisInvertEnabledChanged = (Action<bool>)Delegate.Combine(view.OnYAxisInvertEnabledChanged, new Action<bool>(SaveYAxisInvertEnabledStatus));
		}

		protected override void OnDisposed()
		{
			YAxisInvertSettingsViewBase view = base.View;
			view.OnYAxisInvertEnabledChanged = (Action<bool>)Delegate.Remove(view.OnYAxisInvertEnabledChanged, new Action<bool>(SaveYAxisInvertEnabledStatus));
		}

		private void SaveYAxisInvertEnabledStatus(bool value)
		{
			_notAppliedSettingsModel.YAxisInvertEnabled = value;
		}

		private void Initialize()
		{
			bool yAxisInvertEnabled = _currentSettingsModel.YAxisInvertEnabled;
			base.View.UpdateToggleVisual(yAxisInvertEnabled);
		}
	}
}
