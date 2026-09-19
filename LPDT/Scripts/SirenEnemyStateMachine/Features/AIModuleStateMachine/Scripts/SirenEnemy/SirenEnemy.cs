using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.States;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy
{
	[NetworkBehaviourWeaved(7)]
	public class SirenEnemy : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IHfsmDebugSource<SirenStateId, SirenStateId, SirenEvent>, IHfsmDebugSource, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface
	{
		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SirenVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentTargetPlayerId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentTargetPlayerId = -1;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsLookAtPresentationActive", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsLookAtPresentationActive;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedPosition", 3, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedPosition;

		[WeaverGenerated]
		[DefaultForProperty("HasNetworkedPosition", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _HasNetworkedPosition;

		private StateMachine<SirenStateId, SirenStateId, SirenEvent> _fsm;

		private HybridStateMachine<SirenStateId, SirenCombatStateId, SirenEvent> _combatFsm;

		private SirenTargetSensor _sensor;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private PlayerMovableModel _playerMovableModel;

		private SirenChaseSettings _chaseSettings;

		private SirenStunSettings _stunSettings;

		private IInstantiator _instantiator;

		private SirenEnemyContext _context;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private IPlayerStateService _playerStateService;

		private ILocalPlayerFollowService _localPlayerFollowService;

		private NetworkPlayerFollowRequest _networkPlayerFollowRequest;

		private IAudioService _audioService;

		private bool _isInitialized;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe SirenVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(SirenVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(SirenVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int CurrentTargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool IsLookAtPresentationActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.IsLookAtPresentationActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.IsLookAtPresentationActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 3)]
		private unsafe Vector3 NetworkedPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.NetworkedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.NetworkedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		private unsafe bool HasNetworkedPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.HasNetworkedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SirenEnemy.HasNetworkedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 6) = new NetworkBool(value);
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.Siren;

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

		public StateMachine<SirenStateId, SirenStateId, SirenEvent> StateMachineForDebug => _fsm;

		public event Action<IEnemyBehaviour> OnDeath;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, EnemyFearListenerModel enemyFearListenerModel, IInstantiator instantiator, SirenChaseSettings chaseSettings, SirenStunSettings stunSettings, SirenEnemyContext context, PlayersStatesSynchronizer playersStatesSynchronizer, IPlayerStateService playerStateService, ILocalPlayerFollowService localPlayerFollowService, NetworkPlayerFollowRequest networkPlayerFollowRequest, IAudioService audioService)
		{
			_playerMovableModel = playerMovableModel;
			_enemyFearListenerModel = enemyFearListenerModel;
			_instantiator = instantiator;
			_chaseSettings = chaseSettings;
			_stunSettings = stunSettings;
			_context = context;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playerStateService = playerStateService;
			_localPlayerFollowService = localPlayerFollowService;
			_networkPlayerFollowRequest = networkPlayerFollowRequest;
			_audioService = audioService;
		}

		private StateMachine<SirenStateId, SirenStateId, SirenEvent> BuildFsm()
		{
			StateMachine<SirenStateId, SirenStateId, SirenEvent> stateMachine = new StateMachine<SirenStateId, SirenStateId, SirenEvent>();
			stateMachine.AddState(SirenStateId.Idle, _instantiator.Instantiate<SirenIdleState>());
			stateMachine.AddState(SirenStateId.Stun, _instantiator.Instantiate<SirenStunState>());
			stateMachine.AddState(SirenStateId.Fear, _instantiator.Instantiate<SirenFearState>());
			_combatFsm = BuildCombatStateMachine();
			stateMachine.AddState(SirenStateId.Combat, _combatFsm);
			stateMachine.AddTransition(new TransitionAfter<SirenStateId>(SirenStateId.Stun, SirenStateId.Idle, _stunSettings.StunTime));
			stateMachine.AddTransition(SirenStateId.Fear, SirenStateId.Idle, (Transition<SirenStateId> transition) => !_context.IsFearing);
			stateMachine.AddTriggerTransition(SirenEvent.OnTargetAcquired, SirenStateId.Idle, SirenStateId.Combat);
			stateMachine.AddTriggerTransition(SirenEvent.OnTargetLost, SirenStateId.Combat, SirenStateId.Stun, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SirenEvent.OnFear, SirenStateId.Idle, SirenStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SirenEvent.OnFear, SirenStateId.Stun, SirenStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(SirenEvent.OnDamage, SirenStateId.Stun, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(SirenStateId.Idle);
			return stateMachine;
		}

		private HybridStateMachine<SirenStateId, SirenCombatStateId, SirenEvent> BuildCombatStateMachine()
		{
			HybridStateMachine<SirenStateId, SirenCombatStateId, SirenEvent> hybridStateMachine = new HybridStateMachine<SirenStateId, SirenCombatStateId, SirenEvent>();
			hybridStateMachine.AddState(SirenCombatStateId.Chasing, _instantiator.Instantiate<SirenChasingState>());
			hybridStateMachine.AddState(SirenCombatStateId.Attacking, _instantiator.Instantiate<SirenAttackingState>());
			hybridStateMachine.AddTransition(SirenCombatStateId.Chasing, SirenCombatStateId.Attacking, (Transition<SirenCombatStateId> transition) => _context.ChaseElapsed >= _chaseSettings.MinChaseTimeBeforeAttack && _context.GetDistanceToTarget() <= _chaseSettings.DistanceToAttack);
			hybridStateMachine.SetStartState(SirenCombatStateId.Chasing);
			return hybridStateMachine;
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				_context.Damageable.OnDamaged += OnDamagedHandler;
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				}
				_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerPlayerStateChangedHandler;
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			_isInitialized = true;
			_context.ValidateRequiredReferences();
			FearCompleted = false;
			_context.IsFearing = false;
			_context.IsDead = false;
			_context.CreateFmodInstances();
			_context.SongSoundInstance.start();
			_enemyFearListenerModel.RegisterFearListener(EnemyType.Siren, base.gameObject.GetHashCode(), this);
			if (base.HasStateAuthority)
			{
				_context.Damageable.OnDamaged += OnDamagedHandler;
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				}
				_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerPlayerStateChangedHandler;
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.Siren, base.gameObject.GetHashCode());
			_context.ReleaseFmodInstances();
			if (base.HasStateAuthority)
			{
				_context.Damageable.OnDamaged -= OnDamagedHandler;
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead -= OnEnemyDeadHandler;
				}
				_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerPlayerStateChangedHandler;
				ReleaseFollowPlayerIfDespawningDuringCombat();
			}
			this.OnDeath?.Invoke(this);
			_isInitialized = false;
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null && !_context.IsDead && _isInitialized)
			{
				NetworkedPosition = base.transform.position;
				HasNetworkedPosition = true;
				if (_context.IsFearing)
				{
					_fsm.Trigger(SirenEvent.OnFear);
				}
				_fsm.OnLogic();
			}
		}

		public override void Render()
		{
			base.Render();
			if (!base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid && HasNetworkedPosition)
			{
				base.transform.position = NetworkedPosition;
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

		public void TriggerEvent(SirenEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void SetVisualState(SirenVisualState state)
		{
			if (base.HasStateAuthority)
			{
				VisualState = state;
			}
		}

		public void SetCurrentTargetPlayerId(int playerId)
		{
			if (base.HasStateAuthority && !_context.IsDead && !(base.Object == null))
			{
				CurrentTargetPlayerId = playerId;
			}
		}

		public void SetLookAtPresentationActive(bool active)
		{
			if (base.HasStateAuthority)
			{
				IsLookAtPresentationActive = active;
			}
		}

		private void ReleaseFollowPlayerIfDespawningDuringCombat()
		{
			if (ShouldReleaseFollowPlayerOnDespawn())
			{
				RaiseExitFollowMode(_context.TargetPlayer.PlayerId);
			}
		}

		private bool ShouldReleaseFollowPlayerOnDespawn()
		{
			if (!_context.HasTarget)
			{
				return false;
			}
			if (VisualState != SirenVisualState.Chasing)
			{
				return VisualState == SirenVisualState.Attacking;
			}
			return true;
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		public void RaiseSongSound(bool play)
		{
			if (_isInitialized)
			{
				SwitchSongSoundRpc(play);
			}
		}

		public void RaiseScreamSound(bool play)
		{
			if (_isInitialized)
			{
				SwitchScreamSoundRpc(play);
			}
		}

		public void RaiseHitSound(bool play)
		{
			if (_isInitialized)
			{
				SwitchHitSoundRpc(play);
			}
		}

		public void RaiseScreamParticle()
		{
			if (_isInitialized)
			{
				PlayScreamParticleRpc();
			}
		}

		public void RaiseSetPlayerRotation(int playerId)
		{
			if (_isInitialized)
			{
				SetPlayerRotationObjectRpc(playerId);
			}
		}

		public void RaiseResetPlayerRotation(int playerId)
		{
			if (_isInitialized)
			{
				ResetPlayerRotationObjectRpc(playerId);
			}
		}

		public void RaiseEnterFollowMode(int playerId)
		{
			_networkPlayerFollowRequest.Request(playerId, PlayerFollowEventType.Enter);
		}

		public void RaiseExitFollowMode(int playerId)
		{
			_networkPlayerFollowRequest.Request(playerId, PlayerFollowEventType.Exit);
		}

		public void RequestDespawnAfterFear()
		{
			if (base.HasStateAuthority)
			{
				_context.SimpleEnemyDeadProcessor.IsNeedToSpawnItem = false;
				base.Object.DespawnHierarchy();
			}
		}

		public void MarkFearCompleted()
		{
			FearCompleted = true;
		}

		private void OnDamagedHandler(DamageData damage)
		{
			if (!_context.IsDead)
			{
				_fsm?.Trigger(SirenEvent.OnDamage);
			}
		}

		private void OnEnemyDeadHandler()
		{
			_context.IsDead = true;
		}

		private void OnSomePlayerPlayerStateChangedHandler(PlayerStateData data)
		{
			if (base.HasStateAuthority && _context.HasTarget && _context.TargetPlayer.PlayerId == data.PlayerId && !_playerStateService.IsPlayerAlive(data.PlayerId))
			{
				_fsm?.Trigger(SirenEvent.OnTargetLost);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2673754837u)]
		private void SwitchScreamSoundRpc([RpcPayload(4)] bool play)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(play);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2673754837u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::SwitchScreamSoundRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(play);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_isInitialized)
			{
				if (play)
				{
					_audioService.StartInstanceWith3DAttributes(_context.ScreamSoundInstance, _context.SoundSourceBehaviour);
				}
				else
				{
					_audioService.StopInstance(_context.ScreamSoundInstance, STOP_MODE.IMMEDIATE);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2914327547u)]
		private void SwitchSongSoundRpc([RpcPayload(4)] bool play)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(play);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2914327547u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::SwitchSongSoundRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(play);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_isInitialized)
			{
				if (play)
				{
					_audioService.StartInstanceWith3DAttributes(_context.SongSoundInstance, _context.SoundSourceBehaviour);
				}
				else
				{
					_audioService.StopInstance(_context.SongSoundInstance, STOP_MODE.IMMEDIATE);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3457297063u)]
		private void SwitchHitSoundRpc([RpcPayload(4)] bool play)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(play);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3457297063u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::SwitchHitSoundRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(play);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_isInitialized)
			{
				if (play)
				{
					_audioService.StartInstanceWith3DAttributes(_context.HitSoundInstance, _context.SoundSourceBehaviour);
				}
				else
				{
					_audioService.StopInstance(_context.HitSoundInstance, STOP_MODE.IMMEDIATE);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2391330771u)]
		private void PlayScreamParticleRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2391330771u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::PlayScreamParticleRpc()", invokeInfo, PlayerRef.None);
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
			if (_isInitialized && _context.ScreamParticle != null)
			{
				_context.ScreamParticle.Play();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4194422505u)]
		private void SetPlayerRotationObjectRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4194422505u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::SetPlayerRotationObjectRpc(System.Int32)", invokeInfo, PlayerRef.None);
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
			if (_isInitialized && !(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_playerMovableModel.Rotator.RotationObject = _context.LookAtTarget;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1154391026u)]
		private void ResetPlayerRotationObjectRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1154391026u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SirenEnemy.SirenEnemy::ResetPlayerRotationObjectRpc(System.Int32)", invokeInfo, PlayerRef.None);
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
			if (_isInitialized && !(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_playerMovableModel.Rotator.RotationObject = null;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			CurrentTargetPlayerId = _CurrentTargetPlayerId;
			IsLookAtPresentationActive = _IsLookAtPresentationActive;
			NetworkedPosition = _NetworkedPosition;
			HasNetworkedPosition = _HasNetworkedPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_CurrentTargetPlayerId = CurrentTargetPlayerId;
			_IsLookAtPresentationActive = IsLookAtPresentationActive;
			_NetworkedPosition = NetworkedPosition;
			_HasNetworkedPosition = HasNetworkedPosition;
		}

		[NetworkRpcWeavedInvoker(2673754837u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SwitchScreamSoundRpc_0040Invoker2673754837([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).SwitchScreamSoundRpc(value);
		}

		[NetworkRpcWeavedInvoker(2914327547u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SwitchSongSoundRpc_0040Invoker2914327547([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).SwitchSongSoundRpc(value);
		}

		[NetworkRpcWeavedInvoker(3457297063u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SwitchHitSoundRpc_0040Invoker3457297063([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).SwitchHitSoundRpc(value);
		}

		[NetworkRpcWeavedInvoker(2391330771u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayScreamParticleRpc_0040Invoker2391330771([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).PlayScreamParticleRpc();
		}

		[NetworkRpcWeavedInvoker(4194422505u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetPlayerRotationObjectRpc_0040Invoker4194422505([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).SetPlayerRotationObjectRpc(value);
		}

		[NetworkRpcWeavedInvoker(1154391026u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ResetPlayerRotationObjectRpc_0040Invoker1154391026([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SirenEnemy)context.TargetBehaviour).ResetPlayerRotationObjectRpc(value);
		}
	}
}
