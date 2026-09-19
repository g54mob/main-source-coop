using System;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.SettingsMenuModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class SessionLobbyWindowSystem : IInitializable, IDisposable
	{
		private readonly SessionStateMachine _sessionStateMachine;

		private readonly IWindowsService _windowsService;

		private readonly LobbyWindow _lobbyWindow;

		private readonly ISessionLoadingUi _sessionLoadingUi;

		private readonly SettingsWindow _settingsWindow;

		private readonly TutorialModel _tutorialModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		public SessionLobbyWindowSystem(SessionStateMachine sessionStateMachine, IWindowsService windowsService, LobbyWindow lobbyWindow, ISessionLoadingUi sessionLoadingUi, SettingsWindow settingsWindow, TutorialModel tutorialModel, MultiplayerModel multiplayerModel, GameAnalyticsEventSendService gameAnalyticsEventSendService)
		{
			_sessionStateMachine = sessionStateMachine;
			_windowsService = windowsService;
			_lobbyWindow = lobbyWindow;
			_sessionLoadingUi = sessionLoadingUi;
			_settingsWindow = settingsWindow;
			_tutorialModel = tutorialModel;
			_multiplayerModel = multiplayerModel;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
		}

		public void Initialize()
		{
			_sessionStateMachine.CurrentChanged += OnCurrentChanged;
			if (_sessionStateMachine.Current == SessionState.Lobby)
			{
				OpenLobby();
			}
		}

		public void Dispose()
		{
			_sessionStateMachine.CurrentChanged -= OnCurrentChanged;
		}

		private void OnCurrentChanged(SessionState state)
		{
			if (state == SessionState.Lobby)
			{
				OpenLobby();
			}
			else
			{
				CloseLobby();
			}
		}

		private void OpenLobby()
		{
			if (_settingsWindow.WindowStatus == WindowStatus.Showed)
			{
				_settingsWindow.Close();
			}
			if (_lobbyWindow.WindowStatus != WindowStatus.Showed)
			{
				_windowsService.OpenWindow<LobbyWindow>();
			}
			if (!_tutorialModel.IsBaseTutorialSequenceInvoked)
			{
				_sessionLoadingUi.HideAsync().Forget();
			}
			_gameAnalyticsEventSendService.TrackLobbyEntered(_multiplayerModel.LocalJoinSource.ToString());
		}

		private void CloseLobby()
		{
			if (_lobbyWindow.WindowStatus == WindowStatus.Showed)
			{
				_lobbyWindow.Close();
			}
		}
	}
}
