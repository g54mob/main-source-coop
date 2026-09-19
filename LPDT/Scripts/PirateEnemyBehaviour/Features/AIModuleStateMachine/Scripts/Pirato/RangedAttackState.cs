using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class RangedAttackState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		public RangedAttackState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.PirateAnimationController.TriggerRangeAttack();
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: true);
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded += ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack += ProcessAttack;
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.EnableAim();
			}
		}

		public override void OnExit()
		{
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: false);
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded -= ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack -= ProcessAttack;
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.DisableAim();
			}
		}

		private void ProcessAttack()
		{
			if (_pirateEnemyContext.CanAttackTargetWithLineOfSight() && _pirateEnemyContext.TryGetTargetPosition(out var targetPosition))
			{
				_pirateEnemyContext.PirateAttackController.Attack(targetPosition);
				if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
				{
					_pirateEnemyContext.PirateAimController.DisableAim();
				}
			}
		}

		private void ResetToWaitState()
		{
			_pirateEnemy.TriggerEvent(PirateEvent.OnWaitToAttack);
		}

		public override void OnLogic()
		{
			Vector3 targetPosition;
			if (!_pirateEnemyContext.CanEnemyInteractWithTarget())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
			}
			else if (!_pirateEnemyContext.TryGetTargetMovePosition(out targetPosition))
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
			}
			else
			{
				_pirateEnemyContext.PirateAttackRotationComponent.RotateTowardsTarget(targetPosition, 3f);
			}
		}
	}
}
