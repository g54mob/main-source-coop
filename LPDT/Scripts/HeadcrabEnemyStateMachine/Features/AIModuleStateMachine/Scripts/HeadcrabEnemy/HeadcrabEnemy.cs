using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.States;
using Features.AudioServiceModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	[NetworkBehaviourWeaved(14)]
	public class HeadcrabEnemy : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface, IHfsmDebugSource<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent>, IHfsmDebugSource
	{
		private const float ROOT_POSITION_SYNC_EPSILON_SQR = 0.0001f;

		private const float ROOT_ROTATION_SYNC_EPSILON = 0.1f;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[SerializeField]
		private HeadcrabAnimatorPresenter _animatorPresenter;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private HeadcrabVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OriginPosition", 1, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _OriginPosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsOriginCached", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsOriginCached;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("AttachedTargetId", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _AttachedTargetId;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedRootPosition", 6, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedRootPosition;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedRootRotation", 9, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _NetworkedRootRotation;

		[WeaverGenerated]
		[DefaultForProperty("HasNetworkedRootPose", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _HasNetworkedRootPose;

		private bool _isAuthoritySubscribed;

		private StateMachine<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent> _fsm;

		private IInstantiator _instantiator;

		private HeadcrabEnemyContext _context;

		private HeadcrabEnemySettings _enemySettings;

		private PlayersStatesSynchronizer _playerStateModel;

		private MultiplayerModel _multiplayerModel;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private INavigationService _navigationService;

		private IHeadCrabVignetteService _vignetteService;

		private BusyByHeadCrabPlayers _busyPlayers;

		private IAudioService _audioService;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe HeadcrabVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(HeadcrabVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(HeadcrabVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 3)]
		public unsafe Vector3 OriginPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.OriginPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.OriginPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsOriginCached
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.IsOriginCached. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.IsOriginCached. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe NetworkId AttachedTargetId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.AttachedTargetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 5);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.AttachedTargetId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 5) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 3)]
		private unsafe Vector3 NetworkedRootPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.NetworkedRootPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.NetworkedRootPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(9, 4)]
		private unsafe Quaternion NetworkedRootRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.NetworkedRootRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 9);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.NetworkedRootRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 9) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 1)]
		private unsafe bool HasNetworkedRootPose
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.HasNetworkedRootPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 13);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadcrabEnemy.HasNetworkedRootPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 13) = new NetworkBool(value);
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.HeadCrab;

		public int EnemyInstants => base.gameObject.GetInstanceID();

		public bool IsDespawnAfterFear
		{
			get
			{
				if (_context != null)
				{
					return _context.IsDespawnAfterFear;
				}
				return false;
			}
			set
			{
				if (_context != null)
				{
					_context.IsDespawnAfterFear = value;
				}
			}
		}

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public StateMachine<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent> StateMachineForDebug => _fsm;

		public event Action<IEnemyBehaviour> OnDeath;

		[Inject]
		public void InjectDependencies(IInstantiator instantiator, HeadcrabEnemyContext context, PlayersStatesSynchronizer playerStateModel, MultiplayerModel multiplayerModel, EnemyFearListenerModel enemyFearListenerModel, HeadcrabEnemySettings enemySettings, INavigationService navigationService, IHeadCrabVignetteService vignetteService, BusyByHeadCrabPlayers busyPlayers, IAudioService audioService)
		{
			_instantiator = instantiator;
			_context = context;
			_playerStateModel = playerStateModel;
			_multiplayerModel = multiplayerModel;
			_enemyFearListenerModel = enemyFearListenerModel;
			_enemySettings = enemySettings;
			_navigationService = navigationService;
			_vignetteService = vignetteService;
			_busyPlayers = busyPlayers;
			_audioService = audioService;
		}

		private StateMachine<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent> BuildFsm()
		{
			StateMachine<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent> stateMachine = new StateMachine<HeadcrabStateId, HeadcrabStateId, HeadcrabEvent>();
			stateMachine.AddState(HeadcrabStateId.Idle, _instantiator.Instantiate<HeadcrabIdleState>());
			stateMachine.AddState(HeadcrabStateId.Chase, _instantiator.Instantiate<HeadcrabChaseState>());
			stateMachine.AddState(HeadcrabStateId.Attached, _instantiator.Instantiate<HeadcrabAttachedState>());
			stateMachine.AddState(HeadcrabStateId.Fear, _instantiator.Instantiate<HeadcrabFearState>());
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnTargetAcquired, HeadcrabStateId.Idle, HeadcrabStateId.Chase);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnTargetLost, HeadcrabStateId.Chase, HeadcrabStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnTargetLost, HeadcrabStateId.Attached, HeadcrabStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnChaseTimeout, HeadcrabStateId.Chase, HeadcrabStateId.Attached);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnFear, HeadcrabStateId.Idle, HeadcrabStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnFear, HeadcrabStateId.Chase, HeadcrabStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTransition(HeadcrabStateId.Fear, HeadcrabStateId.Idle, (Transition<HeadcrabStateId> transition) => !_context.IsFearing);
			stateMachine.AddTriggerTransition(HeadcrabEvent.OnResnap, HeadcrabStateId.Attached, HeadcrabStateId.Attached, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(HeadcrabStateId.Idle);
			return stateMachine;
		}

		public void StateAuthorityChanged()
		{
			if (base.Object == null || !base.Object.IsValid)
			{
				return;
			}
			if (!base.HasStateAuthority)
			{
				TeardownAuthority();
				return;
			}
			ResetToPristineState();
			if (_context.HomeInstance == null)
			{
				TrySpawnHome();
			}
			InitializeAuthority();
		}

		private void InitializeAuthority()
		{
			if (!_isAuthoritySubscribed)
			{
				_playerStateModel.OnSomePlayerStateChanged += OnPlayerStateChangedHandler;
				_isAuthoritySubscribed = true;
			}
			_fsm = BuildFsm();
			_fsm.Init();
		}

		private void TeardownAuthority()
		{
			if (_isAuthoritySubscribed)
			{
				_playerStateModel.OnSomePlayerStateChanged -= OnPlayerStateChangedHandler;
				_isAuthoritySubscribed = false;
			}
			_fsm = null;
		}

		private void ResetToPristineState()
		{
			ReleaseStaleTargetInteraction();
			_context.IsFearing = false;
			_context.TargetPlayer = PlayerRef.None;
			_context.TargetObject = null;
			_context.ChaseElapsed = 0f;
			_context.SnapElapsed = 0f;
			_context.ResnapCooldownElapsed = 0f;
			ReturnToOrigin();
		}

		public void ReturnToOrigin()
		{
			if (base.HasStateAuthority && !(_context == null))
			{
				_context.IsAttached = false;
				_context.CurrentParentWithPosition = null;
				AttachedTargetId = default(NetworkId);
				base.transform.position = OriginPosition;
				TryAttachToCeilingOnSpawn();
				if (_context.Rigidbody != null)
				{
					_context.Rigidbody.isKinematic = true;
				}
				VisualState = HeadcrabVisualState.Idle;
				PublishRootPose();
				RaiseResetAnimation();
			}
		}

		private void ReleaseStaleTargetInteraction()
		{
			if (!(_context.TargetPlayer == PlayerRef.None))
			{
				int playerId = _context.TargetPlayer.PlayerId;
				RaiseLoopedSittingSound(playerId, play: false);
				if (_context.HomeInstance != null)
				{
					_context.HomeInstance.SetOutlineActiveForPlayer(isActive: false, playerId);
				}
				_vignetteService?.DisableVignetteForPlayer(playerId);
				if (_busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_context.IsFearing = false;
			_context.CreateFmodInstances();
			_enemyFearListenerModel.RegisterFearListener(EnemyType.HeadCrab, base.gameObject.GetHashCode(), this);
			if (_context.Rigidbody != null)
			{
				_context.Rigidbody.isKinematic = true;
			}
			if (base.HasStateAuthority)
			{
				if (!IsOriginCached)
				{
					OriginPosition = base.transform.position;
					IsOriginCached = true;
				}
				TryAttachToCeilingOnSpawn();
				PublishRootPose();
				if (!TrySpawnHome())
				{
					StartCoroutine(DelayedDespawn());
				}
				else
				{
					InitializeAuthority();
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_context.ReleaseFmodInstances();
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.HeadCrab, base.gameObject.GetHashCode());
			if (_isAuthoritySubscribed)
			{
				_playerStateModel.OnSomePlayerStateChanged -= OnPlayerStateChangedHandler;
				_isAuthoritySubscribed = false;
			}
			if (base.HasStateAuthority)
			{
				CleanupAuthorityOnly();
			}
			this.OnDeath?.Invoke(this);
		}

		public override void FixedUpdateNetwork()
		{
			ApplyAttachPose();
			if (base.HasStateAuthority && _fsm != null)
			{
				if (_context.IsFearing)
				{
					_fsm.Trigger(HeadcrabEvent.OnFear);
				}
				_fsm.OnLogic();
				PublishRootPose();
			}
		}

		public override void Render()
		{
			base.Render();
			if (!base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid && (!AttachedTargetId.IsValid || !FollowAttachedTarget()))
			{
				ApplyNetworkedRootPose();
			}
		}

		public void SetAttachedTarget(NetworkObject target)
		{
			if (base.HasStateAuthority)
			{
				AttachedTargetId = ((target != null) ? target.Id : default(NetworkId));
			}
		}

		private bool FollowAttachedTarget()
		{
			if (!AttachedTargetId.IsValid || base.Runner == null)
			{
				return false;
			}
			NetworkObject networkObject = base.Runner.FindObject(AttachedTargetId);
			if (networkObject == null)
			{
				return false;
			}
			Transform targetTransform = networkObject.transform;
			if (networkObject.TryGetComponent<HeadCrabTarget>(out var component) && component.TargetTransform != null)
			{
				targetTransform = component.TargetTransform;
			}
			base.transform.position = targetTransform.position;
			base.transform.rotation = targetTransform.rotation;
			return true;
		}

		private void ApplyAttachPose()
		{
			if (!(_context == null) && _context.CurrentParentWithPosition != null)
			{
				Transform parent = _context.CurrentParentWithPosition.Parent;
				if (parent == null)
				{
					_context.CurrentParentWithPosition = null;
					return;
				}
				base.transform.position = parent.TransformPoint(_context.CurrentParentWithPosition.Position);
				base.transform.rotation = parent.rotation * _context.CurrentParentWithPosition.Rotation;
			}
		}

		private void PublishRootPose()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			if (!HasNetworkedRootPose)
			{
				NetworkedRootPosition = position;
				NetworkedRootRotation = rotation;
				HasNetworkedRootPose = true;
				return;
			}
			if ((NetworkedRootPosition - position).sqrMagnitude > 0.0001f)
			{
				NetworkedRootPosition = position;
			}
			if (Quaternion.Angle(NetworkedRootRotation, rotation) > 0.1f)
			{
				NetworkedRootRotation = rotation;
			}
		}

		private void ApplyNetworkedRootPose()
		{
			if (HasNetworkedRootPose)
			{
				base.transform.position = NetworkedRootPosition;
				base.transform.rotation = NetworkedRootRotation;
			}
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_context.IsFearing = true;
			}
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public void TriggerEvent(HeadcrabEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void RequestStateChange(HeadcrabStateId stateId)
		{
			if (base.HasStateAuthority)
			{
				_fsm?.RequestStateChange(stateId);
			}
		}

		public void SetVisualState(HeadcrabVisualState state)
		{
			if (base.HasStateAuthority)
			{
				VisualState = state;
			}
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		public void RaiseFoundPlayerSound()
		{
			FoundPlayerSoundRpc();
		}

		public void RaiseDeattachSound()
		{
			DeattachSoundRpc();
		}

		public void RaiseAttachToPlayerSound()
		{
			AttachToPlayerSoundRpc();
		}

		public void RaiseKillPlayerSound()
		{
			KillPlayerSoundRpc();
		}

		public void RaiseGoHomeSound()
		{
			GoHomeSoundRpc();
		}

		public void RaiseJumpStart()
		{
			_animatorPresenter?.TriggerJumpStart();
		}

		public void RaiseJumpEnd()
		{
			_animatorPresenter?.TriggerJumpEnd();
		}

		public void RaiseResetAnimation()
		{
			_animatorPresenter?.ResetToIdle();
		}

		public void RaiseLoopedSittingSound(int playerId, bool play)
		{
			if (play)
			{
				StartLoopedSittingSoundRpc(playerId);
			}
			else
			{
				StopLoopedSittingSoundRpc(playerId);
			}
		}

		public void RequestDespawnNoItemDrop()
		{
			if (base.HasStateAuthority)
			{
				if (_context != null && _context.LegacyHealthController != null)
				{
					_context.LegacyHealthController.IsNeedToSpawnItem = false;
				}
				if (base.Runner != null && base.Object != null)
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1736029040u)]
		private void FoundPlayerSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1736029040u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::FoundPlayerSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_context != null)
			{
				_audioService.PlayOneShotAttached(_context.FoundPlayerSound, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2513743069u)]
		private void DeattachSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2513743069u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::DeattachSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_context != null)
			{
				_audioService.PlayOneShotAttached(_context.DeattachSound, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 305390452u)]
		private void AttachToPlayerSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(305390452u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::AttachToPlayerSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_context != null)
			{
				_audioService.PlayOneShotAttached(_context.AttachToPlayerSound, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 284716348u)]
		private void KillPlayerSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(284716348u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::KillPlayerSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_context != null)
			{
				_audioService.PlayOneShotAttached(_context.KillPlayerSound, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2140276520u)]
		private void GoHomeSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2140276520u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::GoHomeSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_context != null)
			{
				_audioService.PlayOneShotAttached(_context.GoHomeSound, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3920489217u)]
		private void StartLoopedSittingSoundRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3920489217u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::StartLoopedSittingSoundRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId && !(_context == null) && _context.SittingOnPlayerLoopedSoundInstance.isValid())
			{
				_audioService.StartInstanceWith3DAttributes(_context.SittingOnPlayerLoopedSoundInstance, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 427830417u)]
		private void StopLoopedSittingSoundRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(427830417u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.HeadcrabEnemy::StopLoopedSittingSoundRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId && !(_context == null) && _context.SittingOnPlayerLoopedSoundInstance.isValid())
			{
				_audioService.StopInstance(_context.SittingOnPlayerLoopedSoundInstance, STOP_MODE.IMMEDIATE);
			}
		}

		private void OnPlayerStateChangedHandler(PlayerStateData data)
		{
			if (!base.HasStateAuthority || _context == null || _context.TargetPlayer == PlayerRef.None || _context.TargetPlayer.PlayerId != data.PlayerId || VisualState != HeadcrabVisualState.Attached)
			{
				return;
			}
			PlayerState playerState = data.PlayerState;
			if (playerState == PlayerState.Dead || playerState == PlayerState.PreDeadCrouch)
			{
				if (_context.LegacyHealthController != null)
				{
					_context.LegacyHealthController.IsNeedToSpawnItem = false;
				}
				base.Object.DespawnHierarchy();
			}
		}

		private IEnumerator DelayedDespawn()
		{
			yield return new WaitForSeconds(1f);
			if (_context != null && _context.LegacyHealthController != null)
			{
				_context.LegacyHealthController.IsNeedToSpawnItem = false;
			}
			if (base.Runner != null && base.Object != null)
			{
				base.Object.DespawnHierarchy();
			}
		}

		private void CleanupAuthorityOnly()
		{
			if (_context == null)
			{
				return;
			}
			if (_context.TargetPlayer != PlayerRef.None)
			{
				int playerId = _context.TargetPlayer.PlayerId;
				if (_context.HomeInstance != null)
				{
					_context.HomeInstance.SetOutlineActiveForPlayer(isActive: false, playerId);
				}
				_vignetteService?.DisableVignetteForPlayer(playerId);
				if (_busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
			}
			if (_context.HomeInstance != null && _context.HomeInstance.Object != null)
			{
				_context.HomeInstance.StartDespawnLoop();
			}
		}

		private void TryAttachToCeilingOnSpawn()
		{
			if (!(_context == null))
			{
				RaycastHit? raycastHit = _context.RaycastUpward(_enemySettings.CeilingRaycastDistance);
				if (raycastHit.HasValue)
				{
					AttachToPositionImmediate(raycastHit.Value.point, Quaternion.identity);
				}
				else
				{
					AttachToPositionImmediate(base.transform.position, Quaternion.identity);
				}
			}
		}

		public void DetachAndGround()
		{
			if (base.HasStateAuthority && !(_context == null))
			{
				_context.IsAttached = false;
				_context.CurrentParentWithPosition = null;
				AttachedTargetId = default(NetworkId);
				if (TryFindGroundPosition(base.transform.position, out var groundPosition))
				{
					base.transform.position = groundPosition;
				}
				if (_context.Rigidbody != null)
				{
					_context.Rigidbody.isKinematic = true;
				}
				PublishRootPose();
			}
		}

		private bool TrySpawnHome()
		{
			if (_context == null || _context.HomePrefab == null || base.Runner == null)
			{
				return false;
			}
			if (!TryFindHomeSpawnPosition(out var position))
			{
				Debug.LogWarning("Headcrab could not resolve a navmesh position for its home; skipping home spawn.");
				return false;
			}
			try
			{
				_context.HomeInstance = base.Runner.Spawn(_context.HomePrefab, position);
			}
			catch (NetworkObjectSpawnException)
			{
				return false;
			}
			return true;
		}

		private bool TryFindHomeSpawnPosition(out Vector3 position)
		{
			if (_navigationService.TryGetRandomSafeNavmeshPositionWithPathFinding(base.transform.position, _enemySettings.HomeSpawnRadius, _enemySettings.HomeSpawnPathLength, _enemySettings.HomeSpawnAttempts, out position))
			{
				return true;
			}
			return TryFindGroundPosition(base.transform.position, out position);
		}

		private bool TryFindGroundPosition(Vector3 origin, out Vector3 groundPosition)
		{
			if (_navigationService.TryGetPointOnNavMeshProjected(origin, out groundPosition))
			{
				return true;
			}
			if (NavMesh.SamplePosition(origin, out var hit, _enemySettings.HomeSpawnRadius, -1))
			{
				groundPosition = hit.position;
				return true;
			}
			groundPosition = origin;
			return false;
		}

		private void AttachToPositionImmediate(Vector3 position, Quaternion rotation)
		{
			_context.CurrentParentWithPosition = null;
			ApplyStaticPlacement(position, rotation);
			_context.IsAttached = true;
		}

		private void ApplyStaticPlacement(Vector3 position, Quaternion rotation)
		{
			if (_context.NoParentGrabPoint != null)
			{
				Vector3 localPosition = _context.NoParentGrabPoint.localPosition;
				localPosition = new Vector3(localPosition.x * base.transform.localScale.x, localPosition.y * base.transform.localScale.y, localPosition.z * base.transform.localScale.z);
				base.transform.position = position - localPosition;
			}
			else
			{
				base.transform.position = position;
			}
			base.transform.rotation = rotation;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			OriginPosition = _OriginPosition;
			IsOriginCached = _IsOriginCached;
			AttachedTargetId = _AttachedTargetId;
			NetworkedRootPosition = _NetworkedRootPosition;
			NetworkedRootRotation = _NetworkedRootRotation;
			HasNetworkedRootPose = _HasNetworkedRootPose;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_OriginPosition = OriginPosition;
			_IsOriginCached = IsOriginCached;
			_AttachedTargetId = AttachedTargetId;
			_NetworkedRootPosition = NetworkedRootPosition;
			_NetworkedRootRotation = NetworkedRootRotation;
			_HasNetworkedRootPose = HasNetworkedRootPose;
		}

		[NetworkRpcWeavedInvoker(1736029040u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FoundPlayerSoundRpc_0040Invoker1736029040([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).FoundPlayerSoundRpc();
		}

		[NetworkRpcWeavedInvoker(2513743069u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DeattachSoundRpc_0040Invoker2513743069([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).DeattachSoundRpc();
		}

		[NetworkRpcWeavedInvoker(305390452u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AttachToPlayerSoundRpc_0040Invoker305390452([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).AttachToPlayerSoundRpc();
		}

		[NetworkRpcWeavedInvoker(284716348u)]
		[Preserve]
		[WeaverGenerated]
		protected static void KillPlayerSoundRpc_0040Invoker284716348([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).KillPlayerSoundRpc();
		}

		[NetworkRpcWeavedInvoker(2140276520u)]
		[Preserve]
		[WeaverGenerated]
		protected static void GoHomeSoundRpc_0040Invoker2140276520([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).GoHomeSoundRpc();
		}

		[NetworkRpcWeavedInvoker(3920489217u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StartLoopedSittingSoundRpc_0040Invoker3920489217([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).StartLoopedSittingSoundRpc(value);
		}

		[NetworkRpcWeavedInvoker(427830417u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StopLoopedSittingSoundRpc_0040Invoker427830417([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadcrabEnemy)context.TargetBehaviour).StopLoopedSittingSoundRpc(value);
		}
	}
}
