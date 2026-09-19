using System;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.VoiceControlModule.Scripts;

namespace Features.SessionManagementModule.Models
{
	public sealed class LobbyState : SessionStateBase
	{
		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		private readonly INetworkedSceneLoader _networkedSceneLoader;

		private readonly ILevelSelectionSource _levelSelectionSource;

		private readonly IPlayerAvatarLifecycle _playerAvatarLifecycle;

		private readonly IVoiceService _voiceService;

		private readonly ISessionLoadingUi _sessionLoadingUi;

		private readonly ISessionEndScreens _sessionEndScreens;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly INetworkedModelScopeController _networkedModelScopeController;

		private readonly IMultiplayerService _multiplayerService;

		private readonly IEnemySpawnGate _enemySpawnGate;

		private readonly ILevelBeachPresetApplication _levelBeachPresetApplication;

		private readonly IBeachInteractableRunReset _beachInteractableRunReset;

		private readonly ILevelQuotaInitializer _levelQuotaInitializer;

		private readonly IPlayerJoinSourcesLobbyLifecycle _playerJoinSourcesLobbyLifecycle;

		private readonly IMatchmakingSessionStartedAnalytics _matchmakingSessionStartedAnalytics;

		public override SessionState State => SessionState.Lobby;

		public override SessionAuthorityLane ActivateLane => SessionAuthorityLane.LobbyEnterActivate;

		public override SessionAuthorityLane DeactivateLane => SessionAuthorityLane.LobbyExitDeactivate;

		public LobbyState(ISessionPlayerPresence sessionPlayerPresence, INetworkedSceneLoader networkedSceneLoader, ILevelSelectionSource levelSelectionSource, IPlayerAvatarLifecycle playerAvatarLifecycle, IVoiceService voiceService, ISessionLoadingUi sessionLoadingUi, ISessionEndScreens sessionEndScreens, MultiplayerModel multiplayerModel, INetworkedModelScopeController networkedModelScopeController, IMultiplayerService multiplayerService, IEnemySpawnGate enemySpawnGate, ILevelBeachPresetApplication levelBeachPresetApplication, IBeachInteractableRunReset beachInteractableRunReset, ILevelQuotaInitializer levelQuotaInitializer, IPlayerJoinSourcesLobbyLifecycle playerJoinSourcesLobbyLifecycle, IMatchmakingSessionStartedAnalytics matchmakingSessionStartedAnalytics)
		{
			_sessionPlayerPresence = sessionPlayerPresence;
			_networkedSceneLoader = networkedSceneLoader;
			_levelSelectionSource = levelSelectionSource;
			_playerAvatarLifecycle = playerAvatarLifecycle;
			_voiceService = voiceService;
			_sessionLoadingUi = sessionLoadingUi;
			_sessionEndScreens = sessionEndScreens;
			_multiplayerModel = multiplayerModel;
			_networkedModelScopeController = networkedModelScopeController;
			_multiplayerService = multiplayerService;
			_enemySpawnGate = enemySpawnGate;
			_levelBeachPresetApplication = levelBeachPresetApplication;
			_beachInteractableRunReset = beachInteractableRunReset;
			_levelQuotaInitializer = levelQuotaInitializer;
			_playerJoinSourcesLobbyLifecycle = playerJoinSourcesLobbyLifecycle;
			_matchmakingSessionStartedAnalytics = matchmakingSessionStartedAnalytics;
		}

		public override async UniTask EnterAsync()
		{
			_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
			_sessionEndScreens.Dismiss();
			PlayerSessionPrefs.SetRecoveryPhase(SessionRecoveryPhase.Lobby);
			PlayerSessionPrefs.ClearErrorTrigger();
			await AuthorityGate(SessionAuthorityLane.LobbyEnterMarkSessionNotStarted, (Action)delegate
			{
				_multiplayerService.OverrideSessionProperty(_multiplayerModel.NetworkRunner, SessionPropertyType.IsSessionStarted, propertyValue: false);
			});
			await AuthorityGate(SessionAuthorityLane.LobbyReleaseRoster, () => _sessionPlayerPresence.ReleaseAllAsync());
			await AuthorityGate(SessionAuthorityLane.LobbyEnterResetWallet, (Action)delegate
			{
				_levelQuotaInitializer.ResetRunWallet();
			});
			_networkedModelScopeController.CloseLocalScope(ModelScope.Run);
			await AuthorityGate(SessionAuthorityLane.LobbyCloseRunScope, (Action)delegate
			{
				_networkedModelScopeController.CloseGlobalScope(ModelScope.Run);
			});
			_playerAvatarLifecycle.DespawnIfHeld();
			_levelBeachPresetApplication.ClearActivePreset();
			_beachInteractableRunReset.ResetRecordedInteractables();
			_playerJoinSourcesLobbyLifecycle.OnLobbyEntered();
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Lobby);
			await _voiceService.SpawnNon3dVoiceSpeaker(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			_sessionEndScreens.Dismiss();
		}

		public override async UniTask ExitAsync(SessionState next)
		{
			_matchmakingSessionStartedAnalytics.ReportRandoms();
			_voiceService.DespawnNon3dVoiceSpeaker(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			_networkedModelScopeController.CloseLocalScope(ModelScope.Lobby);
			await _sessionLoadingUi.ShowRunStartAsync();
			await AuthorityGate(SessionAuthorityLane.LobbyExitMarkSessionStarted, (Action)delegate
			{
				_multiplayerService.OverrideSessionProperty(_multiplayerModel.NetworkRunner, SessionPropertyType.IsSessionStarted, propertyValue: true);
			});
			await AuthorityGate(SessionAuthorityLane.LobbyOpenRunScope, () => _networkedModelScopeController.OpenGlobalScopeAsync(ModelScope.Run));
			await AuthorityGate(SessionAuthorityLane.LobbyExitApplyChapterStartWallet, (Action)delegate
			{
				_levelQuotaInitializer.ApplyChapterStartWallet();
			});
			await AuthorityGate(SessionAuthorityLane.LobbyLoadFirstLevel, (int epoch) => _networkedSceneLoader.LoadSceneAsync(_levelSelectionSource.SelectedLevelName, epoch));
			await _sessionPlayerPresence.PinAsync();
		}
	}
}
