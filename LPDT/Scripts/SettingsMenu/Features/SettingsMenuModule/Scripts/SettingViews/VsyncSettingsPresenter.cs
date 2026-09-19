using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class VsyncSettingsPresenter : PresenterBehaviour<VsyncSettingsViewBase>
	{
		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly CurrentSettingsModel _currentSettingsModel;

		public VsyncSettingsPresenter(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
		}

		protected override void OnViewSet()
		{
			Initialize();
			base.View.OnVSyncEnabledChanged += SaveVSyncStatus;
		}

		protected override void OnDisposed()
		{
			base.View.OnVSyncEnabledChanged -= SaveVSyncStatus;
		}

		private void Initialize()
		{
			bool vSyncEnabled = (_notAppliedSettingsModel.IsVSyncEnabledChanged ? _notAppliedSettingsModel.VSyncEnabled : _currentSettingsModel.VSyncEnabled);
			base.View.UpdateToggleVisual(vSyncEnabled);
		}

		private void SaveVSyncStatus(bool value)
		{
			_notAppliedSettingsModel.VSyncEnabled = value;
		}
	}
}
