using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.States;
using Features.EnemyFearingModule.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(1)]
	public class ParrotMobEnemy : EnemyBase<ParrotMobStateId, ParrotMobEvent>, IEnemyFearCallbackListener
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ParrotMobVisualState _VisualState;

		private ParrotMobEnemyContext _parrotContext;

		private ParrotMobAudioController _audioController;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe ParrotMobVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(ParrotMobVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParrotMobEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(ParrotMobVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		protected override bool UsesNavMeshAgent => false;

		public override EnemyType EnemyType => EnemyType.ParrotMob;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public bool IsDespawnAfterFear
		{
			get
			{
				return _parrotContext.IsDespawnAfterFear;
			}
			set
			{
				_parrotContext.IsDespawnAfterFear = value;
			}
		}

		[Inject]
		public void InjectDependencies(ParrotMobEnemyContext context, ParrotMobAudioController audioController, IInstantiator instantiator)
		{
			InjectBaseDependencies(context, instantiator);
			_parrotContext = context;
			_audioController = audioController;
		}

		protected override StateMachine<ParrotMobStateId, ParrotMobStateId, ParrotMobEvent> BuildFsm()
		{
			StateMachine<ParrotMobStateId, ParrotMobStateId, ParrotMobEvent> stateMachine = new StateMachine<ParrotMobStateId, ParrotMobStateId, ParrotMobEvent>();
			stateMachine.AddState(ParrotMobStateId.Idle, Instantiator.Instantiate<ParrotMobIdleState>());
			stateMachine.AddState(ParrotMobStateId.Alert, Instantiator.Instantiate<ParrotMobAlertState>());
			stateMachine.AddState(ParrotMobStateId.Scream, Instantiator.Instantiate<ParrotMobScreamState>());
			stateMachine.AddTriggerTransition(ParrotMobEvent.OnPlayerDetected, ParrotMobStateId.Idle, ParrotMobStateId.Alert);
			stateMachine.AddTriggerTransition(ParrotMobEvent.OnPlayerLost, ParrotMobStateId.Alert, ParrotMobStateId.Idle);
			stateMachine.AddTriggerTransition(ParrotMobEvent.OnAlertCompleted, ParrotMobStateId.Alert, ParrotMobStateId.Scream);
			stateMachine.AddTriggerTransition(ParrotMobEvent.OnScreamCompleted, ParrotMobStateId.Scream, ParrotMobStateId.Idle);
			stateMachine.AddTriggerTransition(ParrotMobEvent.OnRepeatScream, ParrotMobStateId.Idle, ParrotMobStateId.Scream);
			stateMachine.AddTriggerTransitionFromAny(ParrotMobEvent.OnDamage, ParrotMobStateId.Scream, (Transition<ParrotMobStateId> transition) => base.CurrentStateId != ParrotMobStateId.Scream && !_parrotContext.IsDead && !_parrotContext.IsFearing, null, null, forceInstantly: true);
			stateMachine.SetStartState(ParrotMobStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			DisableNavMeshAgent();
		}

		public override void StateAuthorityChanged()
		{
			base.StateAuthorityChanged();
			DisableNavMeshAgent();
			if (base.HasStateAuthority)
			{
				_audioController.StopScream();
				SetVisualState(ParrotMobVisualState.Idle);
				_parrotContext.IsZoneEngagementActive = false;
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!_parrotContext.IsDead)
			{
				base.FixedUpdateNetwork();
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

		public void SetVisualState(ParrotMobVisualState visualState)
		{
			if (base.HasStateAuthority && VisualState != visualState)
			{
				VisualState = visualState;
			}
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_parrotContext.IsFearing = true;
			}
		}

		private void DisableNavMeshAgent()
		{
			if (_parrotContext.NavMeshAgent != null)
			{
				_parrotContext.NavMeshAgent.enabled = false;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			VisualState = _VisualState;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_VisualState = VisualState;
		}
	}
}
