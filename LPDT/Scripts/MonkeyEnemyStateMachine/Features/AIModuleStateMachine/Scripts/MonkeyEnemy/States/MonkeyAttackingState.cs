using System.Collections;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyAttackingState : StateBase<MonkeyCombatStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyCombatSettings _combatSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private Coroutine _attackRoutine;

		public MonkeyAttackingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyCombatSettings combatSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_combatSettings = combatSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Attacking);
			_context.StopAgent();
			_enemy.RaiseAttackAnimation();
			_attackRoutine = _enemy.StartCoroutine(AttackRoutine());
		}

		public override void OnExit()
		{
			if (_attackRoutine != null)
			{
				_enemy.StopCoroutine(_attackRoutine);
				_attackRoutine = null;
			}
		}

		public override void OnLogic()
		{
			if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
			{
				_enemy.LoseInterestInTarget();
				return;
			}
			_context.ChaseElapsed += _enemy.GetTickDelta();
			if (_context.ChaseElapsed >= _combatSettings.ChaseTimeoutDuration)
			{
				_enemy.LoseInterestInTarget();
			}
			else if (_enemy.IsFull)
			{
				_enemy.LoseInterestInTarget();
			}
		}

		private IEnumerator AttackRoutine()
		{
			float attackElapsed = 0f;
			while (true)
			{
				if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
				{
					_enemy.LoseInterestInTarget();
					yield break;
				}
				if (_enemy.IsFull)
				{
					_enemy.LoseInterestInTarget();
					yield break;
				}
				_context.FacePosition(_context.GetTargetPosition(), _movementSettings.RotationSpeed, Time.deltaTime);
				yield return null;
				attackElapsed += Time.deltaTime;
				if (!(attackElapsed < _combatSettings.AttackTime))
				{
					attackElapsed = 0f;
					if (_context.GetDistanceToTarget() > _combatSettings.DistanceToAttack)
					{
						break;
					}
					_enemy.RaiseAttackAnimation();
				}
			}
			_enemy.TriggerEvent(MonkeyEvent.OnCombatRequired);
		}
	}
}
