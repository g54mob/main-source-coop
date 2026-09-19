using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MouseVisibilityModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.SessionManagementModule.Models
{
	public sealed class ShopState : SessionStateBase
	{
		private const string SESSION_CORE_SCENE_NAME = "SessionCoreScene";

		private const string TUTORIAL_CORE_SCENE_NAME = "TutorialCoreScene";

		private const int SHOP_HEALTH_POLL_COUNT = 20;

		private const float SHOP_HEALTH_POLL_INTERVAL = 0.1f;

		private readonly IShopLeaveVoteObservation _shopLeaveVoteObservation;

		private readonly INetworkedModelScopeController _networkedModelScopeController;

		private readonly ISynchronizationGate _synchronizationGate;

		private readonly ISessionGateWindowPolicy _sessionGateWindowPolicy;

		private readonly IShopActivation _shopActivation;

		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		private readonly IPlayerAvatarLifecycle _playerAvatarLifecycle;

		private readonly MouseVisibilityModel _mouseVisibilityModel;

		private readonly IShopPlayerControl _shopPlayerControl;

		private readonly ISessionLoadingUi _sessionLoadingUi;

		private readonly ISessionLevelUi _sessionLevelUi;

		private readonly ILevelLoadObservation _levelLoadObservation;

		private readonly ILevelSelectionSource _levelSelectionSource;

		private readonly ISessionPlayerControl _sessionPlayerControl;

		private readonly ISessionVoiceControl _sessionVoiceControl;

		private readonly ISessionGameplayActivation _sessionGameplayActivation;

		private readonly IEnemySpawnGate _enemySpawnGate;

		private readonly IShopSeatHealthObservation _shopSeatHealthObservation;

		public override SessionState State => SessionState.Shop;

		public override SessionAuthorityLane ActivateLane => SessionAuthorityLane.ShopEnterActivate;

		public override SessionAuthorityLane DeactivateLane => SessionAuthorityLane.ShopExitDeactivate;

		public ShopState(IShopLeaveVoteObservation shopLeaveVoteObservation, INetworkedModelScopeController networkedModelScopeController, ISynchronizationGate synchronizationGate, ISessionGateWindowPolicy sessionGateWindowPolicy, IShopActivation shopActivation, ISessionPlayerPresence sessionPlayerPresence, IPlayerAvatarLifecycle playerAvatarLifecycle, MouseVisibilityModel mouseVisibilityModel, IShopPlayerControl shopPlayerControl, ISessionLoadingUi sessionLoadingUi, ISessionLevelUi sessionLevelUi, ILevelLoadObservation levelLoadObservation, ILevelSelectionSource levelSelectionSource, ISessionPlayerControl sessionPlayerControl, ISessionVoiceControl sessionVoiceControl, ISessionGameplayActivation sessionGameplayActivation, IEnemySpawnGate enemySpawnGate, IShopSeatHealthObservation shopSeatHealthObservation)
		{
			_shopLeaveVoteObservation = shopLeaveVoteObservation;
			_networkedModelScopeController = networkedModelScopeController;
			_synchronizationGate = synchronizationGate;
			_sessionGateWindowPolicy = sessionGateWindowPolicy;
			_shopActivation = shopActivation;
			_sessionPlayerPresence = sessionPlayerPresence;
			_playerAvatarLifecycle = playerAvatarLifecycle;
			_mouseVisibilityModel = mouseVisibilityModel;
			_shopPlayerControl = shopPlayerControl;
			_sessionLoadingUi = sessionLoadingUi;
			_sessionLevelUi = sessionLevelUi;
			_levelLoadObservation = levelLoadObservation;
			_levelSelectionSource = levelSelectionSource;
			_sessionPlayerControl = sessionPlayerControl;
			_sessionVoiceControl = sessionVoiceControl;
			_sessionGameplayActivation = sessionGameplayActivation;
			_enemySpawnGate = enemySpawnGate;
			_shopSeatHealthObservation = shopSeatHealthObservation;
		}

		public override async UniTask EnterAsync()
		{
			bool wasActiveOnEntry = base.IsTargetStateActive;
			_enemySpawnGate.SetSpawningAllowed(isAllowed: false);
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: false));
			_sessionLevelUi.Open();
			await AuthorityGate(SessionAuthorityLane.ShopOpenScope, async delegate(int epoch)
			{
				await _networkedModelScopeController.OpenGlobalScopeAsync(ModelScope.Shop);
				_shopActivation.ActivateShop();
				_synchronizationGate.OpenOnEntry(SynchronizationGateKey.ShopBodyReset, epoch, _sessionGateWindowPolicy.GenericWindowTicks);
				_synchronizationGate.OpenOnEntry(SynchronizationGateKey.EnterShop, epoch, _sessionGateWindowPolicy.GenericWindowTicks);
			});
			Debug.Log("[ShopEnterTrace] step=OpenLocalScope(Shop) begin");
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Shop);
			Debug.Log("[ShopEnterTrace] step=OpenLocalScope(Shop) done");
			Debug.Log("[ShopEnterTrace] step=OpenLocalScope(Run) begin");
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Run);
			Debug.Log("[ShopEnterTrace] step=OpenLocalScope(Run) done");
			Debug.Log("[ShopEnterTrace] step=Attach begin");
			await _sessionPlayerPresence.AttachAsync();
			Debug.Log("[ShopEnterTrace] step=Attach done");
			string levelName = _levelSelectionSource.SelectedLevelName;
			Debug.Log("[ShopEnterTrace] step=WaitUntilLoaded('" + levelName + "') begin");
			await _levelLoadObservation.WaitUntilLoadedAsync(levelName);
			Debug.Log("[ShopEnterTrace] step=WaitUntilLoaded done");
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
			Debug.Log("[ShopEnterTrace] step=EnsureSpawned begin");
			await _playerAvatarLifecycle.EnsureSpawnedAsync();
			Debug.Log("[ShopEnterTrace] step=EnsureSpawned done");
			if (wasActiveOnEntry)
			{
				await _sessionPlayerPresence.WaitForReplicatedStateAsync();
			}
			_sessionPlayerControl.ApplyPersistedLifeState();
			Debug.Log("[ShopEnterTrace] step=ReleaseGrabs begin");
			await _shopPlayerControl.ReleaseGrabsAsync();
			Debug.Log("[ShopEnterTrace] step=ReleaseGrabs done");
			Debug.Log("[ShopEnterTrace] step=EnsureBodyAuthority begin");
			await _shopPlayerControl.EnsureBodyAuthorityAsync();
			Debug.Log("[ShopEnterTrace] step=EnsureBodyAuthority done");
			Debug.Log("[ShopEnterTrace] step=ResetRagdoll begin");
			await _shopPlayerControl.ResetRagdollAsync();
			Debug.Log("[ShopEnterTrace] step=ResetRagdoll done");
			await SynchronizationGate(SynchronizationGateKey.ShopBodyReset);
			Debug.Log("[ShopEnterTrace] step=SeatAtStoreSeat begin");
			await _shopPlayerControl.SeatAtStoreSeatAsync();
			Debug.Log("[ShopEnterTrace] step=SeatAtStoreSeat done");
			await SynchronizationGate(SynchronizationGateKey.EnterShop);
			_sessionVoiceControl.RestartRecording();
			_sessionGameplayActivation.NotifySessionStarted();
			await _sessionLoadingUi.HideAsync();
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: true));
			await VerifyShopHealthAsync();
		}

		private async UniTask VerifyShopHealthAsync()
		{
			List<string> problems = new List<string>();
			if (!_sessionPlayerPresence.IsAttached)
			{
				problems.Add("not attached to its persistent player object");
			}
			if (!_playerAvatarLifecycle.IsAvatarHeld)
			{
				problems.Add("avatar not held");
			}
			if (_sessionLoadingUi.IsBlackoutRaised)
			{
				problems.Add("transition blackout still raised after the reveal");
			}
			string text = await PollLocalSeatSpendableAsync();
			bool isStoreTablePresent = _shopSeatHealthObservation.IsStoreTablePresent;
			if (text != null && isStoreTablePresent)
			{
				problems.Add(text);
			}
			if (problems.Count > 0)
			{
				Debug.LogError("[SessionHealth] Shop entry UNHEALTHY: " + string.Join("; ", problems));
			}
			else if (!isStoreTablePresent)
			{
				Debug.Log("[SessionHealth] Shop entry — attached and revealed; no store table in scene (state-machine transition without shop content), seat spendability not evaluated.");
			}
			else
			{
				Debug.Log("[SessionHealth] Shop entry healthy — attached, avatar held, seat spendable (box open, lamp lit).");
			}
		}

		private async UniTask<string> PollLocalSeatSpendableAsync()
		{
			string problem = "store seat never became spendable";
			for (int attempt = 0; attempt < 20; attempt++)
			{
				if (_shopSeatHealthObservation.TryVerifyLocalSeatSpendable(out problem))
				{
					return null;
				}
				await UniTask.Delay(TimeSpan.FromSeconds(0.10000000149011612));
			}
			return problem;
		}

		private void VerifyShopExitHealth()
		{
			if (_shopSeatHealthObservation.IsStoreTablePresent)
			{
				if (_shopSeatHealthObservation.TryVerifyLocalSeatedAvatarPresent(out var problem))
				{
					Debug.Log("[SessionHealth] Shop exit healthy — local avatar present at its store seat.");
				}
				else
				{
					Debug.LogError("[SessionHealth] Shop exit UNHEALTHY: seated player avatar not at its store seat: " + problem);
				}
			}
		}

		public override void UpdateActive()
		{
			_shopActivation.ActivateShop();
			if (_shopLeaveVoteObservation.HasEveryPresentPlayerVotedToLeave())
			{
				Transition(SessionState.Level);
			}
		}

		public override async UniTask ExitAsync(SessionState next)
		{
			VerifyShopExitHealth();
			await _sessionLoadingUi.ShowAsync();
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: true));
			_shopPlayerControl.LeaveStoreSeat();
			_networkedModelScopeController.CloseLocalScope(ModelScope.Shop);
			await AuthorityGate(SessionAuthorityLane.ShopReset, (Action)delegate
			{
				_shopActivation.DeactivateShop();
				_networkedModelScopeController.CloseGlobalScope(ModelScope.Shop);
			});
		}
	}
}
