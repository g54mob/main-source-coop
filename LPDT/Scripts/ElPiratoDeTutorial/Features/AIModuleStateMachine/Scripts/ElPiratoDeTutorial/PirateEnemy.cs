using System;
using Features.AIModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	[NetworkBehaviourWeaved(0)]
	public class PirateEnemy : NetworkBehaviour, IHfsmDebugSource<PirateStateId, PirateStateId, PirateEvent>, IHfsmDebugSource, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private PirateEnemyContext _context;

		[SerializeField]
		private EnemyType _enemyType = EnemyType.PiratePistol;

		private IInstantiator _instantiator;

		private StateMachine<PirateStateId, PirateStateId, PirateEvent> _fsm;

		private bool _isInitialized;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		public StateMachine<PirateStateId, PirateStateId, PirateEvent> StateMachineForDebug => _fsm;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		public PirateEnemyContext CastedContext => _context;

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

		public event Action<IEnemyBehaviour> OnDeath;

		[Inject]
		private void InjectDependencies(IInstantiator instantiator)
		{
			_instantiator = instantiator;
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
				EnableAndBindNavMeshAgentToNavMesh();
				EnsureStateMachineRunning();
			}
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

		private StateMachine<PirateStateId, PirateStateId, PirateEvent> BuildFsm()
		{
			StateMachine<PirateStateId, PirateStateId, PirateEvent> stateMachine = new StateMachine<PirateStateId, PirateStateId, PirateEvent>();
			stateMachine.AddState(PirateStateId.Idle, _instantiator.Instantiate<PirateIdleState>());
			stateMachine.AddState(PirateStateId.Chasing, _instantiator.Instantiate<PirateChaseState>());
			stateMachine.AddState(PirateStateId.RangedAttack, _instantiator.Instantiate<RangedAttackState>());
			stateMachine.AddState(PirateStateId.Finished, _instantiator.Instantiate<PirateFinishedState>());
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetAcquired, PirateStateId.Idle, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetInAttackRange, PirateStateId.Chasing, PirateStateId.RangedAttack);
			stateMachine.AddTriggerTransition(PirateEvent.OnAttackFinished, PirateStateId.RangedAttack, PirateStateId.Chasing);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetDead, PirateStateId.Chasing, PirateStateId.Finished);
			stateMachine.AddTriggerTransition(PirateEvent.OnTargetDead, PirateStateId.RangedAttack, PirateStateId.Finished);
			stateMachine.SetStartState(PirateStateId.Idle);
			return stateMachine;
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
			if (base.HasStateAuthority)
			{
				_fsm = BuildFsm();
				_fsm.Init();
				_context.EnemyMovableBase.Warp(base.transform.position);
			}
			_isInitialized = true;
			_context.Damageable.IsDamageBlocked = true;
			SwitchOutline(outlineEnabled: false);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			base.Despawned(runner, hasState);
			_context.IsDead = true;
			this.OnDeath?.Invoke(this);
			_isInitialized = false;
		}

		public void TriggerEvent(PirateEvent pirateEvent)
		{
			_fsm?.Trigger(pirateEvent);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null)
			{
				_fsm.OnLogic();
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

		public void SwitchOutline(bool outlineEnabled)
		{
			_context.PirateOutline.enabled = outlineEnabled;
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
