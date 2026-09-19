using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	[NetworkBehaviourWeaved(4)]
	public class HeadmanEnemy : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IHfsmDebugSource<HeadmanStateId, HeadmanStateId, HeadmanEvent>, IHfsmDebugSource, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface, IEnemyAttractionZoneCallbackListener, IEnemyTrackable
	{
		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private HeadmanVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("RoarSequence", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RoarSequence;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("RageSafeZoneType", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SafeZoneType _RageSafeZoneType;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("RageRandomIndex", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RageRandomIndex;

		private Transform _cachedTransform;

		private StateMachine<HeadmanStateId, HeadmanStateId, HeadmanEvent> _fsm;

		private HeadmanStateId _prioritizeDispatchResumeState;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private PlayersStatesSynchronizer _playerStateModel;

		private IPlayerStateService _playerStateService;

		private IInstantiator _instantiator;

		private HeadmanEnemyContext _context;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private HeadmanChasingSettings _chasingSettings;

		private HeadmanWanderingSettings _wanderingSettings;

		private HeadmanSlowedSettings _slowedSettings;

		private HeadmanRageSettings _rageSettings;

		private HeadmanFearSettings _fearSettings;

		private HeadmanNavmeshPositionSettings _navmeshPositionSettings;

		private IAudioService _audioService;

		private IEnemyTrackingService _enemyTrackingService;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe HeadmanVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((HeadmanVisualState*)Ptr)[0];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				((HeadmanVisualState*)Ptr)[0] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int RoarSequence
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RoarSequence. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RoarSequence. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe SafeZoneType RageSafeZoneType
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RageSafeZoneType. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SafeZoneType)Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RageSafeZoneType. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe int RageRandomIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RageRandomIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadmanEnemy.RageRandomIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.HeadMan;

		public int EnemyInstants => base.gameObject.GetHashCode();

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

		public Transform Transform => _cachedTransform ?? (_cachedTransform = base.transform);

		public bool IsTrackable
		{
			get
			{
				if (_context != null && !_context.IsDead)
				{
					return !_context.IsFearing;
				}
				return false;
			}
		}

		public StateMachine<HeadmanStateId, HeadmanStateId, HeadmanEvent> StateMachineForDebug => _fsm;

		public HeadmanEnemyContext Context => _context;

		public event Action<IEnemyBehaviour> OnDeath;

		public event Action<Vector3> OnChangeAreaTriggered;

		[Inject]
		public void InjectDependencies(EnemyFearListenerModel enemyFearListenerModel, PlayersStatesSynchronizer playerStateModel, IPlayerStateService playerStateService, IInstantiator instantiator, HeadmanEnemyContext context, EnemyAttractionZoneListenerModel attractionZoneListenerModel, HeadmanChasingSettings chasingSettings, HeadmanWanderingSettings wanderingSettings, HeadmanSlowedSettings slowedSettings, HeadmanRageSettings rageSettings, HeadmanFearSettings fearSettings, HeadmanNavmeshPositionSettings navmeshPositionSettings, IAudioService audioService, IEnemyTrackingService enemyTrackingService)
		{
			_enemyTrackingService = enemyTrackingService;
			_enemyFearListenerModel = enemyFearListenerModel;
			_playerStateModel = playerStateModel;
			_playerStateService = playerStateService;
			_instantiator = instantiator;
			_context = context;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_chasingSettings = chasingSettings;
			_wanderingSettings = wanderingSettings;
			_slowedSettings = slowedSettings;
			_rageSettings = rageSettings;
			_fearSettings = fearSettings;
			_navmeshPositionSettings = navmeshPositionSettings;
			_audioService = audioService;
		}

		private StateMachine<HeadmanStateId, HeadmanStateId, HeadmanEvent> BuildFsm()
		{
			StateMachine<HeadmanStateId, HeadmanStateId, HeadmanEvent> stateMachine = new StateMachine<HeadmanStateId, HeadmanStateId, HeadmanEvent>();
			stateMachine.AddState(HeadmanStateId.Wandering, _instantiator.Instantiate<HeadmanWanderingState>());
			stateMachine.AddState(HeadmanStateId.Fear, _instantiator.Instantiate<HeadmanFearState>());
			stateMachine.AddState(HeadmanStateId.Slowed, _instantiator.Instantiate<HeadmanSlowedState>());
			stateMachine.AddState(HeadmanStateId.AttractionInvestigate, _instantiator.Instantiate<HeadmanAttractionInvestigateState>());
			stateMachine.AddState(HeadmanStateId.Chasing, BuildChasingStateMachine());
			stateMachine.AddState(HeadmanStateId.Attacking, BuildAttackingStateMachine());
			stateMachine.AddState(HeadmanStateId.Rage, BuildRageStateMachine());
			stateMachine.AddState(HeadmanStateId.PrioritizeDispatch, delegate
			{
				RunPrioritizeDispatchFromGhost();
			}, null, null, null, needsExitTime: false, isGhostState: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnTargetAcquired, HeadmanStateId.Wandering, HeadmanStateId.Chasing);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnAttackStart, HeadmanStateId.Chasing, HeadmanStateId.Attacking);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnAttackFinished, HeadmanStateId.Attacking, HeadmanStateId.Slowed, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnRageStart, HeadmanStateId.Chasing, HeadmanStateId.Rage, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnRageFinished, HeadmanStateId.Rage, HeadmanStateId.Wandering);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnTargetLost, HeadmanStateId.Chasing, HeadmanStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnTargetLost, HeadmanStateId.Attacking, HeadmanStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnFear, HeadmanStateId.Wandering, HeadmanStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTransition(HeadmanStateId.Fear, HeadmanStateId.Wandering, (Transition<HeadmanStateId> transition) => !_context.IsFearing);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnAttractionZoneEntered, HeadmanStateId.Wandering, HeadmanStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnAttractionZoneExited, HeadmanStateId.AttractionInvestigate, HeadmanStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnTargetAcquired, HeadmanStateId.AttractionInvestigate, HeadmanStateId.Chasing, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(HeadmanEvent.OnFear, HeadmanStateId.AttractionInvestigate, HeadmanStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTransition(HeadmanStateId.Slowed, HeadmanStateId.Chasing, (Transition<HeadmanStateId> transition) => FromSlowedToChasingCondition());
			stateMachine.AddTransition(HeadmanStateId.Slowed, HeadmanStateId.Wandering, (Transition<HeadmanStateId> transition) => FromSlowedToWanderingCondition());
			stateMachine.SetStartState(HeadmanStateId.Wandering);
			return stateMachine;
		}

		private bool FromSlowedToWanderingCondition()
		{
			if (_context.SlowedTimer <= 0f)
			{
				return !_context.CanKeepChasing();
			}
			return false;
		}

		private bool FromSlowedToChasingCondition()
		{
			if (_context.SlowedTimer <= 0f)
			{
				return _context.CanKeepChasing();
			}
			return false;
		}

		private HybridStateMachine<HeadmanStateId, HeadmanRageStateId, HeadmanEvent> BuildRageStateMachine()
		{
			HybridStateMachine<HeadmanStateId, HeadmanRageStateId, HeadmanEvent> hybridStateMachine = new HybridStateMachine<HeadmanStateId, HeadmanRageStateId, HeadmanEvent>();
			hybridStateMachine.AddState(HeadmanRageStateId.Rage, _instantiator.Instantiate<HeadmanRageState>());
			hybridStateMachine.AddState(HeadmanRageStateId.Interacted, _instantiator.Instantiate<HeadmanRageInteractedState>());
			hybridStateMachine.AddState(HeadmanRageStateId.End, _instantiator.Instantiate<HeadmanRageEndState>());
			hybridStateMachine.AddTransition(HeadmanRageStateId.Rage, HeadmanRageStateId.Interacted, (Transition<HeadmanRageStateId> transition) => _context.RageIsApproachingCenter && _context.Agent != null && !_context.Agent.pathPending && _context.Agent.remainingDistance <= _context.Agent.stoppingDistance);
			hybridStateMachine.AddTransition(HeadmanRageStateId.Interacted, HeadmanRageStateId.End, (Transition<HeadmanRageStateId> transition) => _context.RageInteractedTimer >= _rageSettings.RageInteractedDuration);
			hybridStateMachine.AddTransition(HeadmanRageStateId.Rage, HeadmanRageStateId.End, (Transition<HeadmanRageStateId> transition) => _context.RageTimeLeft <= 0f);
			hybridStateMachine.SetStartState(HeadmanRageStateId.Rage);
			return hybridStateMachine;
		}

		private HybridStateMachine<HeadmanStateId, HeadmanAttackingStateId, HeadmanEvent> BuildAttackingStateMachine()
		{
			HybridStateMachine<HeadmanStateId, HeadmanAttackingStateId, HeadmanEvent> hybridStateMachine = new HybridStateMachine<HeadmanStateId, HeadmanAttackingStateId, HeadmanEvent>();
			hybridStateMachine.AddState(HeadmanAttackingStateId.Base, _instantiator.Instantiate<HeadmanBaseAttackingState>());
			hybridStateMachine.AddState(HeadmanAttackingStateId.Low, _instantiator.Instantiate<HeadmanLowAttackingState>());
			hybridStateMachine.AddTransition(HeadmanAttackingStateId.Base, HeadmanAttackingStateId.Low, (Transition<HeadmanAttackingStateId> transition) => _context.ShouldUseLowAttack() && _context.CheckDistanceEnoughToAttack() && !_context.AttackExitSent);
			hybridStateMachine.AddTransition(HeadmanAttackingStateId.Low, HeadmanAttackingStateId.Base, (Transition<HeadmanAttackingStateId> transition) => !_context.ShouldUseLowAttack() && !_context.AttackExitSent);
			hybridStateMachine.SetStartState(HeadmanAttackingStateId.Base);
			return hybridStateMachine;
		}

		private HybridStateMachine<HeadmanStateId, HeadmanChasingStateId, HeadmanEvent> BuildChasingStateMachine()
		{
			HybridStateMachine<HeadmanStateId, HeadmanChasingStateId, HeadmanEvent> hybridStateMachine = new HybridStateMachine<HeadmanStateId, HeadmanChasingStateId, HeadmanEvent>();
			hybridStateMachine.AddState(HeadmanChasingStateId.Start, _instantiator.Instantiate<HeadmanChasingStartState>());
			hybridStateMachine.AddState(HeadmanChasingStateId.Chase, _instantiator.Instantiate<HeadmanChasingState>());
			hybridStateMachine.AddTransition(HeadmanChasingStateId.Start, HeadmanChasingStateId.Chase);
			hybridStateMachine.SetStartState(HeadmanChasingStateId.Start);
			return hybridStateMachine;
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				}
				_playerStateModel.OnSomePlayerStateChanged += OnPlayerStateChangedHandler;
				_context.HeadManTargetsModel.OnPrioritizeChanged += OnPrioritizeChangedHandler;
				_context.RageReactor.OnRoarSound += OnRageRoarSoundHandler;
				_context.RageReactor.OnRoarEnded += OnRageRoarEndedHandler;
				_context.RageReactor.OnRageInteractedStart += OnRageInteractedStartHandler;
				_context.Init(_chasingSettings, _wanderingSettings, _fearSettings, _navmeshPositionSettings);
				_context.DamageDealerPlayerId = ((base.Object != null) ? base.Object.StateAuthority.PlayerId : 0);
				_fsm = BuildFsm();
				_fsm.Init();
			}
			SetupObjectByAuthority();
		}

		public override void Spawned()
		{
			base.Spawned();
			SetupObjectByAuthority();
			FearCompleted = false;
			_context.IsFearing = false;
			_context.IsDead = false;
			_context.CreateFmodInstances();
			_audioService.StartInstanceWith3DAttributes(_context.IdleSoundInstance, _context.SoundSourceBehaviour);
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			_enemyTrackingService.RegisterThreat(this);
			_context.RageReactor.OnBite += OnBiteSound;
			if (base.HasStateAuthority)
			{
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				}
				_playerStateModel.OnSomePlayerStateChanged += OnPlayerStateChangedHandler;
				_context.HeadManTargetsModel.OnPrioritizeChanged += OnPrioritizeChangedHandler;
				_context.RageReactor.OnRoarSound += OnRageRoarSoundHandler;
				_context.RageReactor.OnRoarEnded += OnRageRoarEndedHandler;
				_context.RageReactor.OnRageInteractedStart += OnRageInteractedStartHandler;
				_context.Init(_chasingSettings, _wanderingSettings, _fearSettings, _navmeshPositionSettings);
				_context.DamageDealerPlayerId = ((base.Object != null) ? base.Object.StateAuthority.PlayerId : 0);
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			_enemyTrackingService.UnregisterThreat(this);
			_context.ReleaseFmodInstances();
			_context.RageReactor.OnBite -= OnBiteSound;
			if (base.HasStateAuthority)
			{
				if (_context.StatHealthController != null)
				{
					_context.StatHealthController.OnEnemyDead -= OnEnemyDeadHandler;
				}
				_playerStateModel.OnSomePlayerStateChanged -= OnPlayerStateChangedHandler;
				_context.HeadManTargetsModel.OnPrioritizeChanged -= OnPrioritizeChangedHandler;
				_context.RageReactor.OnRoarSound -= OnRageRoarSoundHandler;
				_context.RageReactor.OnRoarEnded -= OnRageRoarEndedHandler;
				_context.RageReactor.OnRageInteractedStart -= OnRageInteractedStartHandler;
			}
			this.OnDeath?.Invoke(this);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null && !_context.IsDead)
			{
				if (_context.IsFearing && _fsm.ActiveStateName == HeadmanStateId.Wandering)
				{
					_fsm.Trigger(HeadmanEvent.OnFear);
				}
				_fsm.OnLogic();
			}
		}

		public void SetVisualState(HeadmanVisualState state)
		{
			if (base.HasStateAuthority && VisualState != state)
			{
				VisualState = state;
			}
		}

		private void SetupObjectByAuthority()
		{
			_context.Agent.enabled = base.HasStateAuthority;
		}

		private void OnEnemyDeadHandler()
		{
			_context.IsDead = true;
			SetVisualState(HeadmanVisualState.Dead);
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_context.IsFearing = true;
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

		public void SetAreaPosition(Vector3 areaPosition)
		{
			_context.SetAreaPosition(areaPosition);
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public void RaiseChangeAreaTriggered(Vector3 position)
		{
			this.OnChangeAreaTriggered?.Invoke(position);
		}

		public void TriggerEvent(HeadmanEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == HeadmanStateId.Wandering && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				_fsm.Trigger(HeadmanEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		private void OnPrioritizeChangedHandler()
		{
			if (base.HasStateAuthority && _fsm != null)
			{
				HeadmanStateId activeStateName = _fsm.ActiveStateName;
				if (activeStateName != HeadmanStateId.PrioritizeDispatch)
				{
					_prioritizeDispatchResumeState = activeStateName;
					_fsm.RequestStateChange(HeadmanStateId.PrioritizeDispatch, forceInstantly: true);
				}
			}
		}

		private void RunPrioritizeDispatchFromGhost()
		{
			HeadmanStateId prioritizeDispatchResumeState = _prioritizeDispatchResumeState;
			if (!_context.TrySyncTargetFromPriority())
			{
				if (_context.HasPlayerChaseTarget)
				{
					bool flag = _context.ShouldEnterRageWhenNoTargets();
					_context.ClearTarget();
					if (prioritizeDispatchResumeState == HeadmanStateId.Chasing || prioritizeDispatchResumeState == HeadmanStateId.Attacking || prioritizeDispatchResumeState == HeadmanStateId.Slowed)
					{
						if (prioritizeDispatchResumeState == HeadmanStateId.Chasing && flag)
						{
							_fsm.RequestStateChange(HeadmanStateId.Rage, forceInstantly: true);
						}
						else
						{
							_fsm.RequestStateChange(HeadmanStateId.Wandering, forceInstantly: true);
						}
						return;
					}
				}
				_fsm.RequestStateChange(prioritizeDispatchResumeState, forceInstantly: true);
			}
			else if (prioritizeDispatchResumeState == HeadmanStateId.Wandering || prioritizeDispatchResumeState == HeadmanStateId.AttractionInvestigate)
			{
				_fsm.RequestStateChange(HeadmanStateId.Chasing, forceInstantly: true);
			}
			else
			{
				_fsm.RequestStateChange(prioritizeDispatchResumeState, forceInstantly: true);
			}
		}

		private void OnPlayerStateChangedHandler(PlayerStateData data)
		{
			if (base.HasStateAuthority && _context.HasPlayerChaseTarget && _context.TargetPlayerToChase.PlayerId == data.PlayerId && !_playerStateService.IsPlayerAlive(data.PlayerId))
			{
				bool flag = _context.ShouldEnterRageWhenNoTargets();
				HeadmanEvent trigger = ((!(_fsm.ActiveStateName == HeadmanStateId.Chasing && flag)) ? HeadmanEvent.OnTargetLost : HeadmanEvent.OnRageStart);
				_context.ClearTarget();
				_fsm.Trigger(trigger);
			}
		}

		private void OnRageRoarSoundHandler()
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == HeadmanStateId.Rage && _context.ActiveRageSubstate == HeadmanRageStateId.End)
			{
				RaiseRoarSound();
				RaiseRageStartScreenShake();
			}
		}

		private void OnRageRoarEndedHandler()
		{
			if (base.HasStateAuthority)
			{
				_context.RoarsPerformed++;
				if (_context.RoarsPerformed < _rageSettings.RoarCount)
				{
					RoarSequence++;
				}
			}
		}

		private void OnRageInteractedStartHandler()
		{
			if (base.HasStateAuthority)
			{
				RaiseRageInteractScreenShake();
			}
		}

		private void OnBiteSound()
		{
			if (base.HasStateAuthority)
			{
				RaiseBiteSound();
			}
		}

		public void RaiseNoticePlayerSound()
		{
			NoticePlayerSoundRpc();
		}

		public void RaiseAttackSound()
		{
			PlayAttackSoundRpc();
		}

		public void RaiseRoarSound()
		{
			PlayRoarSoundRpc();
		}

		public void RaiseBiteSound()
		{
			PlayBiteSoundRpc();
		}

		public void TriggerRoarPresentation()
		{
			if (base.HasStateAuthority)
			{
				RoarSequence++;
			}
		}

		public void RaiseRageStartScreenShake()
		{
			RageStartScreenShakeRpc();
		}

		public void RaiseRageInteractScreenShake()
		{
			RageInteractScreenShakeRpc();
		}

		public void SetRageInteractedPresentation(SafeZoneType safeZoneType, int randomIndex)
		{
			if (base.HasStateAuthority && (RageSafeZoneType != safeZoneType || RageRandomIndex != randomIndex))
			{
				RageSafeZoneType = safeZoneType;
				RageRandomIndex = randomIndex;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 46241796u)]
		private void NoticePlayerSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(46241796u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::NoticePlayerSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayNoticeSound();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1121292943u)]
		private void PlayAttackSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1121292943u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::PlayAttackSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayAttackSound();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3037090841u)]
		private void PlayRoarSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3037090841u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::PlayRoarSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayRoarSound();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 398152705u)]
		private void PlayBiteSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(398152705u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::PlayBiteSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayBiteSound();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2750910745u)]
		private void RageStartScreenShakeRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2750910745u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::RageStartScreenShakeRpc()", invokeInfo, PlayerRef.None);
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
			_context.TriggerRageStartScreenShake();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4137231871u)]
		private void RageInteractScreenShakeRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4137231871u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadmanEnemy.HeadmanEnemy::RageInteractScreenShakeRpc()", invokeInfo, PlayerRef.None);
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
			_context.TriggerRageInteractScreenShake();
		}

		public void RequestDespawnAfterFear()
		{
			if (base.HasStateAuthority)
			{
				base.Object.DespawnHierarchy();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			RoarSequence = _RoarSequence;
			RageSafeZoneType = _RageSafeZoneType;
			RageRandomIndex = _RageRandomIndex;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_RoarSequence = RoarSequence;
			_RageSafeZoneType = RageSafeZoneType;
			_RageRandomIndex = RageRandomIndex;
		}

		[NetworkRpcWeavedInvoker(46241796u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NoticePlayerSoundRpc_0040Invoker46241796([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).NoticePlayerSoundRpc();
		}

		[NetworkRpcWeavedInvoker(1121292943u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAttackSoundRpc_0040Invoker1121292943([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).PlayAttackSoundRpc();
		}

		[NetworkRpcWeavedInvoker(3037090841u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayRoarSoundRpc_0040Invoker3037090841([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).PlayRoarSoundRpc();
		}

		[NetworkRpcWeavedInvoker(398152705u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayBiteSoundRpc_0040Invoker398152705([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).PlayBiteSoundRpc();
		}

		[NetworkRpcWeavedInvoker(2750910745u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RageStartScreenShakeRpc_0040Invoker2750910745([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).RageStartScreenShakeRpc();
		}

		[NetworkRpcWeavedInvoker(4137231871u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RageInteractScreenShakeRpc_0040Invoker4137231871([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadmanEnemy)context.TargetBehaviour).RageInteractScreenShakeRpc();
		}
	}
}
