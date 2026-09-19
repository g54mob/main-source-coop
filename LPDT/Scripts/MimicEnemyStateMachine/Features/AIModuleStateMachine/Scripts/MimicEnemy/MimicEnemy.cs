using System;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	[NetworkBehaviourWeaved(1)]
	public class MimicEnemy : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IHfsmDebugSource<MimicStateId, MimicStateId, MimicEvent>, IHfsmDebugSource, IStateAuthorityChanged, IPublicFacingInterface, ICurrentStateProvider<MimicStateId>, IEnemyAttractionZoneCallbackListener
	{
		[SerializeField]
		private MimicStateId _startState = MimicStateId.MoveToRandomPos;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private MimicVisualState _VisualState = MimicVisualState.MoveToRandomPos;

		private StateMachine<MimicStateId, MimicStateId, MimicEvent> _fsm;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private MimicEnemyContext _mimicContext;

		private IInstantiator _instantiator;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private EnemyHeadwearModel _enemyHeadwearModel;

		private MimicCauldronSettings _cauldronSettings;

		private bool _isCauldronTimerRunning;

		private float _cauldronSecondsLeft;

		private bool _fearing;

		private bool _escaping;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe MimicVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(MimicVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(MimicVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		public MimicStateId CurrentStateId { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.Mimic;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public StateMachine<MimicStateId, MimicStateId, MimicEvent> StateMachineForDebug => _fsm;

		private bool IsCauldronWorn
		{
			get
			{
				if (base.Object != null && base.Object.IsValid)
				{
					return _enemyHeadwearModel.IsWearing(base.Object.Id.Raw);
				}
				return false;
			}
		}

		public event Action<IEnemyBehaviour> OnDeath;

		[Inject]
		public void InjectDependencies(EnemyFearListenerModel enemyFearListenerModel, IInstantiator instantiator, MimicEnemyContext mimicContext, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, EnemyAttractionZoneListenerModel attractionZoneListenerModel, EnemyHeadwearModel enemyHeadwearModel, MimicCauldronSettings mimicCauldronSettings)
		{
			_enemyFearListenerModel = enemyFearListenerModel;
			_instantiator = instantiator;
			_mimicContext = mimicContext;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_enemyHeadwearModel = enemyHeadwearModel;
			_cauldronSettings = mimicCauldronSettings;
		}

		public bool CanEnemyInteractWithPriorityPlayer()
		{
			if (_mimicContext.PriorityPlayer?.NetworkObject == null)
			{
				return false;
			}
			if (IsCauldronWorn)
			{
				return false;
			}
			return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(_mimicContext.PriorityPlayer.NetworkObject.InputAuthority.PlayerId);
		}

		private void UpdateCauldron()
		{
			if (!IsCauldronWorn)
			{
				_isCauldronTimerRunning = false;
				return;
			}
			if (!_isCauldronTimerRunning)
			{
				_isCauldronTimerRunning = true;
				_cauldronSecondsLeft = UnityEngine.Random.Range(_cauldronSettings.MinDuration, _cauldronSettings.MaxDuration);
			}
			_mimicContext.IsReadyForAttack = false;
			if (_mimicContext.IsAttackInProcess)
			{
				_mimicContext.MimicTargetAttackSystem.Clear();
			}
			if (_cauldronSecondsLeft > 0f)
			{
				_cauldronSecondsLeft -= GetTickDelta();
			}
			if (!(_cauldronSecondsLeft > 0f) && !IsUnderCover())
			{
				DropCauldron();
			}
		}

		private bool IsUnderCover()
		{
			return Physics.Raycast(base.transform.position + Vector3.up * _cauldronSettings.ShedCheckOriginHeight, Vector3.up, _cauldronSettings.ShedCheckDistance, _cauldronSettings.ShedBlockingMask, QueryTriggerInteraction.Ignore);
		}

		private void DropCauldron()
		{
			uint raw = base.Object.Id.Raw;
			if (_enemyHeadwearModel.TryGet(raw, out var headwear) && headwear != null)
			{
				_enemyHeadwearModel.Set(raw, isWearing: false);
				_isCauldronTimerRunning = false;
				Vector3 impulse = Vector3.up * _cauldronSettings.UpImpulse + base.transform.forward * _cauldronSettings.ForwardImpulse;
				headwear.DropFromEnemy(impulse);
			}
		}

		private StateMachine<MimicStateId, MimicStateId, MimicEvent> BuildFsm()
		{
			StateMachine<MimicStateId, MimicStateId, MimicEvent> stateMachine = new StateMachine<MimicStateId, MimicStateId, MimicEvent>();
			stateMachine.AddState(MimicStateId.MoveToRandomPos, _instantiator.Instantiate<MoveToRandomPointState>());
			stateMachine.AddState(MimicStateId.MoveToPlayerArea, _instantiator.Instantiate<MoveToPlayerAreaState>());
			stateMachine.AddState(MimicStateId.PlayerBehaviorSimulation, _instantiator.Instantiate<PlayerBehaviorSimulationState>());
			stateMachine.AddState(MimicStateId.TargetChasing, _instantiator.Instantiate<TargetChasingState>());
			stateMachine.AddState(MimicStateId.MoveToNewArea, _instantiator.Instantiate<MoveToNewAreaState>());
			stateMachine.AddState(MimicStateId.FearEscape, _instantiator.Instantiate<FearEscapeState>());
			stateMachine.AddState(MimicStateId.Despawn, _instantiator.Instantiate<DespawnState>());
			stateMachine.AddState(MimicStateId.AttractionInvestigate, _instantiator.Instantiate<MimicAttractionInvestigateState>());
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetAcquired, MimicStateId.MoveToRandomPos, MimicStateId.MoveToPlayerArea);
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetLost, MimicStateId.MoveToPlayerArea, MimicStateId.MoveToRandomPos);
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetLost, MimicStateId.TargetChasing, MimicStateId.MoveToNewArea);
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetPositionCompleted, MimicStateId.MoveToPlayerArea, MimicStateId.PlayerBehaviorSimulation);
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetPositionCompleted, MimicStateId.MoveToNewArea, MimicStateId.MoveToRandomPos);
			stateMachine.AddTriggerTransition(MimicEvent.OnSimulationComplete, MimicStateId.PlayerBehaviorSimulation, MimicStateId.MoveToNewArea);
			stateMachine.AddTriggerTransition(MimicEvent.OnAttackCooldown, MimicStateId.TargetChasing, MimicStateId.MoveToNewArea);
			stateMachine.AddTriggerTransition(MimicEvent.OnFearEscapeCompleted, MimicStateId.FearEscape, MimicStateId.MoveToRandomPos, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnDespawnRequested, MimicStateId.FearEscape, MimicStateId.Despawn, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnReadyForAttack, MimicStateId.MoveToRandomPos, MimicStateId.TargetChasing, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnReadyForAttack, MimicStateId.MoveToPlayerArea, MimicStateId.TargetChasing, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnReadyForAttack, MimicStateId.PlayerBehaviorSimulation, MimicStateId.TargetChasing, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(MimicEvent.OnFear, MimicStateId.FearEscape, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnAttractionZoneEntered, MimicStateId.MoveToRandomPos, MimicStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnAttractionZoneExited, MimicStateId.AttractionInvestigate, MimicStateId.MoveToRandomPos, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MimicEvent.OnTargetAcquired, MimicStateId.AttractionInvestigate, MimicStateId.MoveToPlayerArea);
			stateMachine.AddTriggerTransition(MimicEvent.OnReadyForAttack, MimicStateId.AttractionInvestigate, MimicStateId.TargetChasing, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(_startState);
			return stateMachine;
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				_fsm = BuildFsm();
				_fsm.Init();
			}
			if (base.HasStateAuthority && _mimicContext.NavMeshAgent != null)
			{
				_mimicContext.NavMeshAgent.enabled = true;
				_mimicContext.NavMeshAgent.Warp(base.transform.position);
			}
			if (base.HasStateAuthority)
			{
				ReconcileInterruptedAttack();
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_mimicContext.NavMeshAgent.enabled = base.HasStateAuthority;
			if (base.HasStateAuthority)
			{
				_mimicContext.NavMeshAgent.Warp(base.transform.position);
			}
			_mimicContext.Initialize();
			_enemyFearListenerModel.RegisterFearListener(EnemyType.Mimic, base.gameObject.GetHashCode(), this);
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			if (base.HasStateAuthority)
			{
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
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.Mimic, base.gameObject.GetHashCode());
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			if (base.HasStateAuthority)
			{
				DropCauldron();
				this.OnDeath?.Invoke(this);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority || _fsm == null)
			{
				return;
			}
			ReconcileInterruptedAttack();
			_mimicContext.TickPriorityPin(base.Runner.DeltaTime);
			UpdateCauldron();
			if (ShouldEnterFear())
			{
				_mimicContext.IsDespawnAfterFear = IsDespawnAfterFear;
				_escaping = true;
				_fsm.Trigger(MimicEvent.OnFear);
				if (!IsDespawnAfterFear)
				{
					_escaping = false;
					_fearing = false;
					FearCompleted = true;
				}
			}
			_fsm.OnLogic();
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

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_fearing = true;
			}
		}

		public void TriggerEvent(MimicEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == MimicStateId.MoveToRandomPos && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_mimicContext.SetPendingAttractionZone(zone);
				_fsm.Trigger(MimicEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		public void SetVisualState(MimicVisualState state)
		{
			if (base.HasStateAuthority && VisualState != state)
			{
				VisualState = state;
			}
		}

		public void SetCurrentStateId(MimicStateId stateId)
		{
			if (base.HasStateAuthority && CurrentStateId != stateId)
			{
				CurrentStateId = stateId;
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

		public void MarkFearEscapeCompleted()
		{
			_escaping = false;
			_fearing = false;
			FearCompleted = true;
		}

		public void RequestDespawnAfterFear()
		{
			if (base.HasStateAuthority)
			{
				if (_mimicContext.MimicFearDestroySystem != null)
				{
					_mimicContext.MimicFearDestroySystem.PrepareDespawn();
				}
				base.Object.DespawnHierarchy();
			}
		}

		private void ReconcileInterruptedAttack()
		{
			if (CurrentStateId != MimicStateId.TargetChasing && (_mimicContext.IsAggressive || _mimicContext.IsAttackInProcess))
			{
				_mimicContext.MimicTargetAttackSystem.Clear();
			}
		}

		private bool ShouldEnterFear()
		{
			if (!_fearing || _escaping)
			{
				return false;
			}
			if (_mimicContext.IsAttackInProcess || _mimicContext.IsReadyForAttack)
			{
				return false;
			}
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
		}
	}
}
