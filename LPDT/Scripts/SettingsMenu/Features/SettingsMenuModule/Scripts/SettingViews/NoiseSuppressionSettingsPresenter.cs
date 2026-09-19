using System;
using Features.SettingsMenuModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class NoiseSuppressionSettingsPresenter : PresenterBehaviour<NoiseSuppressionSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public NoiseSuppressionSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			NoiseSuppressionSettingsViewBase view = base.View;
			view.OnNoiseSuppressionEnabledChanged = (Action<bool>)Delegate.Combine(view.OnNoiseSuppressionEnabledChanged, new Action<bool>(SaveNoiseSuppressionEnabledStatus));
		}

		protected override void OnDisposed()
		{
			NoiseSuppressionSettingsViewBase view = base.View;
			view.OnNoiseSuppressionEnabledChanged = (Action<bool>)Delegate.Remove(view.OnNoiseSuppressionEnabledChanged, new Action<bool>(SaveNoiseSuppressionEnabledStatus));
		}

		private void SaveNoiseSuppressionEnabledStatus(bool value)
		{
			_notAppliedSettingsModel.NoiseSuppressionEnabled = value;
		}

		private void Initialize()
		{
			bool noiseSuppressionEnabled = _currentSettingsModel.NoiseSuppressionEnabled;
			base.View.UpdateToggleVisual(noiseSuppressionEnabled);
		}
	}
}
