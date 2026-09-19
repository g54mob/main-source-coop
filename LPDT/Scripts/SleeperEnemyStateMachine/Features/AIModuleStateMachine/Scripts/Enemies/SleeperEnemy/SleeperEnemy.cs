using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperEnemy : EnemyBase<SleeperStateId, SleeperEvent>, IEnemyFearCallbackListener, IFearHomeAssignable, IEnemyBehaviour, IEnemyTypeProvider, IHfsmDebugSource<SleeperStateId, SleeperStateId, SleeperEvent>, IHfsmDebugSource, IEnemyTrackable
	{
		private IEnemyTrackingService _enemyTrackingService;

		private Transform _cachedTransform;

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.Sleeper;

		public Transform Transform => _cachedTransform ?? (_cachedTransform = base.transform);

		public bool IsTrackable
		{
			get
			{
				if (!_fearing && base.CurrentStateId != SleeperStateId.Sleep)
				{
					return base.CurrentStateId != SleeperStateId.Fear;
				}
				return false;
			}
		}

		public StateMachine<SleeperStateId, SleeperStateId, SleeperEvent> StateMachineForDebug => base.Fsm;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context, IInstantiator instantiator, IEnemyTrackingService enemyTrackingService)
		{
			InjectBaseDependencies(context, instantiator);
			_enemyTrackingService = enemyTrackingService;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyTrackingService.RegisterThreat(this);
		}

		public override void StateAuthorityChanged()
		{
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && base.Fsm != null)
			{
				base.Fsm.RequestStateChange(SleeperStateId.GoHome, forceInstantly: true);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_fearing = false;
			_escaping = false;
			_enemyTrackingService.UnregisterThreat(this);
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping)
			{
				_escaping = true;
				TriggerEvent(SleeperEvent.OnFear);
			}
			base.FixedUpdateNetwork();
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_fearing = true;
			}
		}

		public void MarkFearCompleted()
		{
			FearCompleted = true;
			_fearing = false;
			_escaping = false;
		}

		public void RequestDespawn()
		{
			if (base.HasStateAuthority)
			{
				base.Object.DespawnHierarchy();
			}
		}

		public void RequestGoHome()
		{
			if (base.HasStateAuthority && base.Fsm != null)
			{
				base.Fsm.RequestStateChange(SleeperStateId.GoHome, forceInstantly: true);
			}
		}

		public void RequestMovingImmediately()
		{
			if (base.HasStateAuthority && base.Fsm != null)
			{
				base.Fsm.RequestStateChange(SleeperStateId.MovingImmediately, forceInstantly: true);
			}
		}

		public void ApplyFearHome(Vector3 homePosition, float homeMatchSqrDistance)
		{
			if (base.HasStateAuthority && Context is SleeperEnemyContext sleeperEnemyContext)
			{
				bool num = !sleeperEnemyContext.HasHomePosition || (sleeperEnemyContext.HomePosition - homePosition).sqrMagnitude > homeMatchSqrDistance;
				sleeperEnemyContext.HomePosition = homePosition;
				sleeperEnemyContext.HasHomePosition = true;
				if (num)
				{
					RequestMovingImmediately();
				}
			}
		}

		protected override StateMachine<SleeperStateId, SleeperStateId, SleeperEvent> BuildFsm()
		{
			StateMachine<SleeperStateId, SleeperStateId, SleeperEvent> stateMachine = new StateMachine<SleeperStateId, SleeperStateId, SleeperEvent>();
			stateMachine.AddState(SleeperStateId.Sleep, Instantiator.Instantiate<SleeperSleepState>());
			stateMachine.AddState(SleeperStateId.WakeUp, Instantiator.Instantiate<SleeperWakeUpState>());
			stateMachine.AddState(SleeperStateId.Aggressive, Instantiator.Instantiate<SleeperAggressiveState>());
			stateMachine.AddState(SleeperStateId.Chase, Instantiator.Instantiate<SleeperChaseState>());
			stateMachine.AddState(SleeperStateId.Attack, Instantiator.Instantiate<SleeperAttackState>());
			stateMachine.AddState(SleeperStateId.GoHome, Instantiator.Instantiate<SleeperGoHomeState>());
			stateMachine.AddState(SleeperStateId.Investigate, Instantiator.Instantiate<SleeperInvestigateState>());
			stateMachine.AddState(SleeperStateId.DamageAggro, Instantiator.Instantiate<SleeperDamageAggroState>());
			stateMachine.AddState(SleeperStateId.Fear, Instantiator.Instantiate<SleeperFearState>());
			stateMachine.AddState(SleeperStateId.MovingImmediately, Instantiator.Instantiate<SleeperMovingImmediatelyState>());
			stateMachine.AddTriggerTransition(SleeperEvent.OnSoundHeard, SleeperStateId.Sleep, SleeperStateId.WakeUp);
			stateMachine.AddTriggerTransition(SleeperEvent.OnWakeUpFinished, SleeperStateId.WakeUp, SleeperStateId.Aggressive);
			stateMachine.AddTriggerTransition(SleeperEvent.OnTargetAcquired, SleeperStateId.Aggressive, SleeperStateId.Chase);
			stateMachine.AddTriggerTransition(SleeperEvent.OnAggroTimeout, SleeperStateId.Aggressive, SleeperStateId.Sleep);
			stateMachine.AddTriggerTransition(SleeperEvent.OnSoundHeard, SleeperStateId.Aggressive, SleeperStateId.Investigate);
			stateMachine.AddTriggerTransition(SleeperEvent.OnTargetAcquired, SleeperStateId.Investigate, SleeperStateId.Chase);
			stateMachine.AddTriggerTransition(SleeperEvent.OnReachedSoundPoint, SleeperStateId.Investigate, SleeperStateId.Attack);
			stateMachine.AddTriggerTransition(SleeperEvent.OnInvestigateEmpty, SleeperStateId.Investigate, SleeperStateId.GoHome);
			stateMachine.AddTriggerTransition(SleeperEvent.OnInAttackRange, SleeperStateId.Chase, SleeperStateId.Attack);
			stateMachine.AddTriggerTransition(SleeperEvent.OnChaseTimeout, SleeperStateId.Chase, SleeperStateId.GoHome);
			stateMachine.AddTriggerTransition(SleeperEvent.OnAttackFinished, SleeperStateId.Attack, SleeperStateId.GoHome);
			stateMachine.AddTriggerTransition(SleeperEvent.OnReachedHome, SleeperStateId.GoHome, SleeperStateId.Sleep);
			stateMachine.AddTriggerTransition(SleeperEvent.OnInAttackRange, SleeperStateId.GoHome, SleeperStateId.Attack);
			stateMachine.AddTriggerTransition(SleeperEvent.OnReachedHome, SleeperStateId.MovingImmediately, SleeperStateId.Sleep);
			stateMachine.AddTriggerTransition(SleeperEvent.OnInAttackRange, SleeperStateId.MovingImmediately, SleeperStateId.Attack);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.Sleep, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.WakeUp, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.Aggressive, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.GoHome, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.MovingImmediately, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.Investigate, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamaged, SleeperStateId.DamageAggro, SleeperStateId.DamageAggro);
			stateMachine.AddTriggerTransition(SleeperEvent.OnInAttackRange, SleeperStateId.DamageAggro, SleeperStateId.Attack);
			stateMachine.AddTriggerTransition(SleeperEvent.OnDamageAggroTimeout, SleeperStateId.DamageAggro, SleeperStateId.GoHome);
			stateMachine.AddTriggerTransitionFromAny(SleeperEvent.OnFear, SleeperStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SleeperEvent.OnFearEscapeCompleted, SleeperStateId.Fear, SleeperStateId.Sleep, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(SleeperStateId.Sleep);
			return stateMachine;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
