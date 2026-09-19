using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class CrabEnemy : EnemyBase<CrabStateId, CrabEvent>, IEnemyFearCallbackListener, IEnemyAttractionZoneCallbackListener, IHfsmDebugSource<CrabStateId, CrabStateId, CrabEvent>, IHfsmDebugSource
	{
		private EnemyFearListenerModel _enemyFearListenerModel;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private CrabEnemyContext _context;

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.Crab;

		public StateMachine<CrabStateId, CrabStateId, CrabEvent> StateMachineForDebug => base.Fsm;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Inject]
		public void InjectDependencies(CrabEnemyContext context, IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, EnemyAttractionZoneListenerModel attractionZoneListenerModel)
		{
			_context = context;
			InjectBaseDependencies(context, instantiator);
			_enemyFearListenerModel = enemyFearListenerModel;
			_attractionZoneListenerModel = attractionZoneListenerModel;
		}

		protected override StateMachine<CrabStateId, CrabStateId, CrabEvent> BuildFsm()
		{
			StateMachine<CrabStateId, CrabStateId, CrabEvent> stateMachine = new StateMachine<CrabStateId, CrabStateId, CrabEvent>();
			stateMachine.AddState(CrabStateId.SeekItem, Instantiator.Instantiate<CrabSeekItemState>());
			stateMachine.AddState(CrabStateId.GrabWandering, Instantiator.Instantiate<CrabGrabWanderingState>());
			stateMachine.AddState(CrabStateId.Haul, Instantiator.Instantiate<CrabHaulState>());
			stateMachine.AddState(CrabStateId.MoveToNewPos, Instantiator.Instantiate<CrabMoveToNewPosState>());
			stateMachine.AddState(CrabStateId.Rest, Instantiator.Instantiate<CrabRestState>());
			stateMachine.AddState(CrabStateId.Chase, Instantiator.Instantiate<CrabChaseState>());
			stateMachine.AddState(CrabStateId.Fear, Instantiator.Instantiate<CrabFearState>());
			stateMachine.AddState(CrabStateId.AttractionInvestigate, Instantiator.Instantiate<CrabAttractionInvestigateState>());
			stateMachine.AddState(CrabStateId.DamageAggro, Instantiator.Instantiate<CrabDamageAggroState>());
			stateMachine.AddState(CrabStateId.Detach, Instantiator.Instantiate<CrabDetachState>());
			stateMachine.AddTriggerTransition(CrabEvent.OnItemPickedUp, CrabStateId.SeekItem, CrabStateId.GrabWandering);
			stateMachine.AddTriggerTransition(CrabEvent.OnItemWanderCompleted, CrabStateId.GrabWandering, CrabStateId.Haul);
			stateMachine.AddTriggerTransition(CrabEvent.OnItemDropped, CrabStateId.Haul, CrabStateId.MoveToNewPos);
			stateMachine.AddTriggerTransition(CrabEvent.OnNewPosReached, CrabStateId.MoveToNewPos, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnItemLost, CrabStateId.GrabWandering, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnItemLost, CrabStateId.Haul, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetAcquired, CrabStateId.SeekItem, CrabStateId.Chase);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetAcquired, CrabStateId.GrabWandering, CrabStateId.Chase);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetAcquired, CrabStateId.Haul, CrabStateId.Chase);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetAcquired, CrabStateId.MoveToNewPos, CrabStateId.Chase);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetLost, CrabStateId.Chase, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnGrabbed, CrabStateId.Chase, CrabStateId.GrabWandering);
			stateMachine.AddTriggerTransition(CrabEvent.OnHoldTakenByOther, CrabStateId.GrabWandering, CrabStateId.Detach);
			stateMachine.AddTriggerTransition(CrabEvent.OnHoldTakenByOther, CrabStateId.Haul, CrabStateId.Detach);
			stateMachine.AddTriggerTransition(CrabEvent.OnPlayerWanderCompleted, CrabStateId.GrabWandering, CrabStateId.Rest);
			stateMachine.AddTriggerTransition(CrabEvent.OnPlayerReleasedEarly, CrabStateId.GrabWandering, CrabStateId.Rest);
			stateMachine.AddTriggerTransition(CrabEvent.OnGrabbedPlayerDisconnected, CrabStateId.GrabWandering, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnDetachCompleted, CrabStateId.Detach, CrabStateId.Rest);
			stateMachine.AddTriggerTransition(CrabEvent.OnRestCompleted, CrabStateId.Rest, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransition(CrabEvent.OnAttractionZoneEntered, CrabStateId.SeekItem, CrabStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CrabEvent.OnAttractionZoneEntered, CrabStateId.MoveToNewPos, CrabStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CrabEvent.OnAttractionZoneEntered, CrabStateId.GrabWandering, CrabStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CrabEvent.OnAttractionZoneExited, CrabStateId.AttractionInvestigate, CrabStateId.SeekItem, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CrabEvent.OnTargetAcquired, CrabStateId.AttractionInvestigate, CrabStateId.Chase);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.SeekItem, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.GrabWandering, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.Haul, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.MoveToNewPos, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.Rest, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.AttractionInvestigate, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamaged, CrabStateId.DamageAggro, CrabStateId.DamageAggro);
			stateMachine.AddTriggerTransition(CrabEvent.OnGrabbed, CrabStateId.DamageAggro, CrabStateId.GrabWandering);
			stateMachine.AddTriggerTransition(CrabEvent.OnDamageAggroTimeout, CrabStateId.DamageAggro, CrabStateId.SeekItem);
			stateMachine.AddTriggerTransitionFromAny(CrabEvent.OnFear, CrabStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CrabEvent.OnFearEscapeCompleted, CrabStateId.Fear, CrabStateId.SeekItem, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(CrabStateId.SeekItem);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyFearListenerModel.RegisterFearListener(EnemyType.Crab, EnemyInstants, this);
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.Crab, EnemyInstants);
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping)
			{
				_escaping = true;
				TriggerEvent(CrabEvent.OnFear);
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

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && IsInAttractionEligibleState() && !_fearing && (!(_context.AttackCooldown <= 0f) || !_context.CanEnemyTargetPriorityPlayer()) && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				TriggerEvent(CrabEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		private bool IsInAttractionEligibleState()
		{
			switch (base.CurrentStateId)
			{
			case CrabStateId.SeekItem:
			case CrabStateId.MoveToNewPos:
				return true;
			case CrabStateId.GrabWandering:
				if (_context.CrabItemCarrySystem.IsCarryingItem)
				{
					return !_context.PlayerGrabSystem.IsHolding;
				}
				return false;
			default:
				return false;
			}
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
