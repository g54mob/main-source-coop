using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using Features.EmotesModule.Scripts;
using Features.GrabModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Data;
using Features.PlayerSpawner.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class KrakenController : NetworkBehaviour, IKrakenStateMachine, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private KrakenItemDetector _headDetector;

		[SerializeField]
		private KrakenItemDetector _krakenItemDetector;

		[SerializeField]
		private KrakenTentacleEmotesController _tentacleEmotesController;

		[SerializeField]
		private TentacleController[] _tentacles;

		[SerializeField]
		[Range(0f, 1f)]
		private float _reactionChance = 0.7f;

		[SerializeField]
		private float _reactionCooldown = 1f;

		[SerializeField]
		private float _hitCooldown = 2f;

		[SerializeField]
		private float _danceResponseRadius = 15f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _clapChance = 0.5f;

		[SerializeField]
		private float _clapCooldown = 10f;

		[SerializeField]
		private KrakenAggressiveThrowSystem _krakenAggressiveThrowSystem;

		[SerializeField]
		private Transform _aggressiveRockSpawnPoint;

		[SerializeField]
		private Transform _helpDeadPartSpawnPoint;

		[SerializeField]
		private KrakenBehaviourConfiguration _behaviourConfigurationOverride;

		[SerializeField]
		private KrakenAppearTentacleController _appearTentacleController;

		private PlayerBodyEmoteNetworkEvent _playerBodyEmoteNetworkEvent;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		private KrakenPlayerThrowTrackerModel _krakenPlayerThrowTrackerModel;

		private KrakenAggressiveThrowConfiguration _krakenAggressiveThrowConfiguration;

		private KrakenRuntimeModel _krakenRuntimeModel;

		private IKrakenThrowAssignmentService _krakenThrowAssignmentService;

		private KrakenBehaviourConfiguration _krakenBehaviourConfiguration;

		private SessionStateMachine _sessionStateMachine;

		private float _lastClapTime = float.NegativeInfinity;

		private float _lastReactionTime = float.NegativeInfinity;

		private float _lastHitTime = float.NegativeInfinity;

		private float _stateEnteredTime;

		private float _inactivityHideAt = float.PositiveInfinity;

		private bool _hideRequestedAfterCurrentAction;

		private TentacleEmoteType _pendingAppearEmote;

		private Coroutine _pendingAppearEmoteCoroutine;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private KrakenStateId _NetworkedState;

		[WeaverGenerated]
		[DefaultForProperty("IsInitialized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInitialized;

		[WeaverGenerated]
		[DefaultForProperty("DanceResponsePlayerId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DanceResponsePlayerId;

		[WeaverGenerated]
		[DefaultForProperty("ActiveDanceResponseEmote", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TentacleEmoteType _ActiveDanceResponseEmote;

		public bool IsIdle
		{
			get
			{
				if (CurrentState == KrakenStateId.Idle && !IsAnyTentacleBusy())
				{
					return !_tentacleEmotesController.IsPlayingEmote;
				}
				return false;
			}
		}

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe KrakenStateId NetworkedState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.NetworkedState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(KrakenStateId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.NetworkedState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(KrakenStateId*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe bool IsInitialized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.IsInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.IsInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int DanceResponsePlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.DanceResponsePlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.DanceResponsePlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe TentacleEmoteType ActiveDanceResponseEmote
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.ActiveDanceResponseEmote. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (TentacleEmoteType)Ptr[3];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenController.ActiveDanceResponseEmote. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = (int)value;
			}
		}

		public KrakenItemDetector ItemDetector => _krakenItemDetector;

		public Vector3 HelpDeadPartSpawnPosition
		{
			get
			{
				if (!(_helpDeadPartSpawnPoint != null))
				{
					return base.transform.position;
				}
				return _helpDeadPartSpawnPoint.position;
			}
		}

		public KrakenStateId CurrentState => NetworkedState;

		public bool CanAcceptInteraction => IsIdle;

		public bool CanAcceptItemThrow
		{
			get
			{
				if (CurrentState == KrakenStateId.Idle || CurrentState == KrakenStateId.Catching)
				{
					return !_tentacleEmotesController.IsPlayingEmote;
				}
				return false;
			}
		}

		public bool IsVisible
		{
			get
			{
				if (CurrentState != KrakenStateId.Hidden)
				{
					return CurrentState != KrakenStateId.Disappearing;
				}
				return false;
			}
		}

		[Inject]
		private void InjectDependencies(PlayerBodyEmoteNetworkEvent playerBodyEmoteNetworkEvent, SpawnedPlayersModel spawnedPlayersModel, IPlayerDeadPartSpawnService playerDeadPartSpawnService, KrakenPlayerThrowTrackerModel krakenPlayerThrowTrackerModel, KrakenAggressiveThrowConfiguration krakenAggressiveThrowConfiguration, KrakenRuntimeModel krakenRuntimeModel, IKrakenThrowAssignmentService krakenThrowAssignmentService, KrakenBehaviourConfiguration krakenBehaviourConfiguration, SessionStateMachine sessionStateMachine)
		{
			_playerBodyEmoteNetworkEvent = playerBodyEmoteNetworkEvent;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
			_krakenPlayerThrowTrackerModel = krakenPlayerThrowTrackerModel;
			_krakenAggressiveThrowConfiguration = krakenAggressiveThrowConfiguration;
			_krakenRuntimeModel = krakenRuntimeModel;
			_krakenThrowAssignmentService = krakenThrowAssignmentService;
			_krakenBehaviourConfiguration = ((_behaviourConfigurationOverride != null) ? _behaviourConfigurationOverride : krakenBehaviourConfiguration);
			_sessionStateMachine = sessionStateMachine;
		}

		public override void Spawned()
		{
			_krakenRuntimeModel.SetController(this);
			_appearTentacleController?.BindController(this);
			if (base.HasStateAuthority && !IsInitialized)
			{
				IsInitialized = true;
				DanceResponsePlayerId = -1;
				ActiveDanceResponseEmote = TentacleEmoteType.None;
				SetState(KrakenStateId.Idle);
			}
			SyncStateAuthority();
		}

		public void StateAuthorityChanged()
		{
			SyncStateAuthority();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			SyncStateAuthority();
			_krakenRuntimeModel.ClearController(this);
			ClearPendingAppearEmote();
		}

		private void SyncStateAuthority()
		{
			if (!base.HasStateAuthority)
			{
				UnsubscribeHeadDetector();
				return;
			}
			_stateEnteredTime = Time.time;
			_hideRequestedAfterCurrentAction = false;
			ResetInactivityTimer();
			SubscribeHeadDetector();
			_appearTentacleController?.SyncWithKrakenState(CurrentState);
		}

		private void SubscribeHeadDetector()
		{
			if (!(_headDetector == null))
			{
				_headDetector.OnGrabbableDetected -= ProcessHit;
				_headDetector.OnGrabbableDetected += ProcessHit;
			}
		}

		private void UnsubscribeHeadDetector()
		{
			if (!(_headDetector == null))
			{
				_headDetector.OnGrabbableDetected -= ProcessHit;
			}
		}

		private void ProcessHit(IPointGrabable pointGrabable)
		{
			if (CanPlayHit() && !_krakenThrowAssignmentService.IsItemAlreadyAssigned(_tentacles, pointGrabable))
			{
				_lastHitTime = Time.time;
				_tentacleEmotesController.PlayEmote(TentacleEmoteType.Hit);
			}
		}

		private bool CanPlayHit()
		{
			return Time.time - _lastHitTime >= _hitCooldown;
		}

		private void Update()
		{
			if (base.HasStateAuthority)
			{
				UpdateTimedStateTransitions();
				MaintainDanceResponseEmote();
				CompleteInteractionWhenIdle();
			}
		}

		private void UpdateTimedStateTransitions()
		{
			if (CurrentState == KrakenStateId.Appearing && Time.time - _stateEnteredTime >= GetStateAnimationFallbackSeconds())
			{
				SetState(KrakenStateId.Idle);
				ResetInactivityTimer();
				ApplyVisibilityState(KrakenStateId.Idle);
				SchedulePendingAppearEmote();
			}
			else if (CurrentState == KrakenStateId.Disappearing && Time.time - _stateEnteredTime >= GetStateAnimationFallbackSeconds())
			{
				SetState(KrakenStateId.Hidden);
				ApplyVisibilityState(KrakenStateId.Hidden);
			}
			else if (!(Time.time < _inactivityHideAt))
			{
				if (CurrentState == KrakenStateId.Idle)
				{
					RequestHide();
				}
				else if (IsActionState(CurrentState))
				{
					_hideRequestedAfterCurrentAction = true;
				}
			}
		}

		private void CompleteInteractionWhenIdle()
		{
			if ((CurrentState == KrakenStateId.Catching || CurrentState == KrakenStateId.Throwing || CurrentState == KrakenStateId.HelpThrow || CurrentState == KrakenStateId.AggressiveThrow || CurrentState == KrakenStateId.Emoting) && DanceResponsePlayerId < 0 && !_tentacleEmotesController.IsPlayingEmote && !IsAnyTentacleBusy())
			{
				CompleteInteraction();
			}
		}

		private void MaintainDanceResponseEmote()
		{
			if (DanceResponsePlayerId >= 0 && ActiveDanceResponseEmote != TentacleEmoteType.None && !_tentacleEmotesController.IsPlayingEmote && (CurrentState == KrakenStateId.Emoting || TryEnterInteractionState(KrakenStateId.Emoting)))
			{
				_tentacleEmotesController.PlayEmote(ActiveDanceResponseEmote);
			}
		}

		private static bool IsActionState(KrakenStateId stateId)
		{
			if (stateId != KrakenStateId.Catching && stateId != KrakenStateId.Throwing && stateId != KrakenStateId.HelpThrow && stateId != KrakenStateId.AggressiveThrow)
			{
				return stateId == KrakenStateId.Emoting;
			}
			return true;
		}

		public bool RequestAppear(KrakenAppearReason reason)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (CurrentState == KrakenStateId.Appearing || CurrentState == KrakenStateId.Idle)
			{
				if (IsQuotaAppearReason(reason))
				{
					QueueOrPlayAppearEmote();
				}
				ResetInactivityTimer();
				return true;
			}
			if (CurrentState != KrakenStateId.Hidden && CurrentState != KrakenStateId.Disappearing)
			{
				return false;
			}
			if (IsQuotaAppearReason(reason))
			{
				_pendingAppearEmote = ChooseQuotaAppearEmote();
			}
			SetState(KrakenStateId.Appearing);
			ResetInactivityTimer();
			_hideRequestedAfterCurrentAction = false;
			ApplyVisibilityState(KrakenStateId.Appearing);
			return true;
		}

		public bool RequestHide()
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (IsActionState(CurrentState))
			{
				_hideRequestedAfterCurrentAction = true;
				return false;
			}
			if (CurrentState != KrakenStateId.Idle)
			{
				return false;
			}
			ClearPendingAppearEmote();
			SetState(KrakenStateId.Disappearing);
			_hideRequestedAfterCurrentAction = false;
			ApplyVisibilityState(KrakenStateId.Disappearing);
			return true;
		}

		public bool TryEnterInteractionState(KrakenStateId stateId)
		{
			if (!base.HasStateAuthority || !CanAcceptInteraction)
			{
				return false;
			}
			SetState(stateId);
			ResetInactivityTimer();
			return true;
		}

		public void CompleteInteraction(KrakenStateId nextState = KrakenStateId.Idle)
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			SetState(nextState);
			if (nextState == KrakenStateId.Idle && (_hideRequestedAfterCurrentAction || Time.time >= _inactivityHideAt))
			{
				_hideRequestedAfterCurrentAction = false;
				RequestHide();
				return;
			}
			ResetInactivityTimer();
			ApplyVisibilityState(nextState);
			if (nextState == KrakenStateId.Idle)
			{
				SchedulePendingAppearEmote();
			}
		}

		public void ResetInactivityTimer()
		{
			float b = ((_krakenBehaviourConfiguration != null) ? _krakenBehaviourConfiguration.InactivityHideDelay : 30f);
			_inactivityHideAt = Time.time + Mathf.Max(0.01f, b);
		}

		public void NotifyInteractableTentacleActivated()
		{
			if (RequestAppear(KrakenAppearReason.InteractableTentacle))
			{
				ResetInactivityTimer();
			}
		}

		public void ProcessItemDetected(IPointGrabable pointGrabable)
		{
			if (base.HasStateAuthority && _sessionStateMachine.Target.Value == SessionState.Level && _sessionStateMachine.IsTargetStateActive && CanAcceptItemThrow && !_tentacleEmotesController.IsPlayingEmote && NetworkedState != KrakenStateId.Emoting && _krakenThrowAssignmentService.IsGrabbableValid(pointGrabable) && !_krakenThrowAssignmentService.IsItemAlreadyAssigned(_tentacles, pointGrabable))
			{
				RecordPlayerThrow(pointGrabable);
				ProcessItemThrow(pointGrabable);
			}
		}

		private bool TryPlayReactionEmote()
		{
			if (!CanPlayReaction())
			{
				return false;
			}
			_lastReactionTime = Time.time;
			_tentacleEmotesController.PlayEmote(ChooseReactionEmote());
			return true;
		}

		private TentacleEmoteType ChooseReactionEmote()
		{
			if (!(UnityEngine.Random.value < 0.5f))
			{
				return TentacleEmoteType.Reaction;
			}
			return TentacleEmoteType.HitReaction;
		}

		private bool CanPlayReaction()
		{
			return Time.time - _lastReactionTime >= _reactionCooldown;
		}

		public void ProcessPlayerBodyEmote(PlayerBodyEmoteNetworkEvent playerBodyEmoteNetworkEvent)
		{
			if (base.HasStateAuthority)
			{
				if (!playerBodyEmoteNetworkEvent.IsStarted)
				{
					StopDanceResponse(playerBodyEmoteNetworkEvent.PlayerId);
				}
				else if (playerBodyEmoteNetworkEvent.BodyEmoteType != BodyEmoteType.None && CanAcceptInteraction && !_tentacleEmotesController.IsPlayingEmote && !IsAnyTentacleBusy() && IsPlayerNearKraken(playerBodyEmoteNetworkEvent.PlayerId) && TryEnterInteractionState(KrakenStateId.Emoting))
				{
					ActiveDanceResponseEmote = ChooseDanceResponseEmote();
					_tentacleEmotesController.PlayEmote(ActiveDanceResponseEmote);
					DanceResponsePlayerId = playerBodyEmoteNetworkEvent.PlayerId;
				}
			}
		}

		public void HandlePlayerDisconnected(PlayerRef player)
		{
			if (base.HasStateAuthority)
			{
				StopDanceResponse(player.PlayerId);
				TentacleController[] tentacles = _tentacles;
				for (int i = 0; i < tentacles.Length; i++)
				{
					tentacles[i].HandleTargetPlayerDisconnected(player);
				}
			}
		}

		private void StopDanceResponse(int playerId)
		{
			if (DanceResponsePlayerId == playerId)
			{
				_tentacleEmotesController.StopEmote();
				DanceResponsePlayerId = -1;
				ActiveDanceResponseEmote = TentacleEmoteType.None;
				_hideRequestedAfterCurrentAction = false;
				ResetInactivityTimer();
				CompleteInteraction();
			}
		}

		private TentacleEmoteType ChooseDanceResponseEmote()
		{
			if (UnityEngine.Random.value >= _clapChance || !CanPlayClap())
			{
				return TentacleEmoteType.Dance;
			}
			_lastClapTime = Time.time;
			return TentacleEmoteType.Clap;
		}

		private void QueueOrPlayAppearEmote()
		{
			_pendingAppearEmote = ChooseQuotaAppearEmote();
			if (CurrentState == KrakenStateId.Idle)
			{
				SchedulePendingAppearEmote();
			}
		}

		private void SchedulePendingAppearEmote()
		{
			if (_pendingAppearEmote != TentacleEmoteType.None && _pendingAppearEmoteCoroutine == null)
			{
				_pendingAppearEmoteCoroutine = StartCoroutine(PlayPendingAppearEmoteWithDelay());
			}
		}

		private IEnumerator PlayPendingAppearEmoteWithDelay()
		{
			float quotaAppearEmoteDelay = GetQuotaAppearEmoteDelay();
			if (quotaAppearEmoteDelay > 0f)
			{
				yield return new WaitForSeconds(quotaAppearEmoteDelay);
			}
			_pendingAppearEmoteCoroutine = null;
			TryPlayPendingAppearEmote();
		}

		private void TryPlayPendingAppearEmote()
		{
			TentacleEmoteType pendingAppearEmote = _pendingAppearEmote;
			if (pendingAppearEmote != TentacleEmoteType.None)
			{
				_pendingAppearEmote = TentacleEmoteType.None;
				if (!TryEnterInteractionState(KrakenStateId.Emoting))
				{
					_pendingAppearEmote = pendingAppearEmote;
				}
				else
				{
					_tentacleEmotesController.PlayEmote(pendingAppearEmote);
				}
			}
		}

		private void ClearPendingAppearEmote()
		{
			_pendingAppearEmote = TentacleEmoteType.None;
			StopPendingAppearEmoteCoroutine();
		}

		private void StopPendingAppearEmoteCoroutine()
		{
			if (_pendingAppearEmoteCoroutine != null)
			{
				StopCoroutine(_pendingAppearEmoteCoroutine);
				_pendingAppearEmoteCoroutine = null;
			}
		}

		private static bool IsQuotaAppearReason(KrakenAppearReason reason)
		{
			if (reason != KrakenAppearReason.HalfQuota)
			{
				return reason == KrakenAppearReason.FullQuota;
			}
			return true;
		}

		private static TentacleEmoteType ChooseQuotaAppearEmote()
		{
			if (!(UnityEngine.Random.value < 0.5f))
			{
				return TentacleEmoteType.Nodding;
			}
			return TentacleEmoteType.Like;
		}

		private float GetQuotaAppearEmoteDelay()
		{
			if (_krakenBehaviourConfiguration == null)
			{
				return 0.5f;
			}
			return Mathf.Max(0f, _krakenBehaviourConfiguration.QuotaAppearEmoteDelay);
		}

		private bool CanPlayClap()
		{
			return Time.time - _lastClapTime >= _clapCooldown;
		}

		private bool IsPlayerNearKraken(int playerId)
		{
			if (_spawnedPlayersModel == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerRef key = player.Key;
				PlayerDataHolder value = player.Value;
				if (key.PlayerId == playerId && !(value?.NetworkObject == null))
				{
					return Vector3.Distance(base.transform.position, value.NetworkObject.transform.position) <= _danceResponseRadius;
				}
			}
			return false;
		}

		public async UniTask<bool> RequestAggressiveRockThrowAsync(PlayerRef target)
		{
			if (!base.HasStateAuthority || _krakenAggressiveThrowConfiguration == null)
			{
				return false;
			}
			if (!TryEnterInteractionState(KrakenStateId.AggressiveThrow))
			{
				return false;
			}
			if (base.Runner == null || !base.Runner.IsRunning)
			{
				CompleteInteraction();
				return false;
			}
			NetworkObject krakenRockPrefab = _krakenAggressiveThrowConfiguration.KrakenRockPrefab;
			if (krakenRockPrefab == null)
			{
				CompleteInteraction();
				return false;
			}
			Transform transform = ((_aggressiveRockSpawnPoint != null) ? _aggressiveRockSpawnPoint : base.transform);
			NetworkObject networkObject = await base.Runner.SpawnAsync(krakenRockPrefab, transform.position, transform.rotation);
			if (networkObject == null)
			{
				CompleteInteraction();
				return false;
			}
			if (!networkObject.TryGetComponent<IPointGrabable>(out var component))
			{
				base.Runner.Despawn(networkObject);
				CompleteInteraction();
				return false;
			}
			if (!_krakenThrowAssignmentService.IsGrabbableValid(component) || _krakenThrowAssignmentService.IsItemAlreadyAssigned(_tentacles, component))
			{
				base.Runner.Despawn(networkObject);
				CompleteInteraction();
				return false;
			}
			if (!_krakenThrowAssignmentService.TryFindTentacle(_tentacles, component, component.GameObject.transform.position, KrakenTentacleAssignmentStrategy.RandomFree, out var tentacle))
			{
				base.Runner.Despawn(networkObject);
				CompleteInteraction();
				return false;
			}
			if (!tentacle.TryAssignCatchForAggressiveRock(component, target, _krakenAggressiveThrowConfiguration.Damage, _krakenAggressiveThrowConfiguration.KnockbackForce, _krakenAggressiveThrowConfiguration.HitTrackingTime, _krakenAggressiveThrowConfiguration.HitRadius))
			{
				base.Runner.Despawn(networkObject);
				CompleteInteraction();
				return false;
			}
			tentacle.PlayCatchAnimation(component.IsHeavyItem);
			return true;
		}

		public async UniTask<bool> RequestHelpThrowAsync(KrakenHelpThrowRequest request)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (!TryEnterInteractionState(KrakenStateId.HelpThrow))
			{
				return false;
			}
			PlayerDeadPart playerDeadPart;
			try
			{
				playerDeadPart = await _playerDeadPartSpawnService.SpawnPlayerDeadPart(DeadPartType.DefaultButt, request.SpawnPosition, 1, null);
			}
			catch (NetworkObjectSpawnException)
			{
				CompleteInteraction();
				return false;
			}
			NetworkObject networkObject = playerDeadPart.Object;
			if (networkObject == null)
			{
				CompleteInteraction();
				return false;
			}
			if (!networkObject.TryGetComponent<PlayerDeadPart>(out var component) || component.Grabbables == null || component.Grabbables.Count == 0)
			{
				CompleteInteraction();
				return false;
			}
			IPointGrabable pointGrabable = component.Grabbables[0];
			if (!_krakenThrowAssignmentService.IsGrabbableValid(pointGrabable) || _krakenThrowAssignmentService.IsItemAlreadyAssigned(_tentacles, pointGrabable))
			{
				CompleteInteraction();
				return false;
			}
			if (!_krakenThrowAssignmentService.TryFindTentacle(_tentacles, pointGrabable, pointGrabable.GameObject.transform.position, KrakenTentacleAssignmentStrategy.NearestFree, out var tentacle))
			{
				CompleteInteraction();
				return false;
			}
			if (!tentacle.TryAssignCatchForHelpThrow(pointGrabable, request.AliveTarget, request.TargetPosition))
			{
				CompleteInteraction();
				return false;
			}
			_krakenItemDetector.ForgetItem(pointGrabable);
			tentacle.PlayCatchAnimation(pointGrabable.IsHeavyItem);
			return true;
		}

		private bool ProcessItemThrow(IPointGrabable pointGrabable)
		{
			if (!_krakenThrowAssignmentService.TryFindTentacle(_tentacles, pointGrabable, pointGrabable.GameObject.transform.position, KrakenTentacleAssignmentStrategy.NearestFree, out var tentacle))
			{
				return false;
			}
			bool flag = CurrentState == KrakenStateId.Idle;
			if (!TryEnterItemThrowState())
			{
				return false;
			}
			if (!tentacle.TryAssignCatch(KrakenThrowContext.ThrowBack(pointGrabable)))
			{
				if (flag)
				{
					CompleteInteraction();
				}
				return false;
			}
			_krakenItemDetector.ForgetItem(pointGrabable);
			tentacle.PlayCatchAnimation(pointGrabable.IsHeavyItem);
			TryPlayReactionEmote();
			return true;
		}

		private bool TryEnterItemThrowState()
		{
			if (!base.HasStateAuthority || !CanAcceptItemThrow)
			{
				return false;
			}
			if (CurrentState == KrakenStateId.Idle)
			{
				SetState(KrakenStateId.Catching);
			}
			ResetInactivityTimer();
			return true;
		}

		private bool IsAnyTentacleBusy()
		{
			TentacleController[] tentacles = _tentacles;
			for (int i = 0; i < tentacles.Length; i++)
			{
				if (tentacles[i].IsBusy)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsGrabbableValid(IPointGrabable pointGrabable)
		{
			if (pointGrabable == null || pointGrabable.GameObject == null)
			{
				return false;
			}
			if (pointGrabable.GrabbedBySomethingCount > 0)
			{
				return false;
			}
			if (pointGrabable.GrabBlocked)
			{
				return false;
			}
			return true;
		}

		private bool IsItemAlreadyAssigned(IPointGrabable pointGrabable)
		{
			TentacleController[] tentacles = _tentacles;
			for (int i = 0; i < tentacles.Length; i++)
			{
				IPointGrabable assignedItem = tentacles[i].AssignedItem;
				if (assignedItem != null && IsSameOrConnectedGrabable(pointGrabable, assignedItem))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsSameOrConnectedGrabable(IPointGrabable pointGrabable, IPointGrabable assignedItem)
		{
			if (pointGrabable == assignedItem)
			{
				return true;
			}
			if (pointGrabable is SimplePointGrabable simplePointGrabable)
			{
				foreach (SimplePointGrabable connectedGrabable in simplePointGrabable.ConnectedGrabables)
				{
					if (connectedGrabable == assignedItem)
					{
						return true;
					}
				}
			}
			if (assignedItem is SimplePointGrabable simplePointGrabable2)
			{
				foreach (SimplePointGrabable connectedGrabable2 in simplePointGrabable2.ConnectedGrabables)
				{
					if (connectedGrabable2 == pointGrabable)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void RecordPlayerThrow(IPointGrabable pointGrabable)
		{
			if (_krakenPlayerThrowTrackerModel != null && !(pointGrabable?.NetworkObject == null))
			{
				PlayerRef stateAuthority = pointGrabable.NetworkObject.StateAuthority;
				_krakenPlayerThrowTrackerModel.RecordThrow(stateAuthority);
			}
		}

		private TentacleController FindNearestFreeTentacle(Vector3 fromPosition)
		{
			TentacleController result = null;
			float num = float.MaxValue;
			TentacleController[] tentacles = _tentacles;
			foreach (TentacleController tentacleController in tentacles)
			{
				if (!tentacleController.IsBusy)
				{
					float num2 = Vector3.Distance(tentacleController.transform.position, fromPosition);
					if (!(num2 >= num))
					{
						num = num2;
						result = tentacleController;
					}
				}
			}
			return result;
		}

		private TentacleController FindRandomFreeTentacle()
		{
			int num = 0;
			TentacleController[] tentacles = _tentacles;
			for (int i = 0; i < tentacles.Length; i++)
			{
				if (!tentacles[i].IsBusy)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			int num3 = 0;
			tentacles = _tentacles;
			foreach (TentacleController tentacleController in tentacles)
			{
				if (!tentacleController.IsBusy)
				{
					if (num3 == num2)
					{
						return tentacleController;
					}
					num3++;
				}
			}
			return null;
		}

		private void SetState(KrakenStateId stateId)
		{
			if (NetworkedState != stateId)
			{
				NetworkedState = stateId;
				_stateEnteredTime = Time.time;
			}
		}

		private float GetStateAnimationFallbackSeconds()
		{
			if (_krakenBehaviourConfiguration == null)
			{
				return 1.5f;
			}
			return Mathf.Max(0.01f, _krakenBehaviourConfiguration.StateAnimationFallbackSeconds);
		}

		private void ApplyVisibilityState(KrakenStateId stateId)
		{
			switch (stateId)
			{
			case KrakenStateId.Hidden:
				_tentacleEmotesController.SetHidden();
				break;
			case KrakenStateId.Appearing:
				_tentacleEmotesController.PlayAppear();
				break;
			case KrakenStateId.Disappearing:
				_tentacleEmotesController.PlayDisappear();
				break;
			default:
				_tentacleEmotesController.SetVisible();
				break;
			}
			_appearTentacleController?.SyncWithKrakenState(stateId);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkedState = _NetworkedState;
			IsInitialized = _IsInitialized;
			DanceResponsePlayerId = _DanceResponsePlayerId;
			ActiveDanceResponseEmote = _ActiveDanceResponseEmote;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetworkedState = NetworkedState;
			_IsInitialized = IsInitialized;
			_DanceResponsePlayerId = DanceResponsePlayerId;
			_ActiveDanceResponseEmote = ActiveDanceResponseEmote;
		}
	}
}
