using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
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
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemyContext.PirateAnimationController.TriggerRangeAttack();
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: true);
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded += FinishAttack;
			_pirateEnemyContext.PirateAnimationController.OnAttack += ProcessAttack;
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.EnableAim();
			}
		}

		public override void OnExit()
		{
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: false);
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded -= FinishAttack;
			_pirateEnemyContext.PirateAnimationController.OnAttack -= ProcessAttack;
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.DisableAim();
			}
		}

		public override void OnLogic()
		{
			if (_pirateEnemyContext.TryGetTargetPosition(out var position))
			{
				_pirateEnemyContext.PirateAttackRotationComponent.RotateTowardsTarget(position, 3f);
			}
		}

		private void ProcessAttack()
		{
			if (_pirateEnemyContext.TryGetTargetPosition(out var position))
			{
				_pirateEnemyContext.PirateAttackController.Attack(position);
				if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
				{
					_pirateEnemyContext.PirateAimController.DisableAim();
				}
			}
		}

		private void FinishAttack()
		{
			_pirateEnemy.TriggerEvent(_pirateEnemyContext.IsTargetValid() ? PirateEvent.OnAttackFinished : PirateEvent.OnTargetDead);
		}
	}
}
