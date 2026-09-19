using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.BootstrapModule.Scripts.Systems;
using Features.CameraModelModule;
using Features.DeadPartsModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.LevelModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.PositionMarkersModule;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.RumModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SettingsMenuModule.Scripts;
using Features.StatsUsageModule.Scripts;
using Features.StoreModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VignetteUIEffectModule.Scripts;
using Fusion;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	[NetworkBehaviourWeaved(0)]
	public class SessionSceneBootstrap : NetworkBehaviour
	{
		private const int MaxSpawnMarkerWaitFrames = 300;

		private GameFlowStateMachine _gameFlowStateMachine;

		private NetworkRunnerEventBus _networkRunnerEventBus;

		private MultiplayerModel _multiplayerModel;

		private IWindowsService _windowsService;

		[SerializeField]
		private GameObject _playerPrefab;

		[SerializeField]
		private Camera _playerCameraPrefab;

		[SerializeField]
		private GameObject _fpCamera;

		[SerializeField]
		private GameObject _tpCamera;

		[SerializeField]
		private GameObject _fpFollowCamera;

		[SerializeField]
		private GameObject _cardTableCamera;

		[SerializeField]
		private GameObject _playerFreeFlyPrefab;

		private IInputService _inputService;

		private CameraModel _cameraModel;

		private DiContainer _diContainer;

		private PositionMarkersModel _positionMarkersModel;

		private IPlayerStateService _playerStateService;

		private ILevelService _levelService;

		private SettingsWindow _settingsWindow;

		private GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private LevelModel _levelModel;

		private PlayerMovableModel _playerMovableModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IMultiplayerService _multiplayerService;

		private IPlayerStateLifecycleService _playerStateLifecycleService;

		private IPlayerReboundBroadcastService _playerReboundBroadcastService;

		private IPlayerSpawnCoordinationService _playerSpawnCoordinationService;

		private SentAnalyticsModel _sentAnalyticsModel;

		private ISavingService _savingService;

		private StorePhaseModel _storePhaseModel;

		private TeleportationPointsEventClass _teleportationPointsEventClass;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private RumConfiguration _rumConfiguration;

		private RumStatsRewardModel _rumStatsRewardModel;

		private IPlayerStatsUpgradeService _playerStatsUpgradeService;

		private IPlayerStatsInitializeService _playerStatsInitializeService;

		private IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		private IStoreSeatingService _storeSeatingService;

		private ILoadingScreenService _loadingScreenService;

		private bool _localPlayerBootstrapCompleted;

		private bool _sessionUiOpened;

		private bool _subscribedToLevelLoadUnlock;

		private bool _reconnectIntoStore;

		[Inject]
		public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine, NetworkRunnerEventBus networkRunnerEventBus, MultiplayerModel multiplayerModel, IInputService inputService, IWindowsService windowsService, CameraModel cameraModel, DiContainer diContainer, PositionMarkersModel positionMarkersModel, IPlayerStateService playerStateService, ILevelService levelService, SettingsWindow settingsWindow, GameAnalyticsEventSendService gameAnalyticsEventSendService, LevelModel levelModel, PlayerMovableModel playerMovableModel, SpawnedPlayersModel spawnedPlayersModel, IMultiplayerService multiplayerService, IPlayerStateLifecycleService playerStateLifecycleService, IPlayerReboundBroadcastService playerReboundBroadcastService, IPlayerSpawnCoordinationService playerSpawnCoordinationService, SentAnalyticsModel sentAnalyticsModel, ISavingService savingService, StorePhaseModel storePhaseModel, TeleportationPointsEventClass teleportationPointsEventClass, PlayersStatesSynchronizer playersStatesSynchronizer, RumConfiguration rumConfiguration, RumStatsRewardModel rumStatsRewardModel, IPlayerStatsUpgradeService playerStatsUpgradeService, IPlayerStatsInitializeService playerStatsInitializeService, IPlayerDeadPartSpawnService playerDeadPartSpawnService, IStoreSeatingService storeSeatingService, ILoadingScreenService loadingScreenService)
		{
			_positionMarkersModel = positionMarkersModel;
			_diContainer = diContainer;
			_cameraModel = cameraModel;
			_inputService = inputService;
			_gameFlowStateMachine = gameFlowStateMachine;
			_networkRunnerEventBus = networkRunnerEventBus;
			_multiplayerModel = multiplayerModel;
			_windowsService = windowsService;
			_playerStateService = playerStateService;
			_levelService = levelService;
			_settingsWindow = settingsWindow;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_levelModel = levelModel;
			_playerMovableModel = playerMovableModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_multiplayerService = multiplayerService;
			_playerStateLifecycleService = playerStateLifecycleService;
			_playerReboundBroadcastService = playerReboundBroadcastService;
			_playerSpawnCoordinationService = playerSpawnCoordinationService;
			_sentAnalyticsModel = sentAnalyticsModel;
			_savingService = savingService;
			_storePhaseModel = storePhaseModel;
			_teleportationPointsEventClass = teleportationPointsEventClass;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_rumConfiguration = rumConfiguration;
			_rumStatsRewardModel = rumStatsRewardModel;
			_playerStatsUpgradeService = playerStatsUpgradeService;
			_playerStatsInitializeService = playerStatsInitializeService;
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
			_storeSeatingService = storeSeatingService;
			_loadingScreenService = loadingScreenService;
		}

		private void Awake()
		{
			if (_gameFlowStateMachine.CurrentState != GameFlowState.SessionGameState)
			{
				_gameFlowStateMachine.EnterState(GameFlowState.SessionGameState);
			}
		}

		private void Start()
		{
			int num = _sentAnalyticsModel.IncrementSessionStartedCount();
			_savingService.SaveDataForGroup(SavingGroup.Analytics);
			_gameAnalyticsEventSendService.TrackSessionStart(num == 1, ResolvePartySize());
		}

		private int ResolvePartySize()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				Debug.LogWarning("[Analytics] NetworkRunner is null at session start; using party size 1.");
				return 1;
			}
			return networkRunner.ActivePlayers.Count();
		}

		public override void Spawned()
		{
		}

		private void OpenSessionUi()
		{
			if (!_sessionUiOpened)
			{
				_sessionUiOpened = true;
				_windowsService.OpenWindow<SessionWindow>();
				_windowsService.OpenWindow<VignetteEffectWindow>();
				if (Debug.isDebugBuild || Application.isEditor)
				{
					_windowsService.OpenWindow<DebugEnableWindow>();
				}
				_windowsService.OpenWindow<WorldCanvasWindow>();
			}
		}

		private async UniTask EnsureLocalPlayerBootstrapAsync()
		{
			if (_localPlayerBootstrapCompleted)
			{
				return;
			}
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (!(runner == null) && runner.IsRunning)
			{
				await UniTask.WaitUntil(() => runner != null && runner.IsRunning);
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (!_localPlayerBootstrapCompleted)
				{
					await BootstrapLocalPlayerAsync();
				}
			}
		}

		private async UniTask BootstrapLocalPlayerAsync()
		{
			if (_localPlayerBootstrapCompleted)
			{
				return;
			}
			_localPlayerBootstrapCompleted = true;
			OpenSessionUi();
			SubscribeToLevelLoadUnlock();
			bool isLateJoiner = IsLateJoinerSession();
			bool isReconnect = _multiplayerModel.IsReconnecting;
			bool skipLoadingLock = isLateJoiner && !isReconnect;
			try
			{
				_inputService.EnableMovementMap();
				EnsureLocalCamerasRegistered();
				SpawnFreeFlyPlayer(await SpawnOrReconnectPlayerAsync());
				ApplyInitialPlayerStateAfterSpawn(isLateJoiner);
				if (!skipLoadingLock)
				{
					LockMovementUntilPlayersLoading();
					UnlockMovementForDebugLevel();
				}
			}
			finally
			{
				if (skipLoadingLock || isReconnect)
				{
					ReleaseSessionBootstrapLocks();
				}
			}
		}

		private void EnsureLocalCamerasRegistered()
		{
			if (!_cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera))
			{
				GameObject gameObject = _diContainer.InstantiatePrefab(_playerCameraPrefab);
				_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.FPCamera, InstantiateCamera(_fpCamera));
				_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.TPCamera, InstantiateCamera(_tpCamera));
				_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.FPFollowCamera, InstantiateCamera(_fpFollowCamera));
				_cameraModel.RegisterCamera(Features.CameraModelModule.CameraType.CardTableCamera, InstantiateCamera(_cardTableCamera));
				_cameraModel.CameraObject = gameObject.GetComponent<Camera>();
				_cameraModel.UpdateSensitivity();
			}
		}

		private void SubscribeToLevelLoadUnlock()
		{
			if (!_subscribedToLevelLoadUnlock)
			{
				_subscribedToLevelLoadUnlock = true;
				_levelModel.OnLastLoadedLevelChanged += PersistReconnectLevelId;
				_levelModel.OnBeforeLevelLoaded += ClearLevelSpawnTeleportSuppressionOnProgression;
			}
		}

		private bool IsLateJoinerSession()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			return _multiplayerModel.IsReconnecting;
		}

		private void PersistReconnectLevelId(LevelType levelType)
		{
			LevelType levelType2 = ((_levelModel.CurrentLevel != LevelType.None) ? _levelModel.CurrentLevel : levelType);
			if (levelType2 != LevelType.None)
			{
				PlayerSessionPrefs.SetCurrentLevel((int)levelType2);
			}
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			if (_subscribedToLevelLoadUnlock)
			{
				_levelModel.OnLastLoadedLevelChanged -= PersistReconnectLevelId;
				_levelModel.OnBeforeLevelLoaded -= ClearLevelSpawnTeleportSuppressionOnProgression;
			}
		}

		private void LockMovementUntilPlayersLoading()
		{
			_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.SessionLoading);
			_playerMovableModel.AddLockMovementReason(LockMovementReasonEnum.SessionLoading);
		}

		private void UnlockPlayersMovementAfterLoading()
		{
			_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.SessionLoading);
			_playerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.SessionLoading);
		}

		private void ReleaseSessionBootstrapLocks()
		{
			UnlockPlayersMovementAfterLoading();
			UnlockMovementForDebugLevel();
			_loadingScreenService.DismissForLateJoiner();
		}

		private void UnlockMovementForDebugLevel()
		{
			if (_levelModel.LastLoadedLevel != LevelType.None)
			{
				return;
			}
			UnlockPlayersMovementAfterLoading();
			if (!IsCoreLoopBeachPipeline())
			{
				if (_loadingScreenService.IsContentLoadingActive && _loadingScreenService.ActiveShowType.HasValue)
				{
					_loadingScreenService.Hide(_loadingScreenService.ActiveShowType.Value);
				}
				else if (_loadingScreenService.IsVisible)
				{
					_loadingScreenService.FadeOut();
				}
			}
		}

		private bool IsCoreLoopBeachPipeline()
		{
			if (_levelModel.SelectedLevel != LevelType.CoreLoopScene && _levelModel.ParentalLevel != LevelType.CoreLoopScene)
			{
				return _levelService.IsNextLevelLoadAvailable();
			}
			return true;
		}

		private CameraControllerBase InstantiateCamera(GameObject cameraPrefab)
		{
			GameObject obj = _diContainer.InstantiatePrefab(cameraPrefab);
			obj.name = cameraPrefab.name;
			return obj.GetComponent<CameraControllerBase>();
		}

		private void ApplyInitialPlayerStateAfterSpawn(bool isReconnectSpawn)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			int playerId = networkRunner.LocalPlayer.PlayerId;
			if (isReconnectSpawn && _reconnectIntoStore)
			{
				_playerStateService.ChangePlayerState(PlayerState.Store);
				return;
			}
			if (isReconnectSpawn && PlayerSessionPrefs.HasSavedHealth() && !PlayerSessionPrefs.TryGetSavedHealth(out var _))
			{
				_playerStateService.ChangePlayerState(PlayerState.Dead);
				return;
			}
			PlayerState playerState = _playerStateService.GetPlayerState(playerId);
			if (isReconnectSpawn && (playerState == PlayerState.Dead || playerState == PlayerState.PreDeadCrouch))
			{
				_playerStateService.ChangePlayerState(playerState);
			}
			else if (_storePhaseModel.IsStoreActive.Value)
			{
				_playerStateService.ChangePlayerState(PlayerState.Store);
			}
			else
			{
				_playerStateService.ChangePlayerState(PlayerState.Alive);
			}
		}

		private async UniTask<Vector3> SpawnOrReconnectPlayerAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			PlayerRef localPlayer = runner.LocalPlayer;
			bool isLateJoiner = IsLateJoinerSession();
			if (isLateJoiner)
			{
				await UniTask.WaitUntil(() => _levelModel.CurrentLevel != LevelType.None).TimeoutWithoutException(TimeSpan.FromSeconds(10.0));
			}
			int savedLevelId = PlayerSessionPrefs.GetSavedLevelId();
			int currentLevel = (int)_levelModel.CurrentLevel;
			bool hasSavedReconnectPosition = isLateJoiner && PlayerSessionPrefs.HasSavedReconnectPosition(currentLevel);
			bool flag = isLateJoiner && currentLevel != 0 && savedLevelId == currentLevel;
			bool flag2 = isLateJoiner && !hasSavedReconnectPosition && flag;
			if (flag2)
			{
				flag2 = await ResolveMasterStorePhaseAsync();
			}
			_reconnectIntoStore = flag2 && AnyEstablishedPeerInStore();
			if (isLateJoiner)
			{
				PlayerSpawnLock.SetLevelSpawnTeleportSuppressed(value: true);
			}
			if (hasSavedReconnectPosition || (isLateJoiner && _reconnectIntoStore))
			{
				ClearLevelSpawnTeleportSuppressionAsync().Forget();
			}
			Quaternion finalSpawnRotation = Quaternion.identity;
			Vector3 finalSpawnPosition;
			if (hasSavedReconnectPosition && PlayerSessionPrefs.TryGetSavedPosition(out var position))
			{
				finalSpawnPosition = position;
			}
			else if (isLateJoiner && _reconnectIntoStore)
			{
				Vector3 vector = (await ResolveStoreSpawnPositionAsync()) ?? (await ResolveDefaultSpawnPositionAsync());
				finalSpawnPosition = vector;
			}
			else if (isLateJoiner && !_reconnectIntoStore)
			{
				Vector3 vector = (await ResolveAfterStoreSpawnPositionAsync()) ?? (await ResolveDefaultSpawnPositionAsync());
				finalSpawnPosition = vector;
			}
			else
			{
				finalSpawnPosition = await ResolveDefaultSpawnPositionAsync();
			}
			PlayerSpawnLock.BeginSpawn(finalSpawnPosition, finalSpawnRotation, hasSavedReconnectPosition);
			float initialHealth = -1f;
			if (isLateJoiner && PlayerSessionPrefs.HasSavedHealth())
			{
				initialHealth = 0f;
				PlayerSessionPrefs.TryGetSavedHealth(out initialHealth);
			}
			_playerStateLifecycleService.PurgePlayerState(localPlayer);
			await _playerSpawnCoordinationService.WaitForSpawnClearanceAsync(localPlayer, finalSpawnPosition, hasSavedReconnectPosition);
			NetworkObject networkObject;
			try
			{
				networkObject = await runner.SpawnAsync(_playerPrefab, finalSpawnPosition, finalSpawnRotation, localPlayer, delegate(NetworkRunner networkRunner, NetworkObject networkObject2)
				{
					if (networkObject2.TryGetComponent<PlayerInitializer>(out var component))
					{
						component.InitialHealth = initialHealth;
					}
				});
			}
			catch
			{
				PlayerSpawnLock.CancelSpawn();
				throw;
			}
			if (networkObject != null)
			{
				_spawnedPlayersModel.RegisterPlayer(localPlayer, networkObject);
				networkObject.GetComponent<PlayerCharacterMovableBase>()?.RebindToAvatar(networkObject);
				_playerReboundBroadcastService.Broadcast(localPlayer, networkObject);
				RegisterLocalPlayerCameraRig(networkObject);
				if (isLateJoiner)
				{
					RestoreReconnectStatsAsync(localPlayer.PlayerId).Forget();
					_multiplayerModel.IsReconnecting = false;
					PlayerSpawnLock.CompleteSpawn();
				}
			}
			else
			{
				PlayerSpawnLock.CancelSpawn();
			}
			return finalSpawnPosition;
		}

		private async UniTaskVoid RestoreReconnectStatsAsync(int playerId)
		{
			await UniTask.WaitUntil(() => _playerStatsUpgradeService.IsPlayerStatsReady()).TimeoutWithoutException(TimeSpan.FromSeconds(15.0));
			if (_playerStatsUpgradeService.IsPlayerStatsReady())
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				await UniTask.Yield(PlayerLoopTiming.Update);
				_rumStatsRewardModel.RestoreFromSession(_rumConfiguration, _playerStatsUpgradeService);
				_playerDeadPartSpawnService.ReapplySavedDeadPartBooster();
				await UniTask.Yield(PlayerLoopTiming.Update);
				await UniTask.Yield(PlayerLoopTiming.Update);
				_playerStatsInitializeService.ApplyReconnectHealthClampedToMax(playerId);
			}
		}

		private void ClearLevelSpawnTeleportSuppressionOnProgression(LevelType levelType)
		{
			PlayerSpawnLock.SetLevelSpawnTeleportSuppressed(value: false);
		}

		private async UniTaskVoid ClearLevelSpawnTeleportSuppressionAsync()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(8.0));
			PlayerSpawnLock.SetLevelSpawnTeleportSuppressed(value: false);
		}

		private bool AnyEstablishedPeerInStore()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			PlayerRef localPlayer = networkRunner.LocalPlayer;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (!(activePlayer == localPlayer) && _playersStatesSynchronizer.TryGetState(activePlayer.PlayerId, out var state) && state == PlayerState.Store)
				{
					return true;
				}
			}
			return false;
		}

		private async UniTask<bool> ResolveMasterStorePhaseAsync()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return _storePhaseModel.IsStoreActive.Value;
			}
			await UniTask.WaitUntil(() => _storePhaseModel.IsAttached).TimeoutWithoutException(TimeSpan.FromSeconds(10.0));
			return _storePhaseModel.IsAttached && _storePhaseModel.IsStoreActive.Value;
		}

		private async UniTask<Vector3?> ResolveStoreSpawnPositionAsync()
		{
			await UniTask.WaitUntil(() => _teleportationPointsEventClass.TeleportByPlayerStatePoints.TryGetValue(PlayerState.Store, out var value2) && value2.Count > 0).TimeoutWithoutException(TimeSpan.FromSeconds(5.0));
			if (!_teleportationPointsEventClass.TeleportByPlayerStatePoints.TryGetValue(PlayerState.Store, out var value) || value.Count == 0)
			{
				return null;
			}
			int num = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (num < 0)
			{
				num = 0;
			}
			return value[num % value.Count].Position;
		}

		private async UniTask<Vector3?> ResolveAfterStoreSpawnPositionAsync()
		{
			await UniTask.WaitUntil(() => _teleportationPointsEventClass.BeachTeleportationPoints.Count > 0).TimeoutWithoutException(TimeSpan.FromSeconds(5.0));
			List<TeleportationPoint> beachTeleportationPoints = _teleportationPointsEventClass.BeachTeleportationPoints;
			if (beachTeleportationPoints == null || beachTeleportationPoints.Count == 0)
			{
				return null;
			}
			int num = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (num < 0)
			{
				num = 0;
			}
			return beachTeleportationPoints[num % beachTeleportationPoints.Count].Position;
		}

		private async UniTask<Vector3> ResolveDefaultSpawnPositionAsync()
		{
			for (int i = 0; i < 300; i++)
			{
				PositionMarker randomPositionMarker = _positionMarkersModel.GetRandomPositionMarker(PositionName.PlayerSpawnPoint);
				if (randomPositionMarker != null)
				{
					return randomPositionMarker.transform.position;
				}
				await UniTask.Yield();
			}
			PositionMarker randomPositionMarker2 = _positionMarkersModel.GetRandomPositionMarker(PositionName.PlayerSpawnPoint);
			return (randomPositionMarker2 != null) ? randomPositionMarker2.transform.position : Vector3.zero;
		}

		private void RegisterLocalPlayerCameraRig(NetworkObject avatar)
		{
			if (_cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera))
			{
				PlayerCharacterMovableBase component = avatar.GetComponent<PlayerCharacterMovableBase>();
				if (!(component == null))
				{
					Transform cameraPositionTransform = component.CameraPositionTransform;
					_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
					_cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
					_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
				}
			}
		}

		private void SpawnFreeFlyPlayer(Vector3 position)
		{
			_diContainer.InstantiatePrefab(_playerFreeFlyPrefab, position, Quaternion.identity, null);
		}

		private void InvokeStartGamePhase()
		{
			_networkRunnerEventBus.Publish(new OnStartGamePhaseEvent(_levelService.GetSelectedLevelName(), _multiplayerModel.NetworkRunner.SessionInfo.PlayerCount));
			_networkRunnerEventBus.Publish(new OnLateStartGamePhaseEvent());
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
