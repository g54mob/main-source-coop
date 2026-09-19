using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorEnemy : EnemyBase<AnchorStateId, AnchorEvent>, IEnemyFearCallbackListener, IEnemyAttractionZoneCallbackListener, IEnemyTrackable, IHfsmDebugSource<AnchorStateId, AnchorStateId, AnchorEvent>, IHfsmDebugSource
	{
		private EnemyFearListenerModel _enemyFearListenerModel;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private IEnemyTrackingService _enemyTrackingService;

		private AnchorEnemyContext _context;

		private Transform _cachedTransform;

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.Anchor;

		public StateMachine<AnchorStateId, AnchorStateId, AnchorEvent> StateMachineForDebug => base.Fsm;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public Transform Transform => _cachedTransform ?? (_cachedTransform = base.transform);

		public bool IsTrackable
		{
			get
			{
				if (!_fearing)
				{
					return base.CurrentStateId != AnchorStateId.Fear;
				}
				return false;
			}
		}

		[Inject]
		public void InjectDependencies(AnchorEnemyContext context, IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, EnemyAttractionZoneListenerModel attractionZoneListenerModel, IEnemyTrackingService enemyTrackingService)
		{
			_context = context;
			InjectBaseDependencies(context, instantiator);
			_enemyFearListenerModel = enemyFearListenerModel;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_enemyTrackingService = enemyTrackingService;
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		protected override StateMachine<AnchorStateId, AnchorStateId, AnchorEvent> BuildFsm()
		{
			StateMachine<AnchorStateId, AnchorStateId, AnchorEvent> stateMachine = new StateMachine<AnchorStateId, AnchorStateId, AnchorEvent>();
			stateMachine.AddState(AnchorStateId.Wandering, Instantiator.Instantiate<AnchorWanderingState>());
			stateMachine.AddState(AnchorStateId.Chase, Instantiator.Instantiate<AnchorChaseState>());
			stateMachine.AddState(AnchorStateId.ThrowAnchor, Instantiator.Instantiate<AnchorThrowAnchorState>());
			stateMachine.AddState(AnchorStateId.Reel, Instantiator.Instantiate<AnchorReelState>());
			stateMachine.AddState(AnchorStateId.GrabPlayer, Instantiator.Instantiate<AnchorGrabPlayerState>());
			stateMachine.AddState(AnchorStateId.ReleasePlayer, Instantiator.Instantiate<AnchorReleasePlayerState>());
			stateMachine.AddState(AnchorStateId.DamageAggro, Instantiator.Instantiate<AnchorDamageAggroState>());
			stateMachine.AddState(AnchorStateId.MeleeAttack, Instantiator.Instantiate<AnchorMeleeAttackState>());
			stateMachine.AddState(AnchorStateId.Fear, Instantiator.Instantiate<AnchorFearState>());
			stateMachine.AddState(AnchorStateId.AttractionInvestigate, Instantiator.Instantiate<AnchorAttractionInvestigateState>());
			stateMachine.AddTriggerTransition(AnchorEvent.OnTargetAcquired, AnchorStateId.Wandering, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnTargetLost, AnchorStateId.Chase, AnchorStateId.Wandering);
			stateMachine.AddTriggerTransition(AnchorEvent.OnThrowStarted, AnchorStateId.Chase, AnchorStateId.ThrowAnchor);
			stateMachine.AddTriggerTransition(AnchorEvent.OnAnchorHitPlayer, AnchorStateId.ThrowAnchor, AnchorStateId.Reel);
			stateMachine.AddTriggerTransition(AnchorEvent.OnAnchorMissed, AnchorStateId.ThrowAnchor, AnchorStateId.Reel);
			stateMachine.AddTriggerTransition(AnchorEvent.OnThrowCancelled, AnchorStateId.ThrowAnchor, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnPlayerGrabbed, AnchorStateId.Reel, AnchorStateId.GrabPlayer);
			stateMachine.AddTriggerTransition(AnchorEvent.OnAnchorMissed, AnchorStateId.Reel, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnGrabReleased, AnchorStateId.GrabPlayer, AnchorStateId.ReleasePlayer);
			stateMachine.AddTriggerTransition(AnchorEvent.OnGrabEscaped, AnchorStateId.GrabPlayer, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnReleaseCompleted, AnchorStateId.ReleasePlayer, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnMeleeAttack, AnchorStateId.Chase, AnchorStateId.MeleeAttack);
			stateMachine.AddTriggerTransition(AnchorEvent.OnMeleeCompleted, AnchorStateId.MeleeAttack, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnTargetLost, AnchorStateId.MeleeAttack, AnchorStateId.Wandering);
			stateMachine.AddTriggerTransition(AnchorEvent.OnAttractionZoneEntered, AnchorStateId.Wandering, AnchorStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(AnchorEvent.OnAttractionZoneExited, AnchorStateId.AttractionInvestigate, AnchorStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(AnchorEvent.OnTargetAcquired, AnchorStateId.AttractionInvestigate, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.Wandering, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.Chase, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.ThrowAnchor, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.Reel, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.GrabPlayer, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.ReleasePlayer, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.MeleeAttack, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.AttractionInvestigate, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamaged, AnchorStateId.DamageAggro, AnchorStateId.DamageAggro);
			stateMachine.AddTriggerTransition(AnchorEvent.OnTargetAcquired, AnchorStateId.DamageAggro, AnchorStateId.Chase);
			stateMachine.AddTriggerTransition(AnchorEvent.OnDamageAggroTimeout, AnchorStateId.DamageAggro, AnchorStateId.Wandering);
			stateMachine.AddTriggerTransition(AnchorEvent.OnFear, AnchorStateId.Wandering, AnchorStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(AnchorEvent.OnFearEscapeCompleted, AnchorStateId.Fear, AnchorStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(AnchorStateId.Wandering);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyFearListenerModel.RegisterFearListener(EnemyType, EnemyInstants, this);
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			_enemyTrackingService.RegisterThreat(this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType, EnemyInstants);
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			_enemyTrackingService.UnregisterThreat(this);
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping && base.CurrentStateId == AnchorStateId.Wandering)
			{
				_escaping = true;
				TriggerEvent(AnchorEvent.OnFear);
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
			if (base.HasStateAuthority && base.CurrentStateId == AnchorStateId.Wandering && !_fearing && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				TriggerEvent(AnchorEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
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
