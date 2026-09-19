using System;
using Features.ConfirmExitPopupService.Scripts;
using Features.DisconnectHandlerModule.Scripts.Data;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.StateMachinesModule.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class BackMenuSettingsPresenter : PresenterBehaviour<BackMenuSettingsViewBase>
	{
		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly IConfirmExitService _confirmExitService;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private readonly ISettingsWindowProvider _settingsWindowProvider;

		public BackMenuSettingsPresenter(GameFlowStateMachine gameFlowStateMachine, IConfirmExitService confirmExitService, DisconnectRequestEventClass disconnectRequestEventClass, ISettingsWindowProvider settingsWindowProvider)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_confirmExitService = confirmExitService;
			_disconnectRequestEventClass = disconnectRequestEventClass;
			_settingsWindowProvider = settingsWindowProvider;
		}

		protected override void OnViewSet()
		{
			BackMenuSettingsViewBase view = base.View;
			view.OnBackLobbyButtonClick = (Action)Delegate.Combine(view.OnBackLobbyButtonClick, new Action(OpenBackToLobbyConfirmationWindow));
			UpdateViewVisible();
		}

		protected override void OnDisposed()
		{
			BackMenuSettingsViewBase view = base.View;
			view.OnBackLobbyButtonClick = (Action)Delegate.Remove(view.OnBackLobbyButtonClick, new Action(OpenBackToLobbyConfirmationWindow));
		}

		private void OpenBackToLobbyConfirmationWindow()
		{
			_confirmExitService.ShowPopup(BackToLobby, new ConfirmExitData
			{
				TitleLocalizationKey = LocalizationKey.UI_DisconnectConfirm_Title,
				DescriptionLocalizationKey = LocalizationKey.Ui_DisconnectConfirm_Description
			});
		}

		private void BackToLobby()
		{
			_settingsWindowProvider.GetSettingsWindow(isMenu: false).Close();
			_disconnectRequestEventClass.Publish(DisconnectRequestReason.GameplaySettingsBackToMenu);
		}

		private void UpdateViewVisible()
		{
			if (_gameFlowStateMachine.CurrentState == GameFlowState.SessionGameState)
			{
				base.View.ShowView();
			}
			else
			{
				base.View.HideView();
			}
		}
	}
}
