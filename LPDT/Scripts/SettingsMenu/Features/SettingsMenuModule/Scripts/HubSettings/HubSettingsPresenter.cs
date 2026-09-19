using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class HubSettingsPresenter : PresenterBehaviour<HubSettingsViewBase>
	{
		private readonly IApplySettingsService _applySettingsService;

		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly HubSettingsWindow _settingsWindow;

		private HubSettingsTab _currentActiveTab;

		public HubSettingsPresenter(HubSettingsWindow settingsWindow, IApplySettingsService applySettingsService, NotAppliedSettingsModel notAppliedSettingsModel)
		{
			_settingsWindow = settingsWindow;
			_applySettingsService = applySettingsService;
			_notAppliedSettingsModel = notAppliedSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.View.SetActiveTab(_currentActiveTab);
			HubSettingsViewBase view = base.View;
			view.OnClose = (Action)Delegate.Combine(view.OnClose, new Action(OnClose));
			HubSettingsViewBase view2 = base.View;
			view2.OnTabButtonClicked = (Action<HubSettingsTab>)Delegate.Combine(view2.OnTabButtonClicked, new Action<HubSettingsTab>(UpdateActiveTab));
			HubSettingsViewBase view3 = base.View;
			view3.OnApply = (Action)Delegate.Combine(view3.OnApply, new Action(OnApply));
			_notAppliedSettingsModel.OnSettingsChanged += RefreshSaveButtonState;
			RefreshSaveButtonState();
		}

		protected override void OnDisposed()
		{
			HubSettingsViewBase view = base.View;
			view.OnClose = (Action)Delegate.Remove(view.OnClose, new Action(OnClose));
			HubSettingsViewBase view2 = base.View;
			view2.OnTabButtonClicked = (Action<HubSettingsTab>)Delegate.Remove(view2.OnTabButtonClicked, new Action<HubSettingsTab>(UpdateActiveTab));
			HubSettingsViewBase view3 = base.View;
			view3.OnApply = (Action)Delegate.Remove(view3.OnApply, new Action(OnApply));
			_notAppliedSettingsModel.OnSettingsChanged -= RefreshSaveButtonState;
		}

		public void CloseSettingsWindowWithCheck()
		{
			if (_applySettingsService.HasNotAppliedSettings())
			{
				_applySettingsService.ApplySettings();
			}
			CloseSettingsWindow();
		}

		private void UpdateActiveTab(HubSettingsTab settingsTab)
		{
			_currentActiveTab = settingsTab;
			base.View.SetActiveTab(_currentActiveTab);
		}

		private void CloseSettingsWindow()
		{
			_applySettingsService.ResetAllSettings();
			_settingsWindow.Close();
		}

		private void OnApply()
		{
			_applySettingsService.ApplySettings();
			RefreshSaveButtonState();
		}

		private void OnClose()
		{
			CloseSettingsWindowWithCheck();
		}

		private void RefreshSaveButtonState()
		{
			base.View.SetSaveButtonInteractable(_applySettingsService.HasNotAppliedSettings());
		}
	}
}
