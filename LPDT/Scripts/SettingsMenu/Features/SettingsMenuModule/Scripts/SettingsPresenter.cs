using System;
using Cysharp.Threading.Tasks;
using Features.ConfirmExitPopupService.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.UIAnimationsModule.Scripts;
using Features.UserReport.Adapter;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.StateMachinesModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsPresenter : PresenterBehaviour<SettingsViewBase>
	{
		private readonly IApplySettingsService _applySettingsService;

		private readonly IConfirmExitService _confirmExitService;

		private readonly ISavingService _savingService;

		private readonly IUserReportService _userReportService;

		private readonly NotAppliedSettingsModel _notAppliedSettingsModel;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly ISettingsWindowProvider _settingsWindowProvider;

		private readonly INavigationService _navigationService;

		private readonly IInputService _inputService;

		private readonly SettingsModel _settingsModel;

		private readonly IUIAnimationService _uiAnimationService;

		private SettingsTab _currentActiveTab;

		public SettingsPresenter(ISettingsWindowProvider settingsWindowProvider, IApplySettingsService applySettingsService, IConfirmExitService confirmExitService, ISavingService savingService, IUserReportService userReportService, NotAppliedSettingsModel notAppliedSettingsModel, INavigationService navigationService, IInputService inputService, SettingsModel settingsModel, GameFlowStateMachine gameFlowStateMachine, IUIAnimationService uiAnimationService)
		{
			_settingsWindowProvider = settingsWindowProvider;
			_applySettingsService = applySettingsService;
			_confirmExitService = confirmExitService;
			_savingService = savingService;
			_userReportService = userReportService;
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_navigationService = navigationService;
			_inputService = inputService;
			_settingsModel = settingsModel;
			_gameFlowStateMachine = gameFlowStateMachine;
			_uiAnimationService = uiAnimationService;
		}

		public void PlayOpenAnimation()
		{
			_uiAnimationService.Play(base.View.AnimationTarget, UIAnimationKey.ViewMoveIn);
		}

		private UniTask PlayCloseAnimationAsync()
		{
			return _uiAnimationService.PlayAsync(base.View.AnimationTarget, UIAnimationKey.ViewMoveOut);
		}

		protected override void OnViewSet()
		{
			base.View.SetActiveTab(_currentActiveTab);
			SettingsViewBase view = base.View;
			view.OnClose = (Action)Delegate.Combine(view.OnClose, new Action(OnClose));
			SettingsViewBase view2 = base.View;
			view2.OnTabButtonClicked = (Action<SettingsTab>)Delegate.Combine(view2.OnTabButtonClicked, new Action<SettingsTab>(UpdateActiveTab));
			SettingsViewBase view3 = base.View;
			view3.OnApply = (Action)Delegate.Combine(view3.OnApply, new Action(OnApply));
			SettingsViewBase view4 = base.View;
			view4.OnSendReport = (Action)Delegate.Combine(view4.OnSendReport, new Action(SendReport));
			_notAppliedSettingsModel.OnSettingsChanged += RefreshSaveButtonState;
			RefreshSaveButtonState();
		}

		protected override void OnDisposed()
		{
			SettingsViewBase view = base.View;
			view.OnClose = (Action)Delegate.Remove(view.OnClose, new Action(OnClose));
			SettingsViewBase view2 = base.View;
			view2.OnTabButtonClicked = (Action<SettingsTab>)Delegate.Remove(view2.OnTabButtonClicked, new Action<SettingsTab>(UpdateActiveTab));
			SettingsViewBase view3 = base.View;
			view3.OnApply = (Action)Delegate.Remove(view3.OnApply, new Action(OnApply));
			SettingsViewBase view4 = base.View;
			view4.OnSendReport = (Action)Delegate.Remove(view4.OnSendReport, new Action(SendReport));
			_notAppliedSettingsModel.OnSettingsChanged -= RefreshSaveButtonState;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			SubscribeInput();
			UpdateActiveTab(_currentActiveTab);
		}

		protected override void OnViewDisabled()
		{
			UnSubscribeInput();
		}

		public void CloseSettingsWindowWithCheck()
		{
			if (!_applySettingsService.HasNotAppliedSettings())
			{
				CloseSettingsWindowAsync().Forget();
				return;
			}
			_confirmExitService.ShowPopup(OnConfirmApplyAndClose, new ConfirmExitData
			{
				TitleLocalizationKey = LocalizationKey.UI_SettingsUnsavedPopup_Title,
				DescriptionLocalizationKey = LocalizationKey.UI_SettingsUnsavedPopup_Description
			}, delegate
			{
				CloseSettingsWindowAsync().Forget();
			});
		}

		private void SubscribeInput()
		{
			InputDefaultActions additionalNavigationLeft = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft.Performed = (Action)Delegate.Remove(additionalNavigationLeft.Performed, new Action(NavigateToLeft));
			InputDefaultActions additionalNavigationRight = _inputService.AdditionalNavigationRight;
			additionalNavigationRight.Performed = (Action)Delegate.Remove(additionalNavigationRight.Performed, new Action(NavigateToRight));
			InputDefaultActions additionalNavigationLeft2 = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft2.Performed = (Action)Delegate.Combine(additionalNavigationLeft2.Performed, new Action(NavigateToLeft));
			InputDefaultActions additionalNavigationRight2 = _inputService.AdditionalNavigationRight;
			additionalNavigationRight2.Performed = (Action)Delegate.Combine(additionalNavigationRight2.Performed, new Action(NavigateToRight));
		}

		private void UnSubscribeInput()
		{
			InputDefaultActions additionalNavigationLeft = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft.Performed = (Action)Delegate.Remove(additionalNavigationLeft.Performed, new Action(NavigateToLeft));
			InputDefaultActions additionalNavigationRight = _inputService.AdditionalNavigationRight;
			additionalNavigationRight.Performed = (Action)Delegate.Remove(additionalNavigationRight.Performed, new Action(NavigateToRight));
		}

		private void NavigateToRight()
		{
			NavigateToTab(1);
		}

		private void NavigateToLeft()
		{
			NavigateToTab(-1);
		}

		private void NavigateToTab(int direction)
		{
			int num = base.View.Tabs.IndexOf(_currentActiveTab);
			if (num < 0)
			{
				num = 0;
			}
			int index = (num + direction + base.View.Tabs.Count) % base.View.Tabs.Count;
			UpdateActiveTab(base.View.Tabs[index]);
		}

		private void OnConfirmApplyAndClose()
		{
			ApplyAndSaveSettings();
			CloseSettingsWindowAsync().Forget();
		}

		private void UpdateActiveTab(SettingsTab settingsTab)
		{
			_currentActiveTab = settingsTab;
			if (base.View.TabsMap[_currentActiveTab].PrioritySelectable != null)
			{
				_navigationService.SetNavigationToObject(base.View.TabsMap[_currentActiveTab].PrioritySelectable);
			}
			base.View.SetActiveTab(_currentActiveTab);
			_settingsModel.SetActiveTab(settingsTab);
		}

		private async UniTask CloseSettingsWindowAsync()
		{
			await PlayCloseAnimationAsync();
			_applySettingsService.ResetAllSettings();
			FocusableWindowBehaviour settingsWindow = _settingsWindowProvider.GetSettingsWindow(base.View.IsMenu);
			if (settingsWindow.WindowStatus == WindowStatus.Showed)
			{
				settingsWindow.Close();
			}
		}

		private void SendReport()
		{
			_userReportService.OpenUserReport();
		}

		private void OnApply()
		{
			ApplyAndSaveSettings();
			RefreshSaveButtonState();
		}

		private void ApplyAndSaveSettings()
		{
			_applySettingsService.ApplySettings();
			_savingService.SaveDataForGroup(SavingGroup.Settings);
			_navigationService.SetNavigationToObject(base.View.TabsMap[_currentActiveTab].PrioritySelectable);
		}

		private void OnClose()
		{
			CloseSettingsWindowWithCheck();
		}

		private void RefreshSaveButtonState()
		{
			bool interactable = _applySettingsService.HasNotAppliedSettings();
			base.View.TrySetSaveButtonInteractable(interactable);
		}
	}
}
