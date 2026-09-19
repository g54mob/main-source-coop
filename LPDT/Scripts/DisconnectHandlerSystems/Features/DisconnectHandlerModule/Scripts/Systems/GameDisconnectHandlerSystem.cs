using System;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.RunningSessionModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Fusion;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Systems
{
	public class GameDisconnectHandlerSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly ISessionTeardownService _sessionTeardownService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private readonly IRunningSessionService _runningSessionService;

		private readonly ILoadingScreenService _loadingScreenService;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly ISessionEndAnalyticsService _sessionEndAnalyticsService;

		private bool _cleanupStarted;

		public GameDisconnectHandlerSystem(NetworkRunnerEventBus eventBus, ISessionTeardownService sessionTeardownService, MultiplayerModel multiplayerModel, DisconnectRequestEventClass disconnectRequestEventClass, IRunningSessionService runningSessionService, ILoadingScreenService loadingScreenService, GameFlowStateMachine gameFlowStateMachine, ISessionEndAnalyticsService sessionEndAnalyticsService)
		{
			_eventBus = eventBus;
			_sessionTeardownService = sessionTeardownService;
			_multiplayerModel = multiplayerModel;
			_disconnectRequestEventClass = disconnectRequestEventClass;
			_runningSessionService = runningSessionService;
			_loadingScreenService = loadingScreenService;
			_gameFlowStateMachine = gameFlowStateMachine;
			_sessionEndAnalyticsService = sessionEndAnalyticsService;
		}

		public void Initialize()
		{
			AddListeners();
		}

		public void Dispose()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			_disconnectRequestEventClass.OnRequestDisconnect += HandleLocalDisconnect;
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdown);
		}

		private void RemoveListeners()
		{
			_disconnectRequestEventClass.OnRequestDisconnect -= HandleLocalDisconnect;
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
		}

		private void HandleLocalDisconnect(DisconnectRequestReason reason)
		{
			if (_gameFlowStateMachine.CurrentState != GameFlowState.SessionGameState)
			{
				return;
			}
			if (reason != DisconnectRequestReason.Recovery)
			{
				PlayerSessionPrefs.ClearErrorTrigger();
				if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
				{
					_runningSessionService.SetSessionCodeSaved(null);
				}
			}
			_loadingScreenService.Show(LoadingScreenShowType.ShowUntilPlayersLoading, Disconnect);
		}

		private void Disconnect()
		{
			DisconnectAsync().Forget();
		}

		private async UniTaskVoid DisconnectAsync()
		{
			await ReturnHeldPlayersAuthorityAsync();
			_sessionEndAnalyticsService.TrySendSessionEnd(treatUnknownReasonAsDisconnect: true);
			CleanUpAndReload();
		}

		private async UniTask ReturnHeldPlayersAuthorityAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (runner == null || !runner.IsRunning)
			{
				return;
			}
			bool flag = false;
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.GetComponent<PlayerInitializer>() == null) && !(allNetworkObject.StateAuthority != runner.LocalPlayer) && !(allNetworkObject.InputAuthority == runner.LocalPlayer) && !(allNetworkObject.InputAuthority == PlayerRef.None))
				{
					PlayerGrabController componentInChildren = allNetworkObject.GetComponentInChildren<PlayerGrabController>(includeInactive: true);
					if (!(componentInChildren == null))
					{
						componentInChildren.ReturnGrabbedAuthorityToOwner();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (float elapsed = 0f; elapsed < 2f; elapsed += Time.unscaledDeltaTime)
			{
				if (!runner.IsRunning)
				{
					break;
				}
				if (!StillHoldsBorrowedAuthority(runner))
				{
					break;
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
		}

		private bool StillHoldsBorrowedAuthority(NetworkRunner runner)
		{
			if (!runner.IsRunning)
			{
				return false;
			}
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.StateAuthority == runner.LocalPlayer && allNetworkObject.InputAuthority != runner.LocalPlayer && allNetworkObject.InputAuthority != PlayerRef.None)
				{
					return true;
				}
			}
			return false;
		}

		private void OnShutdown(OnShutdownEvent onShutdownEvent)
		{
			if (_gameFlowStateMachine.CurrentState != GameFlowState.MenuGameState)
			{
				if (_sessionEndAnalyticsService.ShouldSendOnShutdown())
				{
					_sessionEndAnalyticsService.TrySendSessionEnd(treatUnknownReasonAsDisconnect: true);
				}
				if (SceneLoadGuard.CanPerformSceneOperations)
				{
					CleanUpAndReload();
				}
			}
		}

		private void CleanUpAndReload()
		{
			if (!_cleanupStarted)
			{
				_cleanupStarted = true;
				_sessionEndAnalyticsService.TrySendSessionEnd(treatUnknownReasonAsDisconnect: true);
				if (_gameFlowStateMachine.CurrentState == GameFlowState.SessionGameState)
				{
					_loadingScreenService.RetainBlackScreenBridgeUntilMenuEntrance();
				}
				_sessionTeardownService.TearDownAsync().Forget();
			}
		}
	}
}
