using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.BeachInteractableCommonModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkRandomModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SettingsMenuModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VoiceControlModule.Scripts;
using Features.WeatherModule.Scripts;
using Fusion;
using Global.StateMachinesModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class LobbySceneBootstrap : MonoBehaviour
	{
		private IWindowsService _windowsService;

		private ILoadingScreenService _loadingScreenService;

		[SerializeField]
		private GameObject _lobbyPlayerPrefab;

		private MultiplayerModel _multiplayerModel;

		private NetworkObject _playerObject;

		private SettingsWindow _settingsWindow;

		private IVoiceService _voiceService;

		private GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private IMultiplayerService _multiplayerService;

		private GameFlowStateMachine _gameFlowStateMachine;

		private BeachInteractableSpawnModel _beachInteractableSpawnModel;

		private bool _bypassedToSession;

		private WeatherModel _weatherModel;

		private INetworkRandomInitializationService _networkRandomInitializationService;

		[Inject]
		public void InjectDependencies(IWindowsService windowsService, MultiplayerModel multiplayerModel, ILoadingScreenService loadingScreenService, SettingsWindow settingsWindow, IVoiceService voiceService, GameAnalyticsEventSendService gameAnalyticsEventSendService, IMultiplayerService multiplayerService, GameFlowStateMachine gameFlowStateMachine, BeachInteractableSpawnModel beachInteractableSpawnModel, WeatherModel weatherModel, INetworkRandomInitializationService networkRandomInitializationService)
		{
			_multiplayerModel = multiplayerModel;
			_windowsService = windowsService;
			_loadingScreenService = loadingScreenService;
			_settingsWindow = settingsWindow;
			_voiceService = voiceService;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_multiplayerService = multiplayerService;
			_gameFlowStateMachine = gameFlowStateMachine;
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
			_weatherModel = weatherModel;
			_networkRandomInitializationService = networkRandomInitializationService;
		}

		private void Awake()
		{
			if (IsGameAlreadyStarted())
			{
				_bypassedToSession = true;
				_gameFlowStateMachine.EnterState(GameFlowState.SessionGameState);
			}
		}

		private void Start()
		{
			if (!_bypassedToSession)
			{
				PlayerSessionPrefs.SetRecoveryPhase(SessionRecoveryPhase.Lobby);
				if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
				{
					_networkRandomInitializationService.InjectNetworkRandoms();
				}
				if (_settingsWindow.WindowStatus == WindowStatus.Showed)
				{
					_settingsWindow.Close();
				}
				_windowsService.OpenWindow<LobbyWindow>();
				_loadingScreenService.DismissVisibleOverlayAsync().Forget();
				_voiceService.SpawnNon3dVoiceSpeaker(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
				_gameAnalyticsEventSendService.TrackLobbyEntered(_multiplayerModel.LocalJoinSource.ToString());
			}
		}

		private bool IsGameAlreadyStarted()
		{
			try
			{
				return _multiplayerService.GetSessionProperty<bool>(_multiplayerModel.NetworkRunner, SessionPropertyType.IsSessionStarted);
			}
			catch
			{
				return false;
			}
		}

		private void OnDestroy()
		{
			_voiceService.DespawnNon3dVoiceSpeaker(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
		}
	}
}
