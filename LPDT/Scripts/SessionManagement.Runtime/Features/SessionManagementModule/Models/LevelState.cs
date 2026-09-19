using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MouseVisibilityModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSkinModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StrechArmsModule.Scripts;
using Features.WeatherModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.SessionManagementModule.Models
{
	public sealed class LevelState : SessionStateBase
	{
		private const string SESSION_CORE_SCENE_NAME = "SessionCoreScene";

		private const string TUTORIAL_CORE_SCENE_NAME = "TutorialCoreScene";

		private const float RUN_COMPLETE_DISPLAY_SECONDS = 2f;

		private readonly ILevelSelectionSource _levelSelectionSource;

		private readonly INetworkedSceneLoader _networkedSceneLoader;

		private readonly ISynchronizationGate _synchronizationGate;

		private readonly INetworkedModelScopeController _networkedModelScopeController;

		private readonly ILevelCompletionObservation _levelCompletionObservation;

		private readonly IRunDefeatObservation _runDefeatObservation;

		private readonly ISessionEndScreens _sessionEndScreens;

		private readonly ILevelLoadObservation _levelLoadObservation;

		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		private readonly IPlayerAvatarLifecycle _playerAvatarLifecycle;

		private readonly ISessionLevelUi _sessionLevelUi;

		private readonly ISessionPlayerControl _sessionPlayerControl;

		private readonly MouseVisibilityModel _mouseVisibilityModel;

		private readonly ISessionGameplayActivation _sessionGameplayActivation;

		private readonly IMasterPlayerAvatarRegistrar _masterPlayerAvatarRegistrar;

		private readonly IMasterWeatherSelector _masterWeatherSelector;

		private readonly ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		private readonly ILevelQuotaInitializer _levelQuotaInitializer;

		private readonly IGamePhaseActivation _gamePhaseActivation;

		private readonly ILevelBeachPresetApplication _levelBeachPresetApplication;

		private readonly ILevelNavigationRebake _levelNavigationRebake;

		private readonly ILevelStatisticsReset _levelStatisticsReset;

		private readonly ILevelStatisticsWindow _levelStatisticsWindow;

		private readonly ILevelTransitionStragglerDamage _levelTransitionStragglerDamage;

		private readonly IEnemySpawnAnalyticsFlush _enemySpawnAnalyticsFlush;

		private readonly IEnemySpawnGate _enemySpawnGate;

		private readonly IEnemySpawnStateReset _enemySpawnStateReset;

		private readonly IEnemyPrefabWarmup _enemyPrefabWarmup;

		private readonly ISpawnPointTeleport _spawnPointTeleport;

		private readonly ISessionLoadingUi _sessionLoadingUi;

		private readonly ISessionVoiceControl _sessionVoiceControl;

		private readonly ISessionPlaceReporter _sessionPlaceReporter;

		private readonly IReconnectPlacement _reconnectPlacement;

		private readonly ISessionRecoveryController _sessionRecoveryController;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LineArmsModel _lineArmsModel;

		private readonly PlayerGrabSimplePointGrabableModel _playerGrabSimplePointGrabableModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly FallDamageGateModel _fallDamageGateModel;

		private readonly ArmStartsModel _armStartsModel;

		private readonly SkinModel _skinModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private static readonly Arm[] _armHealthOrientations = new Arm[2]
		{
			Arm.Left,
			Arm.Right
		};

		private bool _runCompleteShown;

		public override SessionState State => SessionState.Level;

		public override SessionAuthorityLane ActivateLane => SessionAuthorityLane.LevelEnterActivate;

		public override SessionAuthorityLane DeactivateLane => SessionAuthorityLane.LevelExitDeactivate;

		public LevelState(ILevelSelectionSource levelSelectionSource, INetworkedSceneLoader networkedSceneLoader, ISynchronizationGate synchronizationGate, INetworkedModelScopeController networkedModelScopeController, ILevelCompletionObservation levelCompletionObservation, IRunDefeatObservation runDefeatObservation, ISessionEndScreens sessionEndScreens, ILevelLoadObservation levelLoadObservation, ISessionPlayerPresence sessionPlayerPresence, IPlayerAvatarLifecycle playerAvatarLifecycle, ISessionLevelUi sessionLevelUi, ISessionPlayerControl sessionPlayerControl, MouseVisibilityModel mouseVisibilityModel, ISessionGameplayActivation sessionGameplayActivation, IMasterPlayerAvatarRegistrar masterPlayerAvatarRegistrar, IMasterWeatherSelector masterWeatherSelector, ILevelContentSpawnRegistry levelContentSpawnRegistry, ILevelQuotaInitializer levelQuotaInitializer, IGamePhaseActivation gamePhaseActivation, ILevelBeachPresetApplication levelBeachPresetApplication, ILevelNavigationRebake levelNavigationRebake, ILevelStatisticsReset levelStatisticsReset, ILevelStatisticsWindow levelStatisticsWindow, ILevelTransitionStragglerDamage levelTransitionStragglerDamage, IEnemySpawnAnalyticsFlush enemySpawnAnalyticsFlush, IEnemySpawnGate enemySpawnGate, IEnemySpawnStateReset enemySpawnStateReset, IEnemyPrefabWarmup enemyPrefabWarmup, ISpawnPointTeleport spawnPointTeleport, ISessionLoadingUi sessionLoadingUi, ISessionVoiceControl sessionVoiceControl, ISessionPlaceReporter sessionPlaceReporter, IReconnectPlacement reconnectPlacement, ISessionRecoveryController sessionRecoveryController, MultiplayerModel multiplayerModel, LineArmsModel lineArmsModel, PlayerGrabSimplePointGrabableModel playerGrabSimplePointGrabableModel, PlayerMovableModel playerMovableModel, FallDamageGateModel fallDamageGateModel, ArmStartsModel armStartsModel, SkinModel skinModel, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_levelSelectionSource = levelSelectionSource;
			_networkedSceneLoader = networkedSceneLoader;
			_synchronizationGate = synchronizationGate;
			_networkedModelScopeController = networkedModelScopeController;
			_levelCompletionObservation = levelCompletionObservation;
			_runDefeatObservation = runDefeatObservation;
			_sessionEndScreens = sessionEndScreens;
			_levelLoadObservation = levelLoadObservation;
			_sessionPlayerPresence = sessionPlayerPresence;
			_playerAvatarLifecycle = playerAvatarLifecycle;
			_sessionLevelUi = sessionLevelUi;
			_sessionPlayerControl = sessionPlayerControl;
			_mouseVisibilityModel = mouseVisibilityModel;
			_sessionGameplayActivation = sessionGameplayActivation;
			_masterPlayerAvatarRegistrar = masterPlayerAvatarRegistrar;
			_masterWeatherSelector = masterWeatherSelector;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
			_levelQuotaInitializer = levelQuotaInitializer;
			_gamePhaseActivation = gamePhaseActivation;
			_levelBeachPresetApplication = levelBeachPresetApplication;
			_levelNavigationRebake = levelNavigationRebake;
			_levelStatisticsReset = levelStatisticsReset;
			_levelStatisticsWindow = levelStatisticsWindow;
			_levelTransitionStragglerDamage = levelTransitionStragglerDamage;
			_enemySpawnAnalyticsFlush = enemySpawnAnalyticsFlush;
			_enemySpawnGate = enemySpawnGate;
			_enemySpawnStateReset = enemySpawnStateReset;
			_enemyPrefabWarmup = enemyPrefabWarmup;
			_spawnPointTeleport = spawnPointTeleport;
			_sessionLoadingUi = sessionLoadingUi;
			_sessionVoiceControl = sessionVoiceControl;
			_sessionPlaceReporter = sessionPlaceReporter;
			_reconnectPlacement = reconnectPlacement;
			_sessionRecoveryController = sessionRecoveryController;
			_multiplayerModel = multiplayerModel;
			_lineArmsModel = lineArmsModel;
			_playerGrabSimplePointGrabableModel = playerGrabSimplePointGrabableModel;
			_playerMovableModel = playerMovableModel;
			_fallDamageGateModel = fallDamageGateModel;
			_armStartsModel = armStartsModel;
			_skinModel = skinModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
		}

		public override async UniTask EnterAsync()
		{
			_runCompleteShown = false;
			_levelContentSpawnRegistry.BeginNewLevel();
			bool wasActiveOnEntry = base.IsTargetStateActive;
			await AuthorityGate(SessionAuthorityLane.LevelEnterOpenGates, delegate(int epoch)
			{
				_synchronizationGate.OpenOnEntry(SynchronizationGateKey.EnterLevel, epoch, _levelSelectionSource.EnterGateWindowTicks);
			});
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: false));
			_sessionLevelUi.Open();
			_sessionPlayerControl.LockControlls();
			await AuthorityGate(SessionAuthorityLane.LevelEnterOpenGlobalScope, () => _networkedModelScopeController.OpenGlobalScopeAsync(ModelScope.Level));
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Level);
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Run);
			await _sessionPlayerPresence.AttachAsync();
			string levelName = _levelSelectionSource.SelectedLevelName;
			await _levelLoadObservation.WaitUntilLoadedAsync(levelName);
			Scene sceneByName = SceneManager.GetSceneByName("SessionCoreScene");
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				sceneByName = SceneManager.GetSceneByName("TutorialCoreScene");
			}
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				sceneByName = SceneManager.GetSceneByName(levelName);
			}
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				SceneManager.SetActiveScene(sceneByName);
			}
			if (wasActiveOnEntry)
			{
				await _sessionPlayerPresence.WaitForReplicatedStateAsync();
			}
			SessionReconnectState saved = _sessionPlayerPresence.ReconnectState;
			float initialHealth = ((wasActiveOnEntry && saved.HasValue) ? saved.Health : (-1f));
			await _playerAvatarLifecycle.EnsureSpawnedAsync(initialHealth);
			await SynchronizationGate(SynchronizationGateKey.EnterLevel);
			if (wasActiveOnEntry && saved.HasValue && saved.LevelId == _reconnectPlacement.CurrentLevelId)
			{
				_spawnPointTeleport.TeleportLocalTo(saved.Position, saved.Yaw);
			}
			else
			{
				_spawnPointTeleport.TeleportToBeach();
			}
			_sessionPlayerControl.ApplyPersistedLifeState();
			_sessionPlayerControl.UnlockControlls();
			SetLocalArmsGrabEnabled(enabled: true);
			_sessionVoiceControl.RestartRecording();
			_sessionPlaceReporter.ReportLevelReached();
			if (!wasActiveOnEntry)
			{
				await AuthorityGate(SessionAuthorityLane.LevelEnterResetStatistics, (Action)delegate
				{
					_levelStatisticsReset.ResetForNewLevel();
				});
			}
			await AuthorityGate(SessionAuthorityLane.LevelEnterRegisterAvatars, (Action)delegate
			{
				_masterPlayerAvatarRegistrar.RegisterAllKnownAvatars();
			});
			await AuthorityGate(SessionAuthorityLane.LevelEnterSelectWeather, (Action)delegate
			{
				_masterWeatherSelector.SelectForCurrentLevel();
			});
			await AuthorityGate(SessionAuthorityLane.LevelEnterSpawnContent, () => _levelContentSpawnRegistry.SpawnAllAsync());
			await _levelNavigationRebake.RebakeForCurrentLevelAsync();
			await _levelBeachPresetApplication.ApplyForCurrentLevelAsync();
			await AuthorityGate(SessionAuthorityLane.LevelInitializeQuota, (Action)delegate
			{
				_levelQuotaInitializer.InitializeForCurrentLevel();
			});
			await AuthorityGate(SessionAuthorityLane.LevelEnterActivateGamePhases, (Action)delegate
			{
				_gamePhaseActivation.ActivateForCurrentLevel();
			});
			await AuthorityGate(SessionAuthorityLane.LevelEnterActivate, (Action)delegate
			{
				base.IsTargetStateActive = true;
			});
			_sessionGameplayActivation.NotifySessionStarted();
			EnablePlayerInterpolation();
			await _enemyPrefabWarmup.WarmupForCurrentLevelAsync();
			await UniTask.WaitForSeconds(1);
			await _sessionLoadingUi.HideAsync();
			_fallDamageGateModel.RequestArming();
			_enemySpawnGate.SetSpawningAllowed(isAllowed: true);
			VerifyLevelHealth(levelName);
		}

		public override void UpdateActiveLocal()
		{
			_sessionPlayerControl.MirrorLifeStateToPersistence();
			_sessionRecoveryController.EvaluateAvatarPresence(_playerAvatarLifecycle.IsAvatarHeld);
		}

		public override void UpdateActive()
		{
			if (_runCompleteShown)
			{
				return;
			}
			if (_runDefeatObservation.IsRunLost())
			{
				_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
				_sessionEndScreens.ShowDefeat();
				Transition(SessionState.Lobby);
			}
			else if (_levelCompletionObservation.IsLevelComplete())
			{
				if (_networkedSceneLoader.HasNextLevel())
				{
					_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
					Transition(SessionState.Shop);
				}
				else
				{
					_runCompleteShown = true;
					_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
					ShowRunCompleteThenEndRunAsync().Forget();
				}
			}
		}

		public override async UniTask ExitAsync(SessionState next)
		{
			await AuthorityGate(SessionAuthorityLane.LevelExitDeactivate, (Action)delegate
			{
				base.IsTargetStateActive = false;
			});
			_fallDamageGateModel.Disarm();
			_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
			_enemySpawnStateReset.ResetForLevelExit();
			await _sessionLoadingUi.ShowAsync();
			if (next == SessionState.Shop)
			{
				_levelStatisticsWindow.ShowForCurrentLevel();
			}
			ReleaseLocalArmsGrab();
			await WaitForNetworkTickAsync();
			DisablePlayerInterpolation();
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: true));
			await AuthorityGate(SessionAuthorityLane.LevelExitHarvestGold, (Action)delegate
			{
				_levelQuotaInitializer.HarvestCollectedGoldToWallet();
			});
			await AuthorityGate(SessionAuthorityLane.LevelExitFlushEnemySpawnAnalytics, (Action)delegate
			{
				_enemySpawnAnalyticsFlush.FlushPendingSpawns();
			});
			await AuthorityGate(SessionAuthorityLane.LevelExitStragglerDamage, (Action)delegate
			{
				_levelTransitionStragglerDamage.DamageOffBeachStragglers();
			});
			_networkedModelScopeController.CloseLocalScope(ModelScope.Level);
			await AuthorityGate(SessionAuthorityLane.LevelExitCloseGlobalScope, (Action)delegate
			{
				_networkedModelScopeController.CloseGlobalScope(ModelScope.Level);
			});
			if (next == SessionState.Shop)
			{
				await AuthorityGate(SessionAuthorityLane.LevelToShop, async delegate(int epoch)
				{
					_networkedSceneLoader.RequestAdvanceOnNextLoad();
					await _networkedSceneLoader.LoadSceneAsync(_levelSelectionSource.SelectedLevelName, epoch);
				});
				await AuthorityGate(SessionAuthorityLane.LevelExitApplyThemeBoundaryCap, (Action)delegate
				{
					_levelQuotaInitializer.ApplyThemeBoundaryCarryCap();
				});
			}
			else
			{
				await AuthorityGate(SessionAuthorityLane.LevelEndRun, (Func<UniTask>)async delegate
				{
					await _networkedSceneLoader.UnloadAsync();
					await _sessionPlayerPresence.ReleaseAllAsync();
				});
			}
		}

		private async UniTaskVoid ShowRunCompleteThenEndRunAsync()
		{
			_sessionEndScreens.ShowRunComplete();
			await UniTask.WaitForSeconds(2f);
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_runCompleteShown = false;
			}
			else
			{
				Transition(SessionState.Lobby);
			}
		}

		private void VerifyLevelHealth(string levelName)
		{
			List<string> list = new List<string>();
			if (!_levelLoadObservation.TryVerifyLoaded(levelName, out var problem))
			{
				list.Add(problem);
			}
			if (!_sessionPlayerPresence.IsAttached)
			{
				list.Add("not attached to its persistent player object");
			}
			if (!_playerAvatarLifecycle.IsAvatarHeld)
			{
				list.Add("avatar not held");
			}
			if (_sessionLoadingUi.IsBlackoutRaised)
			{
				list.Add("transition blackout still raised after the reveal — the view is blacked out (BlackScreenWindow not cleared)");
			}
			if (!AreLocalArmsConnected(out var problem2))
			{
				list.Add(problem2);
			}
			if (!IsLocalPlayerDeadOrDowned() && !IsLocalBodyHidden(out var problem3))
			{
				list.Add(problem3);
			}
			if (list.Count > 0)
			{
				Debug.LogError("[SessionHealth] Level transition UNHEALTHY: " + string.Join("; ", list));
			}
			else
			{
				Debug.Log("[SessionHealth] Level transition healthy — level '" + levelName + "' loaded, attached, avatar held.");
			}
		}

		private bool AreLocalArmsConnected(out string problem)
		{
			PlayerRef localPlayer = _multiplayerModel.NetworkRunner.LocalPlayer;
			Arm[] armHealthOrientations = _armHealthOrientations;
			foreach (Arm arm in armHealthOrientations)
			{
				if (!IsRigTransformLive(_armStartsModel.GetStart(localPlayer, arm)))
				{
					problem = $"own {arm} arm is disconnected — its registered start is a stale/inactive rig from a prior run (ArmStartsModel not cleared across the run boundary)";
					return false;
				}
				if (!IsRigTransformLive(_armStartsModel.GetEnd(localPlayer, arm)?.Transform))
				{
					problem = $"own {arm} arm is disconnected — its registered end is a stale/inactive rig from a prior run (the joint binds this dead rig; the live hand is unconstrained → collapses to origin/drifts)";
					return false;
				}
			}
			problem = null;
			return true;
		}

		private static bool IsRigTransformLive(Transform t)
		{
			if (t != null)
			{
				return t.gameObject.activeInHierarchy;
			}
			return false;
		}

		private bool IsLocalBodyHidden(out string problem)
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_skinModel.AllCharacterVisibility.TryGetValue(playerId, out var value))
			{
				problem = "own body skin visibility is not registered — the first-person body-hide was never applied";
				return false;
			}
			if (value.IsMainVisible())
			{
				problem = "own body is VISIBLE in first person — the skin's first-person hide leaked across the run boundary (you see your own body mesh)";
				return false;
			}
			problem = null;
			return true;
		}

		private bool IsLocalPlayerDeadOrDowned()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return false;
			}
			if (state != PlayerState.Dead)
			{
				return state == PlayerState.PreDeadCrouch;
			}
			return true;
		}

		private void SetLocalArmsGrabEnabled(bool enabled)
		{
			Dictionary<LineArmType, LineArmControllerBase> dictionary = LocalArms();
			if (dictionary == null)
			{
				return;
			}
			foreach (LineArmControllerBase value in dictionary.Values)
			{
				if (value != null)
				{
					value.EnableGrabbing(enabled);
				}
			}
		}

		private void ReleaseLocalArmsGrab()
		{
			SetLocalArmsGrabEnabled(enabled: false);
			Dictionary<LineArmType, LineArmControllerBase> dictionary = LocalArms();
			if (dictionary != null)
			{
				foreach (LineArmControllerBase value in dictionary.Values)
				{
					if (value != null)
					{
						value.UnJoinAllMatching(IsPlayerGrabbable);
					}
				}
			}
			ForceLocalUnGrabRecovery();
		}

		private bool IsPlayerGrabbable(IPointGrabable grabbable)
		{
			if (grabbable != null)
			{
				return _playerGrabSimplePointGrabableModel.PlayerGrabables.ContainsValue(grabbable);
			}
			return false;
		}

		private void ForceLocalUnGrabRecovery()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			foreach (NetworkObject allNetworkObject in networkRunner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.InputAuthority != networkRunner.LocalPlayer))
				{
					PlayerGrabController componentInChildren = allNetworkObject.GetComponentInChildren<PlayerGrabController>(includeInactive: true);
					if (!(componentInChildren == null))
					{
						componentInChildren.ForceUnGrabRecovery();
						break;
					}
				}
			}
		}

		private async UniTask WaitForNetworkTickAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			int startTick = runner.Tick;
			await UniTask.WaitUntil(() => (int)runner.Tick > startTick);
		}

		private void DisablePlayerInterpolation()
		{
			SetPlayerInterpolation(RigidbodyInterpolation.None);
		}

		private void EnablePlayerInterpolation()
		{
			SetPlayerInterpolation(RigidbodyInterpolation.Interpolate);
		}

		private void SetPlayerInterpolation(RigidbodyInterpolation interpolation)
		{
			foreach (PlayerCharacterMovableBase value in _playerMovableModel.AllCharacterMovables.Values)
			{
				if (!(value == null) && !(value.Rigidbody == null))
				{
					value.Rigidbody.interpolation = interpolation;
				}
			}
		}

		private Dictionary<LineArmType, LineArmControllerBase> LocalArms()
		{
			return _lineArmsModel.GetAllLineArmsForPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
		}
	}
}
