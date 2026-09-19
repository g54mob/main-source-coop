using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateIdleState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		public PirateIdleState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyWalkingSpeed();
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemyContext.TargetEnemy = null;
		}

		public override void OnLogic()
		{
			if (_pirateEnemyContext.TargetDetector.TryGetNearestEnemy(out var target))
			{
				_pirateEnemyContext.TargetEnemy = target;
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetAcquired);
			}
		}
	}
}
