using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class StreamerModeSettingsPresenter : PresenterBehaviour<StreamerModeSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public StreamerModeSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			base.View.OnStreamerModeEnabledChanged += SaveStreamerModeStatus;
		}

		protected override void OnDisposed()
		{
			base.View.OnStreamerModeEnabledChanged -= SaveStreamerModeStatus;
		}

		private void Initialize()
		{
			bool streamerModeEnabled = (_notAppliedSettingsModel.IsStreamerModeEnabledChanged ? _notAppliedSettingsModel.StreamerModeEnabled : _currentSettingsModel.StreamerModeEnabled);
			base.View.UpdateToggleVisual(streamerModeEnabled);
		}

		private void SaveStreamerModeStatus(bool value)
		{
			_notAppliedSettingsModel.StreamerModeEnabled = value;
		}
	}
}
