using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.EnemyFearingModule.Scripts;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class EarEnemy : EnemyBase<EarStateId, EarEvent>, IEnemyFearCallbackListener, IEnemyTrackable
	{
		private EnemyFearListenerModel _enemyFearListenerModel;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private IEnemyTrackingService _enemyTrackingService;

		private Transform _cachedTransform;

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.Ear;

		public Transform Transform => _cachedTransform ?? (_cachedTransform = base.transform);

		public bool IsTrackable
		{
			get
			{
				if (!_fearing)
				{
					return base.CurrentStateId != EarStateId.Fear;
				}
				return false;
			}
		}

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Inject]
		public void InjectDependencies(EarEnemyContext context, IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, SessionAnalyticsModel sessionAnalyticsModel, IEnemyTrackingService enemyTrackingService)
		{
			InjectBaseDependencies(context, instantiator);
			_enemyFearListenerModel = enemyFearListenerModel;
			_sessionAnalyticsModel = sessionAnalyticsModel;
			_enemyTrackingService = enemyTrackingService;
		}

		public void TryRegisterEncounterFromHeardSound(HeardSound heardSound)
		{
			if (heardSound.IsPlayerSource)
			{
				_sessionAnalyticsModel.RegisterEnemyTarget(heardSound.PlayerId);
			}
		}

		protected override StateMachine<EarStateId, EarStateId, EarEvent> BuildFsm()
		{
			StateMachine<EarStateId, EarStateId, EarEvent> stateMachine = new StateMachine<EarStateId, EarStateId, EarEvent>();
			stateMachine.AddState(EarStateId.Wandering, Instantiator.Instantiate<EarWanderingState>());
			stateMachine.AddState(EarStateId.Aggro, Instantiator.Instantiate<EarAggroState>());
			stateMachine.AddState(EarStateId.Attack, Instantiator.Instantiate<EarAttackState>());
			stateMachine.AddState(EarStateId.Stun, Instantiator.Instantiate<EarStunState>());
			stateMachine.AddState(EarStateId.PostAttackWandering, Instantiator.Instantiate<EarPostAttackWanderingState>());
			stateMachine.AddState(EarStateId.MoveToNewArea, Instantiator.Instantiate<EarMoveToNewAreaState>());
			stateMachine.AddState(EarStateId.Fear, Instantiator.Instantiate<EarFearState>());
			stateMachine.AddTriggerTransition(EarEvent.OnSoundHeard, EarStateId.Wandering, EarStateId.Aggro);
			stateMachine.AddTriggerTransition(EarEvent.OnSoundHeard, EarStateId.PostAttackWandering, EarStateId.Aggro);
			stateMachine.AddTriggerTransition(EarEvent.OnSoundTargetLost, EarStateId.Aggro, EarStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(EarEvent.OnReachedSoundPoint, EarStateId.Aggro, EarStateId.Attack);
			stateMachine.AddTriggerTransition(EarEvent.OnAttackFinished, EarStateId.Attack, EarStateId.Stun);
			stateMachine.AddTriggerTransition(EarEvent.OnStunFinished, EarStateId.Stun, EarStateId.PostAttackWandering);
			stateMachine.AddTriggerTransition(EarEvent.OnPostAttackWanderFinished, EarStateId.PostAttackWandering, EarStateId.MoveToNewArea);
			stateMachine.AddTriggerTransition(EarEvent.OnNewAreaReached, EarStateId.MoveToNewArea, EarStateId.Wandering);
			stateMachine.AddTriggerTransitionFromAny(EarEvent.OnFear, EarStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(EarEvent.OnFearEscapeCompleted, EarStateId.Fear, EarStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(EarStateId.Wandering);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyFearListenerModel.RegisterFearListener(EnemyType, EnemyInstants, this);
			_enemyTrackingService.RegisterThreat(this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType, EnemyInstants);
			_enemyTrackingService.UnregisterThreat(this);
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping)
			{
				_escaping = true;
				TriggerEvent(EarEvent.OnFear);
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
			if (base.HasStateAuthority && (!TryGetComponent<EnemyDeathDissolveEffect>(out var component) || !component.TryStartDeferredDespawn(EnemyDissolveReason.Despawn)))
			{
				base.Object.DespawnHierarchy();
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
