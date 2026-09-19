using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.LevelModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.QuotaModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.StoreModule.Scripts.MovedToBeachLocal;
using Fusion;
using NetworkServices.NetworkEvents;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreActivationComponent : MonoBehaviour, IStoreSeater
	{
		private const float BODY_AUTHORITY_WAIT_SECONDS = 10f;

		[SerializeField]
		private List<Transform> _pointsForCamera;

		[SerializeField]
		private Transform _target;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private Transform _pointForItemSpawn;

		[SerializeField]
		protected StoreTableBehaviour _storeTableBehaviour;

		[SerializeField]
		private List<GameObject> _objectsToDisableAfterStore;

		[SerializeField]
		private List<GameObject> _roomVisuals;

		private CameraModel _cameraModel;

		private IPlayerStateService _playerStateService;

		private MultiplayerModel _multiplayerModel;

		protected StoreReadyModel _storeReadyModel;

		private IStoreReadyRoster _storeReadyRoster;

		protected IShopVotePersistence _shopVotePersistence;

		private StoreRewardModel _storeRewardModel;

		private PlayerState _lastPlayerState;

		private CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private StoreDraftMoneyModel _storeDraftMoneyModel;

		private TeleportationPointsEventClass _teleportationPointsEventClass;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private MovedToBeachEventClass _movedToBeachEventClass;

		private LineArmsModel _lineArmsModel;

		private PlayerReboundModel _playerReboundModel;

		private CardsOnTableModel _cardsOnTableModel;

		private StorePhaseModel _storePhaseModel;

		private PlayerMovableModel _playerMovableModel;

		private NetworkRunnerEventBus _networkRunnerEventBus;

		private IStoreSeatingService _storeSeatingService;

		private StoreSeaterRegistry _storeSeaterRegistry;

		private PlayersRagdollModel _playersRagdollModel;

		private bool _localPlayerActivated;

		private int _currentSeatIndex = -1;

		private bool _seatingCancelled;

		private float _seatRosterSettleDeadline;

		private bool _areStoreVisualsVisible;

		private const float SEAT_ROSTER_SETTLE_TIMEOUT = 3f;

		private const float SEAT_READY_WAIT_SECONDS = 30f;

		public SimplePointGrabable SimplePointGrabable => _simplePointGrabable;

		public bool IsReadyBlocked { get; set; }

		[Inject]
		public void InjectDependencies(CameraModel cameraModel, IPlayerStateService playerStateService, MultiplayerModel multiplayerModel, StoreReadyModel storeReadyModel, IStoreReadyRoster storeReadyRoster, StoreRewardModel storeRewardModel, CurrentWalletSynchronizedModel currentWalletSynchronizedModel, StoreDraftMoneyModel storeDraftMoneyModel, TeleportationPointsEventClass teleportationPointsEventClass, SpawnedEntityStatsModel spawnedEntityStatsModel, MovedToBeachEventClass movedToBeachEventClass, LineArmsModel lineArmsModel, PlayerReboundModel playerReboundModel, CardsOnTableModel cardsOnTableModel, StorePhaseModel storePhaseModel, PlayerMovableModel playerMovableModel, NetworkRunnerEventBus networkRunnerEventBus, IStoreSeatingService storeSeatingService, StoreSeaterRegistry storeSeaterRegistry, IShopVotePersistence shopVotePersistence, PlayersRagdollModel playersRagdollModel)
		{
			_playersRagdollModel = playersRagdollModel;
			_cameraModel = cameraModel;
			_playerStateService = playerStateService;
			_multiplayerModel = multiplayerModel;
			_storeReadyModel = storeReadyModel;
			_storeReadyRoster = storeReadyRoster;
			_storeRewardModel = storeRewardModel;
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_teleportationPointsEventClass = teleportationPointsEventClass;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_movedToBeachEventClass = movedToBeachEventClass;
			_lineArmsModel = lineArmsModel;
			_playerReboundModel = playerReboundModel;
			_cardsOnTableModel = cardsOnTableModel;
			_storePhaseModel = storePhaseModel;
			_playerMovableModel = playerMovableModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_storeSeatingService = storeSeatingService;
			_storeSeaterRegistry = storeSeaterRegistry;
			_shopVotePersistence = shopVotePersistence;
		}

		private void Awake()
		{
			ConfigureStoreTable();
			ApplyStoreVisuals(isVisible: false);
		}

		private void OnEnable()
		{
			_simplePointGrabable.LocalOnGrab += OnRingGrabbed;
			_storeReadyRoster.OnPlayerIdsChanged += StoreReadyModelOnPlayerIdsChanged;
			_storeRewardModel.OnPendingRewardsChanged += ApplyReadyRewards;
			_playerReboundModel.OnPlayerRebound += HandlePlayerRebound;
			LineArmsModel lineArmsModel = _lineArmsModel;
			lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Combine(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(HandleLineArmRegistered));
			_storePhaseModel.OnStorePhaseChanged += HandleStorePhaseChanged;
			_storePhaseModel.AttachmentChanged += HandleStorePhaseAttachmentChanged;
			_cardsOnTableModel.OnCardRegistered += HandleCardRegistered;
			_teleportationPointsEventClass.OnPlayerTeleported += HandlePlayerTeleportedToBeach;
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
			ApplyStoreVisuals(IsStoreRoomVisible());
			_storeSeaterRegistry.Register(this);
		}

		protected virtual void OnRingGrabbed(int i)
		{
			if (!IsReadyBlocked)
			{
				SetReady();
			}
		}

		private void OnDisable()
		{
			_simplePointGrabable.LocalOnGrab -= OnRingGrabbed;
			_storeReadyRoster.OnPlayerIdsChanged -= StoreReadyModelOnPlayerIdsChanged;
			_storeRewardModel.OnPendingRewardsChanged -= ApplyReadyRewards;
			_playerReboundModel.OnPlayerRebound -= HandlePlayerRebound;
			LineArmsModel lineArmsModel = _lineArmsModel;
			lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Remove(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(HandleLineArmRegistered));
			_storePhaseModel.OnStorePhaseChanged -= HandleStorePhaseChanged;
			_storePhaseModel.AttachmentChanged -= HandleStorePhaseAttachmentChanged;
			_cardsOnTableModel.OnCardRegistered -= HandleCardRegistered;
			_teleportationPointsEventClass.OnPlayerTeleported -= HandlePlayerTeleportedToBeach;
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
			_storeSeaterRegistry.Unregister(this);
			if (_areStoreVisualsVisible)
			{
				ApplyStoreVisuals(isVisible: false);
			}
		}

		private void HandleStorePhaseChanged(bool isActive)
		{
			if (isActive)
			{
				ApplyStoreVisuals(isVisible: true);
			}
			else
			{
				ClearStoreSeat();
			}
		}

		private void HandleStorePhaseAttachmentChanged(bool isAttached)
		{
			if (isAttached && _storePhaseModel.IsStoreActive.Value)
			{
				ApplyStoreVisuals(isVisible: true);
			}
			else if (!isAttached)
			{
				ClearStoreSeat();
			}
		}

		private void ClearStoreSeat()
		{
			_localPlayerActivated = false;
			_currentSeatIndex = -1;
		}

		private void HandlePlayerTeleportedToBeach()
		{
			if (_areStoreVisualsVisible)
			{
				ApplyStoreVisuals(isVisible: false);
			}
		}

		private bool IsStoreRoomVisible()
		{
			if (_storePhaseModel != null && _storePhaseModel.IsAttached)
			{
				return _storePhaseModel.IsStoreActive.Value;
			}
			return false;
		}

		private void ApplyStoreVisuals(bool isVisible)
		{
			_areStoreVisualsVisible = isVisible;
			if (_roomVisuals != null)
			{
				foreach (GameObject roomVisual in _roomVisuals)
				{
					if (!(roomVisual == null))
					{
						MeshRenderer[] componentsInChildren = roomVisual.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							componentsInChildren[i].enabled = isVisible;
						}
					}
				}
			}
			if (_cardsOnTableModel == null)
			{
				return;
			}
			foreach (StoreCardBehaviour item in _cardsOnTableModel.CardsOnTable)
			{
				ApplyCardVisuals(item, isVisible);
			}
		}

		private void HandleCardRegistered(StoreCardBehaviour card)
		{
			ApplyCardVisuals(card, _areStoreVisualsVisible);
		}

		private void ApplyCardVisuals(StoreCardBehaviour card, bool isVisible)
		{
			if (!(card == null))
			{
				card.SetVisualsEnabled(isVisible);
			}
		}

		public void ReleaseGrabs()
		{
			ReleaseLocalArmsGrab();
			ForceLocalUnGrabRecovery();
		}

		public async UniTask EnsureBodyAuthorityAsync()
		{
			PlayerRagdollEntity playerRagdollEntity = await WaitForLocalRagdollAsync();
			if (playerRagdollEntity.HasStateAuthority)
			{
				return;
			}
			playerRagdollEntity.Object.RequestStateAuthority();
			float deadline = Time.unscaledTime + 10f;
			while (Time.unscaledTime < deadline)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (playerRagdollEntity == null || playerRagdollEntity.HasStateAuthority)
				{
					return;
				}
			}
			throw new StoreSeatingStepFailedException($"EnsureBodyAuthority: body state authority did not return within {10f}s — " + "the ragdoll reset and the seat placement that follow would be written by a peer that does not own the body.");
		}

		public void ResetRagdoll()
		{
			if (!_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				throw new StoreSeatingStepFailedException("ResetRagdoll: this peer has no registered ragdoll entity — the body cannot be put into a known state, and the seat placement that follows would run against whatever the ragdoll was doing.");
			}
			ragdoll.ForceRecoverInPlace();
		}

		private async UniTask<PlayerRagdollEntity> WaitForLocalRagdollAsync()
		{
			int localPlayerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			float deadline = Time.unscaledTime + 10f;
			while (Time.unscaledTime < deadline)
			{
				if (_playersRagdollModel.TryGetPlayerRagdoll(localPlayerId, out var ragdoll))
				{
					return ragdoll;
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
			throw new StoreSeatingStepFailedException($"EnsureBodyAuthority: this peer's ragdoll entity was not registered within {10f}s — " + "there is no body to take authority of, so nothing after this step can enforce its state.");
		}

		public async UniTask SeatAtStoreSeatAsync()
		{
			_seatRosterSettleDeadline = Time.unscaledTime + 3f;
			_seatingCancelled = false;
			float deadline = Time.unscaledTime + 30f;
			string activationBlockReason = GetActivationBlockReason();
			float nextTraceAt = Time.unscaledTime;
			while (activationBlockReason != null)
			{
				if (Time.unscaledTime >= nextTraceAt)
				{
					Debug.Log("[ShopEnterTrace] SeatAtStoreSeat blocked: " + activationBlockReason);
					nextTraceAt = Time.unscaledTime + 2f;
				}
				if (Time.unscaledTime >= deadline)
				{
					throw new StoreSeatingStepFailedException($"SeatAtStoreSeat: still blocked after {30f}s — {activationBlockReason}");
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (_seatingCancelled)
				{
					throw new StoreSeatingStepFailedException("SeatAtStoreSeat: seating was cancelled while the step was holding (shop left or session torn down) — the body was never placed.");
				}
				if (this == null || !base.isActiveAndEnabled)
				{
					throw new StoreSeatingStepFailedException("SeatAtStoreSeat: the store table was torn down while the seat step was holding — the body was never placed, and returning quietly here would report a seat that does not exist.");
				}
				activationBlockReason = GetActivationBlockReason();
			}
			ResyncCardsOnTable();
			await ActivateLocalPlayerStoreAsync();
			_localPlayerActivated = true;
		}

		public void CancelSeating()
		{
			_seatingCancelled = true;
		}

		public void RestorePreStoreState()
		{
			if (_lastPlayerState == PlayerState.Dead || _lastPlayerState == PlayerState.PreDeadCrouch)
			{
				_playerStateService.ChangePlayerState(_lastPlayerState);
			}
			_lastPlayerState = PlayerState.None;
		}

		private void ConfigureStoreTable()
		{
			int num = 0;
			int num2 = 0;
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				PlayerState playerState = _playerStateService.GetPlayerState(activePlayer.PlayerId);
				if (_playerStateService.IsPlayerDead(activePlayer.PlayerId) || playerState == PlayerState.PreDeadCrouch)
				{
					num++;
				}
				if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(activePlayer.PlayerId, out var value))
				{
					IStat stat = value.GetStat(EntityStatType.Health);
					if (stat.FullValue < stat.MaxValue)
					{
						num2++;
					}
				}
			}
			_storeTableBehaviour.SetDeadPlayers(num);
			_storeTableBehaviour.SetNotFullHpPlayersCount(num2);
		}

		private void HandlePlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			if (!(playerRef != _multiplayerModel.NetworkRunner.LocalPlayer) && avatar == null)
			{
				_localPlayerActivated = false;
				_currentSeatIndex = -1;
			}
		}

		private void HandleLineArmRegistered(int playerId)
		{
			if (playerId != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				RefreshStoreSeat();
			}
		}

		private void HandlePlayerLeft(OnPlayerLeftEvent playerLeftEvent)
		{
			RefreshStoreSeat();
		}

		private string GetActivationBlockReason()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return "runner-not-running";
			}
			if (!_cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.CardTableCamera))
			{
				return "no-card-table-camera";
			}
			int playerId = networkRunner.LocalPlayer.PlayerId;
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer == null || !allLineArmsForPlayer.TryGetValue(LineArmType.RightArmDefault, out var value) || !allLineArmsForPlayer.TryGetValue(LineArmType.RightArmByMouse, out value))
			{
				return "arms-missing";
			}
			int seatIndex = _storeSeatingService.GetSeatIndex(playerId);
			if (seatIndex < 0 || seatIndex >= _pointsForCamera.Count)
			{
				return $"seat-invalid({seatIndex}/{_pointsForCamera.Count})";
			}
			if (!AllActivePlayersHaveResolvedSeat() && Time.unscaledTime < _seatRosterSettleDeadline)
			{
				return "seat-roster-incomplete";
			}
			return null;
		}

		private bool AllActivePlayersHaveResolvedSeat()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (_storeSeatingService.GetSeatIndex(activePlayer.PlayerId) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private void ResyncCardsOnTable()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			foreach (NetworkObject allNetworkObject in networkRunner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid)
				{
					StoreCardBehaviour component = allNetworkObject.GetComponent<StoreCardBehaviour>();
					if (component != null)
					{
						_cardsOnTableModel.RegisterCard(component);
					}
				}
			}
		}

		private async UniTask ActivateLocalPlayerStoreAsync()
		{
			int seatIndex = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			_lastPlayerState = _playerStateService.GetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (_lastPlayerState != PlayerState.Dead && _lastPlayerState != PlayerState.PreDeadCrouch)
			{
				_lastPlayerState = ((!IsLocalPlayerDeadByHealth()) ? PlayerState.Alive : PlayerState.Dead);
			}
			await ApplyStoreSeatAsync(seatIndex);
			_playerStateService.ChangePlayerState(PlayerState.Store);
			EnableStoreArmControls();
			RestoreStoreReadyState();
		}

		private void ReleaseLocalArmsGrab()
		{
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer == null)
			{
				return;
			}
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null))
				{
					value.EnableGrabbing(enable: false);
					value.UnJoinAll(throwItem: false);
				}
			}
		}

		private void ForceLocalUnGrabRecovery()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
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

		private void RefreshStoreSeat()
		{
			if (!_localPlayerActivated || !_storePhaseModel.IsStoreActive.Value)
			{
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning)
			{
				int seatIndex = _storeSeatingService.GetSeatIndex(networkRunner.LocalPlayer.PlayerId);
				if (seatIndex >= 0 && seatIndex != _currentSeatIndex)
				{
					ApplyStoreSeatAsync(seatIndex).Forget();
					RefreshLocalStoreArmGrabbing();
				}
			}
		}

		private void RefreshLocalStoreArmGrabbing()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer != null && allLineArmsForPlayer.TryGetValue(LineArmType.RightArmByMouse, out var value))
			{
				bool flag = _storeReadyRoster.IsPlayerReady(playerId);
				value.UnJoinAll(throwItem: false);
				value.enabled = true;
				value.EnableGrabbing(!flag);
			}
		}

		private async UniTask ApplyStoreSeatAsync(int seatIndex)
		{
			if (seatIndex < 0 || seatIndex >= _pointsForCamera.Count)
			{
				throw new StoreSeatingStepFailedException($"ApplyStoreSeat: seat ordinal {seatIndex} is outside the table's {_pointsForCamera.Count} seats — there is nowhere to place the body.");
			}
			_currentSeatIndex = seatIndex;
			_cameraModel.Cameras[Features.CameraModelModule.CameraType.CardTableCamera].transform.position = _pointsForCamera[seatIndex].position;
			_cameraModel.Cameras[Features.CameraModelModule.CameraType.CardTableCamera].SetTrackingTarget(_target);
			await TeleportLocalBodyToStoreSeatAsync(seatIndex);
		}

		private async UniTask TeleportLocalBodyToStoreSeatAsync(int seatIndex)
		{
			if (!_storeTableBehaviour.TryGetSeatPose(seatIndex, out var seatPosition, out var seatRotation))
			{
				throw new StoreSeatingStepFailedException($"Teleport: the store table has no pose for seat {seatIndex} — the body cannot be placed.");
			}
			float deadline = Time.unscaledTime + 10f;
			while (PlayerSpawnLock.IsFirstSpawnInProgress || _playerMovableModel.LocalMovable == null || !_playerMovableModel.LocalMovable.HasStateAuthority)
			{
				if (Time.unscaledTime >= deadline)
				{
					throw new StoreSeatingStepFailedException($"Teleport: the body was not placeable within {10f}s " + $"(firstSpawnInProgress={PlayerSpawnLock.IsFirstSpawnInProgress}, " + "movable=" + ((_playerMovableModel.LocalMovable == null) ? "null" : "present") + ", " + $"hasAuthority={_playerMovableModel.LocalMovable != null && _playerMovableModel.LocalMovable.HasStateAuthority}) — " + "the seat would have been reported without the body ever moving.");
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
			_teleportationPointsEventClass.InvokeCancelPendingTeleports();
			_playerMovableModel.LocalMovable.SetLinearVelocity(Vector3.zero);
			_playerMovableModel.LocalMovable.ChangePosition(seatPosition, seatRotation, isForced: true);
			_playerMovableModel.LocalMovable.SetLinearVelocity(Vector3.zero);
		}

		private void EnableStoreArmControls()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer != null)
			{
				if (allLineArmsForPlayer.TryGetValue(LineArmType.RightArmDefault, out var value))
				{
					value.UnJoinAll(throwItem: false);
					value.enabled = false;
				}
				if (allLineArmsForPlayer.TryGetValue(LineArmType.RightArmByMouse, out var value2))
				{
					value2.enabled = true;
					value2.EnableGrabbing(enable: true);
				}
			}
		}

		protected virtual void SetReady()
		{
			if (_simplePointGrabable.GrabbedByPlayers.Contains(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) && !IsReadyBlocked)
			{
				_storeReadyModel.SetReady(isReady: true);
				_shopVotePersistence.SaveVote(hasVoted: true);
			}
		}

		private void RestoreStoreReadyState()
		{
			if (_shopVotePersistence.HasVoteForCurrentShop())
			{
				_storeReadyModel.SetReady(isReady: true);
			}
		}

		private void StoreReadyModelOnPlayerIdsChanged()
		{
			ApplyReadyRewards();
			if (AreAllActivePlayersReady())
			{
				ApplyStoreExitRewardsAndCleanup();
			}
		}

		private bool AreAllActivePlayersReady()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			int num = 0;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				num++;
				if (!_storeReadyRoster.IsPlayerReady(activePlayer.PlayerId))
				{
					return false;
				}
			}
			return num > 0;
		}

		private void ApplyReadyRewards()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (int readyPlayer in _storeReadyRoster.ReadyPlayers)
			{
				ApplyReadyRewardsForPlayer(readyPlayer);
			}
		}

		private async void ApplyReadyRewardsForPlayer(int readyPlayerId)
		{
			List<StoreRewardRequest> pendingRewards = _storeRewardModel.PendingRewards;
			for (int index = 0; index < pendingRewards.Count; index++)
			{
				StoreRewardRequest storeRewardRequest = pendingRewards[index];
				if (storeRewardRequest.TargetPlayerId == readyPlayerId && !(storeRewardRequest.StoreCard == null) && storeRewardRequest.StoreCard.CardData != null)
				{
					StoreRewardContext context = new StoreRewardContext
					{
						TargetPlayerId = storeRewardRequest.TargetPlayerId,
						SpawnPosition = _pointForItemSpawn.position,
						SpawnRotation = _pointForItemSpawn.rotation,
						CardData = storeRewardRequest.StoreCard.CardData
					};
					await storeRewardRequest.StoreCard.ApplyReward(context, StoreRewardApplyMoment.OnPlayerReadyInStore);
				}
			}
		}

		private void ApplyStoreExitRewardsAndCleanup()
		{
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer != null && allLineArmsForPlayer.TryGetValue(LineArmType.RightArmByMouse, out var value))
			{
				value.EnableGrabbing(enable: true);
			}
			ApplyReadyRewards();
			_playerStateService.SetStateChangeBlocked(isBlocked: false);
			PlayerSpawnLock.CompleteSpawn();
			_shopVotePersistence.SaveVote(hasVoted: false);
			_storeReadyModel.SetReady(isReady: false);
			_storeTableBehaviour.Deactivate();
			_currentWalletSynchronizedModel.RemoveQuota(_storeDraftMoneyModel.DraftMoney);
			_storeDraftMoneyModel.SetDraftMoney(0f);
			_movedToBeachEventClass.InvokeLocalPlayerMovedToBeach();
			_objectsToDisableAfterStore.ForEach(delegate(GameObject x)
			{
				if (x != null)
				{
					x.SetActive(value: false);
				}
			});
		}

		private bool IsLocalPlayerDeadByHealth()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				return false;
			}
			return value.GetStat(EntityStatType.Health).FullValue <= 0f;
		}
	}
}
