using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.LevelGatesModule.Data;
using Features.LevelModule.Scripts;
using Features.MainMenuModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StoreModule.Scripts.MovedToBeachLocal;
using Global.StateMachinesModule.Scripts;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class SessionEndPlaceTrackingSystem : IInitializable, IDisposable
	{
		private readonly SessionEndAnalyticsContextModel _contextModel;

		private readonly StartGameLoadingNetworkEvent _startGameLoadingNetworkEvent;

		private readonly StartLevelLoadingTransitionNetworkEvent _startLevelLoadingTransitionNetworkEvent;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelModel _levelModel;

		private readonly MovedToBeachEventClass _movedToBeachEventClass;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		public SessionEndPlaceTrackingSystem(SessionEndAnalyticsContextModel contextModel, StartGameLoadingNetworkEvent startGameLoadingNetworkEvent, StartLevelLoadingTransitionNetworkEvent startLevelLoadingTransitionNetworkEvent, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, LevelModel levelModel, MovedToBeachEventClass movedToBeachEventClass, GameFlowStateMachine gameFlowStateMachine, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel)
		{
			_contextModel = contextModel;
			_startGameLoadingNetworkEvent = startGameLoadingNetworkEvent;
			_startLevelLoadingTransitionNetworkEvent = startLevelLoadingTransitionNetworkEvent;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_movedToBeachEventClass = movedToBeachEventClass;
			_gameFlowStateMachine = gameFlowStateMachine;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
		}

		public void Initialize()
		{
			_startGameLoadingNetworkEvent.OnNetworkEventSend += OnLoadingStarted;
			_startLevelLoadingTransitionNetworkEvent.OnNetworkEventSend += OnLoadingStarted;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += OnBeforeLevelChange;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit += OnSomePlayerStateExit;
			_movedToBeachEventClass.OnLocalPlayerMovedToBeach += OnLocalPlayerLeftStoreToBeach;
			_gameFlowStateMachine.OnStateEnter += OnGameFlowStateEnter;
			_playersGatesModelSynchronizedModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChanged;
			_levelModel.OnCurrentSequenceLevelNumberChanged += OnCurrentSequenceLevelNumberChanged;
		}

		public void Dispose()
		{
			_startGameLoadingNetworkEvent.OnNetworkEventSend -= OnLoadingStarted;
			_startLevelLoadingTransitionNetworkEvent.OnNetworkEventSend -= OnLoadingStarted;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= OnBeforeLevelChange;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit -= OnSomePlayerStateExit;
			_movedToBeachEventClass.OnLocalPlayerMovedToBeach -= OnLocalPlayerLeftStoreToBeach;
			_gameFlowStateMachine.OnStateEnter -= OnGameFlowStateEnter;
			_playersGatesModelSynchronizedModel.OnPlayerInsideGateChanged -= OnPlayerInsideGateChanged;
			_levelModel.OnCurrentSequenceLevelNumberChanged -= OnCurrentSequenceLevelNumberChanged;
		}

		private void OnCurrentSequenceLevelNumberChanged(int _)
		{
			RegisterBeach();
		}

		private void OnGameFlowStateEnter(GameFlowState state)
		{
			if (state == GameFlowState.SessionGameState)
			{
				RegisterLoading();
			}
		}

		private void OnLoadingStarted<T>(T _)
		{
			RegisterLoading();
		}

		private void OnBeforeLevelChange(BeforeLevelChangeNetworkEvent _)
		{
			UnregisterLevel();
			UnregisterShop();
			UnregisterBeach();
			RegisterLoading();
		}

		private void OnPlayerInsideGateChanged(int ownerId, bool playerInsideGate)
		{
			if (IsLocalPlayer(ownerId))
			{
				SyncLocalGatePlaceState(playerInsideGate);
			}
		}

		private void OnSomePlayerStateChanged(PlayerStateData playerStateData)
		{
			if (IsLocalPlayer(playerStateData.PlayerId) && playerStateData.PlayerState == PlayerState.Store)
			{
				RegisterShop();
			}
		}

		private void OnSomePlayerStateExit(PlayerStateData playerStateData)
		{
			if (IsLocalPlayer(playerStateData.PlayerId) && playerStateData.PlayerState == PlayerState.Store)
			{
				UnregisterShop();
			}
		}

		private void OnLocalPlayerLeftStoreToBeach()
		{
			UnregisterShop();
		}

		private void RegisterLoading()
		{
			if (_gameFlowStateMachine.CurrentState == GameFlowState.SessionGameState)
			{
				_contextModel.RegisterPlace(SessionEndAnalyticsPlaceKind.Loading);
			}
		}

		private void RegisterBeach()
		{
			_contextModel.RegisterPlace(SessionEndAnalyticsPlaceKind.Beach, _levelModel.CurrentSequenceLevelNumber);
		}

		private void UnregisterBeach()
		{
			_contextModel.UnregisterPlace(SessionEndAnalyticsPlaceKind.Beach, _levelModel.CurrentSequenceLevelNumber);
		}

		private void RegisterLevel()
		{
			_contextModel.RegisterPlace(SessionEndAnalyticsPlaceKind.Level, _levelModel.CurrentSequenceLevelNumber);
		}

		private void UnregisterLevel()
		{
			_contextModel.UnregisterPlace(SessionEndAnalyticsPlaceKind.Level, _levelModel.CurrentSequenceLevelNumber);
		}

		private void RegisterShop()
		{
			_contextModel.RegisterPlace(SessionEndAnalyticsPlaceKind.Shop, _levelModel.CurrentSequenceLevelNumber);
		}

		private void UnregisterShop()
		{
			_contextModel.UnregisterPlace(SessionEndAnalyticsPlaceKind.Shop, _levelModel.CurrentSequenceLevelNumber);
		}

		private void SyncLocalGatePlaceState(bool insideGate)
		{
			if (insideGate)
			{
				RegisterLevel();
			}
			else
			{
				UnregisterLevel();
			}
		}

		private bool IsLocalPlayer(int playerId)
		{
			int localPlayerId = GetLocalPlayerId();
			if (localPlayerId != 0)
			{
				return playerId == localPlayerId;
			}
			return false;
		}

		private int GetLocalPlayerId()
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return 0;
			}
			return _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
		}
	}
}
