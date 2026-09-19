using System;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Features.RunningSessionModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Fusion;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Systems
{
	public class LobbyDisconnectHandlerSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly IMultiplayerService _multiplayerService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private readonly IRunningSessionService _runningSessionService;

		private readonly ILoadingScreenService _loadingScreenService;

		private readonly ISessionEndAnalyticsService _sessionEndAnalyticsService;

		private readonly MenuOpenedAnalyticsSourceModel _menuOpenedAnalyticsSourceModel;

		public LobbyDisconnectHandlerSystem(NetworkRunnerEventBus eventBus, IMultiplayerService multiplayerService, MultiplayerModel multiplayerModel, GameFlowStateMachine gameFlowStateMachine, DisconnectRequestEventClass disconnectRequestEventClass, IRunningSessionService runningSessionService, ILoadingScreenService loadingScreenService, ISessionEndAnalyticsService sessionEndAnalyticsService, MenuOpenedAnalyticsSourceModel menuOpenedAnalyticsSourceModel)
		{
			_eventBus = eventBus;
			_multiplayerService = multiplayerService;
			_multiplayerModel = multiplayerModel;
			_gameFlowStateMachine = gameFlowStateMachine;
			_disconnectRequestEventClass = disconnectRequestEventClass;
			_runningSessionService = runningSessionService;
			_loadingScreenService = loadingScreenService;
			_sessionEndAnalyticsService = sessionEndAnalyticsService;
			_menuOpenedAnalyticsSourceModel = menuOpenedAnalyticsSourceModel;
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
			_eventBus.Subscribe<OnPlayerLeftEvent>(HandlePlayerDisconnect);
		}

		private void RemoveListeners()
		{
			_disconnectRequestEventClass.OnRequestDisconnect -= HandleLocalDisconnect;
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerDisconnect);
		}

		private void HandlePlayerDisconnect(OnPlayerLeftEvent playerLeftEvent)
		{
		}

		private void HandleLocalDisconnect(DisconnectRequestReason _)
		{
			_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade, ExitFromLobby);
		}

		private void ExitFromLobby()
		{
			PlayerSessionPrefs.ClearErrorTrigger();
			_sessionEndAnalyticsService.TrySendSessionEnd(treatUnknownReasonAsDisconnect: true);
			_runningSessionService.SetSessionCodeSaved(null);
			RemoveListeners();
			_multiplayerService.Shutdown(_multiplayerModel.NetworkRunner, ShutdownReason.Ok).ContinueWith((Action)EnterHubState).Forget();
		}

		private void EnterHubState()
		{
			_menuOpenedAnalyticsSourceModel.SetPending(MainMenuOpenedAnalyticsSource.Lobby);
			_gameFlowStateMachine.EnterState(GameFlowState.MenuGameState);
		}
	}
}
