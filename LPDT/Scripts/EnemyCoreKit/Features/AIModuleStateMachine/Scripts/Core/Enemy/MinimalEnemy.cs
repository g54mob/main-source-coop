using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy.States;
using Fusion;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Enemy
{
	[NetworkBehaviourWeaved(0)]
	public class MinimalEnemy : EnemyBase<SimpleEnemyStateId, SimpleEnemyEvent>
	{
		public override EnemyType EnemyType => EnemyType.None;

		[Inject]
		public void InjectDependencies(EnemyContextBase context, IInstantiator instantiator)
		{
			InjectBaseDependencies(context, instantiator);
		}

		protected override StateMachine<SimpleEnemyStateId, SimpleEnemyStateId, SimpleEnemyEvent> BuildFsm()
		{
			StateMachine<SimpleEnemyStateId, SimpleEnemyStateId, SimpleEnemyEvent> stateMachine = new StateMachine<SimpleEnemyStateId, SimpleEnemyStateId, SimpleEnemyEvent>();
			stateMachine.AddState(SimpleEnemyStateId.Idle, Instantiator.Instantiate<SimpleIdleState>());
			stateMachine.AddState(SimpleEnemyStateId.Chase, Instantiator.Instantiate<SimpleChaseState>());
			stateMachine.AddState(SimpleEnemyStateId.Attack, Instantiator.Instantiate<SimpleAttackState>());
			stateMachine.AddTriggerTransition(SimpleEnemyEvent.OnTargetAcquired, SimpleEnemyStateId.Idle, SimpleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(SimpleEnemyEvent.OnTargetLost, SimpleEnemyStateId.Chase, SimpleEnemyStateId.Idle);
			stateMachine.AddTriggerTransition(SimpleEnemyEvent.OnInAttackRange, SimpleEnemyStateId.Chase, SimpleEnemyStateId.Attack);
			stateMachine.AddTriggerTransition(SimpleEnemyEvent.OnOutOfAttackRange, SimpleEnemyStateId.Attack, SimpleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(SimpleEnemyEvent.OnTargetLost, SimpleEnemyStateId.Attack, SimpleEnemyStateId.Idle);
			stateMachine.SetStartState(SimpleEnemyStateId.Idle);
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
