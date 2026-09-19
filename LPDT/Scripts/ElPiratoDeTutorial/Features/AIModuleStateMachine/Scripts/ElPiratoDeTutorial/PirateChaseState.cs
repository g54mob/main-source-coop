using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateChaseState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		public PirateChaseState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyChaseSpeed();
		}

		public override void OnLogic()
		{
			if (!_pirateEnemyContext.TryGetTargetPosition(out var position))
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetDead);
			}
			else if (Vector3.Distance(position, _pirateEnemy.transform.position) < _pirateEnemyContext.PirateTutorialConfiguration.AttackRange)
			{
				_pirateEnemyContext.EnemyMovableBase.ResetPath();
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetInAttackRange);
			}
			else
			{
				_pirateEnemyContext.EnemyMovableBase.MoveToPoint(position);
			}
		}
	}
}
