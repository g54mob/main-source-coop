using Features.Movement.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class SafeZoneAttackState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		private bool _attackStarted;

		private readonly PlayerMovableModel _playerMovableModel;

		public SafeZoneAttackState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext, PlayerMovableModel playerMovableModel)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
			_playerMovableModel = playerMovableModel;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyChaseSpeed();
			_attackStarted = false;
			_pirateEnemyContext.IsSafeZoneAttackActive = true;
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded += ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack += ProcessAttack;
			if (!_pirateEnemyContext.HasSafeZoneAttackPosition)
			{
				ResetToWaitState();
				return;
			}
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: true);
			_pirateEnemyContext.EnemyMovableBase.MoveToPoint(_pirateEnemyContext.SafeZoneAttackPosition);
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.EnableAim();
			}
		}

		public override void OnExit()
		{
			_pirateEnemyContext.IsSafeZoneAttackActive = false;
			_pirateEnemyContext.PirateAttackController.EnableWeaponVisual(enable: false);
			if (_pirateEnemyContext.PirateAttackController.IsWithAiming)
			{
				_pirateEnemyContext.PirateAimController.DisableAim();
			}
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded -= ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack -= ProcessAttack;
			_pirateEnemyContext.ReleaseSafeZoneAttackPoint();
			_pirateEnemyContext.EnemyMovableBase.EnableNavMeshAgent(enable: true);
			_pirateEnemyContext.TargetDetector.EnableDetecting();
		}

		public override void OnLogic()
		{
			if (!_pirateEnemyContext.CanEnemyInteractWithTarget())
			{
				ResetToWaitState();
			}
			else if (!_pirateEnemyContext.IsTargetAlive())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
			}
			else if (_attackStarted)
			{
				if (_playerMovableModel.AllCharacterMovables.TryGetValue(_pirateEnemyContext.TargetPlayer, out var value) && value != null)
				{
					_pirateEnemyContext.PirateAttackRotationComponent.RotateTowardsTarget(value.transform.position);
				}
			}
			else if (!_pirateEnemyContext.IsTargetInSafeZone())
			{
				ResetToWaitState();
			}
			else if (_pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
			{
				if (_pirateEnemyContext.TryGetTargetPosition(out var targetPosition) && _pirateEnemyContext.PirateSafeZoneDetector != null && _pirateEnemyContext.PirateSafeZoneDetector.TryFindBlockingSafeZoneUnderPlayer(targetPosition, out var safeZone) && !_pirateEnemyContext.CanUseBottomSafeZoneAttack(safeZone))
				{
					ResetToWaitState();
					return;
				}
				_pirateEnemyContext.EnemyMovableBase.ResetPath();
				_pirateEnemyContext.EnemyMovableBase.EnableNavMeshAgent(enable: false);
				_pirateEnemyContext.TargetDetector.DisableDetecting();
				_attackStarted = true;
				_pirateEnemyContext.PirateAnimationController.TriggerSafeZoneAttack();
			}
		}

		private void ProcessAttack()
		{
			if (_pirateEnemyContext.IsTargetInSafeZone() && _playerMovableModel.AllCharacterMovables.TryGetValue(_pirateEnemyContext.TargetPlayer, out var value) && !(value == null))
			{
				Vector3 position = value.CameraPositionTransform.position;
				_pirateEnemyContext.PirateAttackController.Attack(position);
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
	}
}
