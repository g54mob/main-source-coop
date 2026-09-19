using Features.DamageableTrackModule.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class MeleeAttackState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		public MeleeAttackState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			if (_pirateEnemyContext.HasSafeZoneAttackPosition)
			{
				_pirateEnemyContext.PirateAnimationController.TriggerSafeZoneMeleeAttack();
			}
			else
			{
				_pirateEnemyContext.PirateAnimationController.TriggerMeleeAttack();
			}
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded += ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack += ProcessAttack;
		}

		public override void OnExit()
		{
			_pirateEnemyContext.PirateAnimationController.OnAttackEnded -= ResetToWaitState;
			_pirateEnemyContext.PirateAnimationController.OnAttack -= ProcessAttack;
		}

		private void ProcessAttack()
		{
			if (_pirateEnemyContext.CanEnemyInteractWithTarget() && _pirateEnemyContext.TryGetTargetMovePosition(out var targetPosition) && !(Vector3.Distance(targetPosition, _pirateEnemy.transform.position) > _pirateEnemyContext.PirateConfiguration.MeleeAttackRange) && (_pirateEnemyContext.HasSafeZoneAttackPosition || _pirateEnemyContext.CanAttackTargetWithLineOfSight()) && _pirateEnemyContext.TryGetTargetDamageable(out var damageable))
			{
				DamageData damage = new DamageData
				{
					Damage = _pirateEnemyContext.PirateConfiguration.MeleeAttackDamage,
					Position = _pirateEnemy.transform.position,
					Direction = (targetPosition - _pirateEnemy.transform.position).normalized,
					Force = _pirateEnemyContext.PirateConfiguration.MeleeAttackImpulseStrength,
					ForceMode = ForceMode.Impulse,
					IsStunning = false,
					Source = DamageDataSourceExtensions.ForEnemyAttack(_pirateEnemy.transform, _pirateEnemy.EnemyType.ToString(), DamageType.Melee)
				};
				damageable.Damage(damage);
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
				_pirateEnemyContext.PirateAttackRotationComponent.RotateTowardsTarget(targetPosition);
			}
		}
	}
}
