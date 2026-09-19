using System;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SessionManagementModule.Models;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Fusion;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.PlatformStatusRealizationModule.Scripts.Systems
{
	public class GameStatusSyncSystem : IInitializable, IDisposable
	{
		private readonly SessionStateMachine _sessionStateMachine;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly TutorialModel _tutorialModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private readonly IGameStatusService _gameStatusService;

		public GameStatusSyncSystem(SessionStateMachine sessionStateMachine, Features.LevelModule.Scripts.LevelModel levelModel, PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, TutorialModel tutorialModel, NetworkRunnerEventBus networkRunnerEventBus, MultiplayerSessionConfig multiplayerSessionConfig, IGameStatusService gameStatusService)
		{
			_sessionStateMachine = sessionStateMachine;
			_levelModel = levelModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_tutorialModel = tutorialModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_multiplayerSessionConfig = multiplayerSessionConfig;
			_gameStatusService = gameStatusService;
		}

		public void Initialize()
		{
			_sessionStateMachine.CurrentChanged += OnSessionStateChanged;
			_levelModel.OnCurrentChapterIndexChanged += OnLevelProgressChanged;
			_levelModel.OnCurrentLevelNumberChanged += OnLevelProgressChanged;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
			_tutorialModel.OnIsTutorialInProgressChanged += OnIsTutorialInProgressChanged;
			_tutorialModel.OnBaseTutorialCompleted += OnBaseTutorialCompleted;
			_networkRunnerEventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			RefreshStatus();
		}

		public void Dispose()
		{
			_sessionStateMachine.CurrentChanged -= OnSessionStateChanged;
			_levelModel.OnCurrentChapterIndexChanged -= OnLevelProgressChanged;
			_levelModel.OnCurrentLevelNumberChanged -= OnLevelProgressChanged;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
			_tutorialModel.OnIsTutorialInProgressChanged -= OnIsTutorialInProgressChanged;
			_tutorialModel.OnBaseTutorialCompleted -= OnBaseTutorialCompleted;
			_networkRunnerEventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		private void OnSessionStateChanged(SessionState state)
		{
			RefreshStatus();
		}

		private void OnLevelProgressChanged(int value)
		{
			RefreshStatus();
		}

		private void OnIsTutorialInProgressChanged()
		{
			RefreshStatus();
		}

		private void OnBaseTutorialCompleted()
		{
			RefreshStatus();
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent eventData)
		{
			RefreshStatus();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent eventData)
		{
			RefreshStatus();
		}

		private void OnSomePlayerStateChanged(PlayerStateData playerStateData)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && playerStateData.PlayerId == networkRunner.LocalPlayer.PlayerId)
			{
				RefreshStatus();
			}
		}

		private void RefreshStatus()
		{
			if (_tutorialModel.IsTutorialInProgress || _tutorialModel.IsBaseTutorialSequenceInvoked)
			{
				_gameStatusService.SetGameStatus(GameStatusId.Tutorial);
				return;
			}
			switch (_sessionStateMachine.Current)
			{
			case SessionState.Shop:
				RefreshStoreStatus();
				break;
			case SessionState.Level:
				RefreshLevelStatus();
				break;
			default:
				RefreshLobbyStatus();
				break;
			}
		}

		private void RefreshLobbyStatus()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			int num = (((object)networkRunner == null || !networkRunner.IsRunning) ? 1 : networkRunner.SessionInfo.PlayerCount);
			_gameStatusService.SetGameStatusParameter(GameStatusParameterId.Players, num.ToString());
			_gameStatusService.SetGameStatusParameter(GameStatusParameterId.MaxPlayers, _multiplayerSessionConfig.MaxPlayersInRoom.ToString());
			_gameStatusService.SetGameStatus(GameStatusId.Lobby);
		}

		private void RefreshStoreStatus()
		{
			int num = Math.Max(1, _levelModel.CurrentLevelInChapterIndex);
			_gameStatusService.SetGameStatusParameter(GameStatusParameterId.Chapter, (_levelModel.CurrentChapterIndex + 1).ToString());
			_gameStatusService.SetGameStatusParameter(GameStatusParameterId.Day, num.ToString());
			_gameStatusService.SetGameStatus(GameStatusId.Store);
		}

		private void RefreshLevelStatus()
		{
			if (_levelModel.CurrentLevelInChapterIndex != 0)
			{
				_gameStatusService.SetGameStatusParameter(GameStatusParameterId.Chapter, (_levelModel.CurrentChapterIndex + 1).ToString());
				_gameStatusService.SetGameStatusParameter(GameStatusParameterId.Day, _levelModel.CurrentLevelInChapterIndex.ToString());
				_gameStatusService.SetGameStatus(IsLocalPlayerDead() ? GameStatusId.SessionDead : GameStatusId.Session);
			}
		}

		private bool IsLocalPlayerDead()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			if (_playersStatesSynchronizer.TryGetState(networkRunner.LocalPlayer.PlayerId, out var state))
			{
				return state == PlayerState.Dead;
			}
			return false;
		}
	}
}
