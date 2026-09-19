using System;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	[NetworkBehaviourWeaved(0)]
	public class PirateEnemy : NetworkBehaviour, IHfsmDebugSource<PirateStateId, PirateStateId, PirateEvent>, IHfsmDebugSource, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface, IEnemyAttractionZoneCallbackListener, IEnemyTrackable
	{
		[SerializeField]
		private PirateEnemyContext _context;

		[SerializeField]
		private EnemyType _enemyType = EnemyType.PiratePistol;

		private IInstantiator _instantiator;

		private StateMachine<PirateStateId, PirateStateId, PirateEvent> _fsm;

		private bool _isInitialized;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private IEnemyTrackingService _enemyTrackingService;

		private bool _isFleeTimerRunning;

		private float _fleeTimer;

		private Transform _cachedTransform;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		public StateMachine<PirateStateId, PirateStateId, PirateEvent> StateMachineForDebug => _fsm;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => _enemyType;

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

		public event Action<IEnemyBehaviour> OnDeath;

		public event Action<Vector3> OnChangeAreaTriggered;

		[Inject]
		private void InjectDependencies(IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, EnemyAttractionZoneListenerModel attractionZoneListenerModel, IEnemyTrackingService enemyTrackingService)
		{
			_instantiator = instantiator;
			_enemyFearListenerModel = enemyFearListenerModel;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_enemyTrackingService = enemyTrackingService;
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				base.Object.DespawnHierarchy();
			}
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _isInitialized)
			{
				ResumeAuthoritativeControl();
			}
		}

		private void ResumeAuthoritativeControl()
		{
			EnableAndBindNavMeshAgentToNavMesh();
			EnsureStateMachineRunning();
			RebindPlayerInsideGateListener();
		}

		private void EnableAndBindNavMeshAgentToNavMesh()
		{
			if (!(_context.EnemyMovableBase == null))
			{
				_context.EnemyMovableBase.EnableNavMeshAgent(enable: true);
				_context.EnemyMovableBase.Warp(base.transform.position);
			}
		}

		private void EnsureStateMachineRunning()
		{
			if (_fsm == null)
			{
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		private void RebindPlayerInsideGateListener()
		{
			if (_context.PlayersGatesModel != null)
			{
				_context.PlayersGatesModel.OnPlayerInsideGateChanged -= OnPlayerInsideGateChanged;
				_context.PlayersGatesModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChanged;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			if (!base.HasStateAuthority)
			{
				_context.EnemyMovableBase.EnableNavMeshAgent(enable: false);
			}
			FearCompleted = false;
			_context.IsFearing = false;
			_context.IsDead = false;
			_isFleeTimerRunning = false;
			_fleeTimer = 0f;
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			_enemyTrackingService.RegisterThreat(this);
			if (base.HasStateAuthority)
			{
				_fsm = BuildFsm();
				_fsm.Init();
				_context.EnemyMovableBase.Warp(base.transform.position);
			}
			if (base.HasStateAuthority && _context.PlayersGatesModel != null)
			{
				_context.PlayersGatesModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChanged;
			}
			_isInitialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_context.ReleaseSafeZoneAttackPoint();
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			_enemyTrackingService.UnregisterThreat(this);
			if (hasState && _context.PlayersGatesModel != null)
			{
				_context.PlayersGatesModel.OnPlayerInsideGateChanged -= OnPlayerInsideGateChanged;
			}
			base.Despawned(runner, hasState);
			this.OnDeath?.Invoke(this);
			_isInitialized = false;
		}

		public void TriggerEvent(PirateEvent pirateEvent)
		{
			_fsm?.Trigger(pirateEvent);
		}

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == PirateStateId.Idle && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				_fsm.Trigger(PirateEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		private StateMachine<PirateStateId, PirateStateId, PirateEvent> BuildFsm()
		{
			StateMachine<PirateStateId, PirateStateId, PirateEvent> stateMachine = new StateMachine<PirateStateId, PirateStateId, PirateEvent>();
			stateMachine.AddState(PirateStateId.Idle, _instantiator.Instantiate<PirateIdleState>());
			stateMachine.AddState(PirateStateId.Chasing, _instantiator.Instantiate<PirateChaseState>());
			stateMachine.AddState(PirateStateId.Combat, BuildCombatStateMachine());
			stateMachine.AddState(PirateStateId.Investigating, _instantiator.Instantiate<PirateInvestigateLastSeenState>());
			stateMachine.AddState(PirateStateId.Fleeing, _instantiator.Instantiate<PirateFleeState>());
			stateMachine.AddState(PirateStateId.AttractionInvestigate, _instantiator.Instantiate<PirateAttractionInvestigateState>());
			stateMachine.AddTriggerTransition(PirateEvent.OnAttractionZoneEntered, PirateStateId.Idle, PirateStateId.AttractionInvestigate);
			stateMachine.AddTriggerTransition(PirateEvent.OnAttractionZoneExited, PirateStateId.AttractionInvestigate, PirateStateId.Idle);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetAcquired, PirateStateId.AttractionInvestigate, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnFlee, PirateStateId.AttractionInvestigate, PirateStateId.Fleeing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetAcquired, PirateStateId.Idle, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetAcquired, PirateStateId.Investigating, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetInAttackRange, PirateStateId.Chasing, PirateStateId.Combat);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetInAttackRange, PirateStateId.Investigating, PirateStateId.Combat);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetOutAttackRange, PirateStateId.Combat, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetLost, PirateStateId.Chasing, PirateStateId.Investigating);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetLost, PirateStateId.Combat, PirateStateId.Investigating);
			stateMachine.AddTriggerTransition(PirateEvent.OnIdle, PirateStateId.Combat, PirateStateId.Idle);
			stateMachine.AddTriggerTransition(PirateEvent.OnIdle, PirateStateId.Chasing, PirateStateId.Idle);
			stateMachine.AddTriggerTransition(PirateEvent.OnIdle, PirateStateId.Investigating, PirateStateId.Idle);
			stateMachine.AddTriggerTransition(PirateEvent.OnFlee, PirateStateId.Combat, PirateStateId.Fleeing);
			stateMachine.AddTriggerTransition(PirateEvent.OnFlee, PirateStateId.Chasing, PirateStateId.Fleeing);
			stateMachine.AddTriggerTransition(PirateEvent.OnFlee, PirateStateId.Investigating, PirateStateId.Fleeing);
			stateMachine.AddTriggerTransition(PirateEvent.OnIdle, PirateStateId.Fleeing, PirateStateId.Idle);
			stateMachine.SetStartState(PirateStateId.Idle);
			return stateMachine;
		}

		private HybridStateMachine<PirateStateId, PirateStateId, PirateEvent> BuildCombatStateMachine()
		{
			HybridStateMachine<PirateStateId, PirateStateId, PirateEvent> hybridStateMachine = new HybridStateMachine<PirateStateId, PirateStateId, PirateEvent>();
			hybridStateMachine.AddState(PirateStateId.WaitForAttack, _instantiator.Instantiate<WaitForAttackState>());
			hybridStateMachine.AddState(PirateStateId.MeleeAttack, _instantiator.Instantiate<MeleeAttackState>());
			hybridStateMachine.AddState(PirateStateId.RangedAttack, _instantiator.Instantiate<RangedAttackState>());
			hybridStateMachine.AddState(PirateStateId.SafeZoneAttack, _instantiator.Instantiate<SafeZoneAttackState>());
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnMeleeAttack, PirateStateId.WaitForAttack, PirateStateId.MeleeAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnRangedAttack, PirateStateId.WaitForAttack, PirateStateId.RangedAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnSafeZoneAttack, PirateStateId.WaitForAttack, PirateStateId.SafeZoneAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnMeleeAttack, PirateStateId.RangedAttack, PirateStateId.MeleeAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnWaitToAttack, PirateStateId.RangedAttack, PirateStateId.WaitForAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnWaitToAttack, PirateStateId.MeleeAttack, PirateStateId.WaitForAttack);
			hybridStateMachine.AddTriggerTransition(PirateEvent.OnWaitToAttack, PirateStateId.SafeZoneAttack, PirateStateId.WaitForAttack);
			hybridStateMachine.SetStartState(PirateStateId.WaitForAttack);
			return hybridStateMachine;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null)
			{
				TickFleeTimer();
				_fsm.OnLogic();
			}
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
			_context.SetAreaPosition(_enemyType, areaPosition);
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

		public void StartFleeTimer()
		{
			if (!_isFleeTimerRunning)
			{
				_isFleeTimerRunning = true;
				_fleeTimer = _context.PirateConfiguration.FleeDelayAfterReachingPlayers;
			}
		}

		public bool TryConsumeFinishedFleeTimer()
		{
			if (!_isFleeTimerRunning || _fleeTimer > 0f)
			{
				return false;
			}
			ResetFleeTimer();
			return true;
		}

		public void ResetFleeTimer()
		{
			_isFleeTimerRunning = false;
			_fleeTimer = 0f;
		}

		private void TickFleeTimer()
		{
			if (_isFleeTimerRunning)
			{
				_fleeTimer -= base.Runner.DeltaTime;
			}
		}

		private void OnPlayerInsideGateChanged(int ownerId, bool playerInsideGate)
		{
			if (!(_context.TargetPlayer == PlayerRef.None) && _context.TargetPlayer.PlayerId == ownerId)
			{
				_context.RefreshTargetBeachState();
			}
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
