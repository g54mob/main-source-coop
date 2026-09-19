using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.GrabModule.Scripts;
using Features.ItemSpawnerModule;
using Fusion;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerTutorialGuideEnemy : EnemyBase<PlayerTutorialGuideStateId, PlayerTutorialGuideEvent>, IEnemyDeadStateProvider, IEnemyTargetProvider
	{
		private PlayerTutorialGuideEnemyContext _context;

		public override EnemyType EnemyType => EnemyType.PlayerTutorialGuide;

		public PlayerTutorialGuideEnemyContext CastedContext => _context;

		public bool IsDead => _context.IsDead;

		public Transform HitTarget => _context.ShootTarget;

		public event Action<bool> OnIsDeadChanged;

		[Inject]
		public void InjectDependencies(PlayerTutorialGuideEnemyContext context, IInstantiator instantiator)
		{
			_context = context;
			InjectBaseDependencies(context, instantiator);
		}

		protected override StateMachine<PlayerTutorialGuideStateId, PlayerTutorialGuideStateId, PlayerTutorialGuideEvent> BuildFsm()
		{
			StateMachine<PlayerTutorialGuideStateId, PlayerTutorialGuideStateId, PlayerTutorialGuideEvent> stateMachine = new StateMachine<PlayerTutorialGuideStateId, PlayerTutorialGuideStateId, PlayerTutorialGuideEvent>();
			stateMachine.AddState(PlayerTutorialGuideStateId.Idle, Instantiator.Instantiate<PlayerTutorialGuideIdleState>());
			stateMachine.AddState(PlayerTutorialGuideStateId.MoveToDestination, Instantiator.Instantiate<PlayerTutorialGuideMoveToDestinationState>());
			stateMachine.AddState(PlayerTutorialGuideStateId.Dead, Instantiator.Instantiate<PlayerTutorialGuideDeadState>());
			stateMachine.AddState(PlayerTutorialGuideStateId.Dancing, Instantiator.Instantiate<PlayerTutorialGuideDancingState>());
			stateMachine.AddTriggerTransition(PlayerTutorialGuideEvent.OnDestinationAssigned, PlayerTutorialGuideStateId.Idle, PlayerTutorialGuideStateId.MoveToDestination);
			stateMachine.AddTriggerTransition(PlayerTutorialGuideEvent.OnDestinationReached, PlayerTutorialGuideStateId.MoveToDestination, PlayerTutorialGuideStateId.Idle);
			stateMachine.AddTriggerTransition(PlayerTutorialGuideEvent.OnDied, PlayerTutorialGuideStateId.Idle, PlayerTutorialGuideStateId.Dead);
			stateMachine.AddTriggerTransition(PlayerTutorialGuideEvent.OnDied, PlayerTutorialGuideStateId.MoveToDestination, PlayerTutorialGuideStateId.Dead);
			stateMachine.AddTriggerTransition(PlayerTutorialGuideEvent.OnRevived, PlayerTutorialGuideStateId.Dead, PlayerTutorialGuideStateId.Dancing);
			stateMachine.SetStartState(PlayerTutorialGuideStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority && _context.HealthController != null)
			{
				_context.HealthController.OnEnemyDead += HandleDied;
			}
			if (base.HasStateAuthority)
			{
				_context.CarryItemSpawner.Spawn();
				_context.CarryItemSpawner.OnItemSpawned += OnCarryItemSpawned;
			}
			_context.OnIsDeadChanged += InvokeIsDeadChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (base.HasStateAuthority && _context.HealthController != null)
			{
				_context.HealthController.OnEnemyDead -= HandleDied;
			}
			_context.OnIsDeadChanged -= InvokeIsDeadChanged;
			_context.CarryItemSpawner.OnItemSpawned -= OnCarryItemSpawned;
			base.Despawned(runner, hasState);
		}

		private void OnCarryItemSpawned(ItemSpawnerBase itemSpawnerBase)
		{
			if (itemSpawnerBase.SpawnedInstance.TryGetComponent<SimplePointGrabable>(out var component))
			{
				_context.CarryItemGrabbable = component;
				_context.CarryItemGrabbable.ChangeRigidbodyKinematic = false;
				_context.CarryItemGrabbable.Rigidbody.isKinematic = true;
				_context.CarryItemGrabbable.Rigidbody.interpolation = RigidbodyInterpolation.None;
			}
			if (itemSpawnerBase.SpawnedInstance.TryGetComponent<TransformReplicator>(out var component2))
			{
				_context.CarryItemTransformReplicator = component2;
				_context.CarryItemTransformReplicator.StartReplication(_context.CarryItemSpawner.transform);
			}
		}

		public void SetDestination(Vector3 position)
		{
			_context.SetDestination(position);
		}

		public void SwitchCanBeRevived(bool canBeRevived)
		{
			_context.CanBeRevived = canBeRevived;
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		private void HandleDied()
		{
			TriggerEvent(PlayerTutorialGuideEvent.OnDied);
		}

		private void InvokeIsDeadChanged(bool value)
		{
			this.OnIsDeadChanged?.Invoke(value);
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
