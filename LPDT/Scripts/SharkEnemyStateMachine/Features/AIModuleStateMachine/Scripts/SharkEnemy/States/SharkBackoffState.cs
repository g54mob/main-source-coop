using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkBackoffState : StateBase<SharkStateId>
	{
		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		private readonly SharkAttackSettings _attack;

		private readonly SharkMovementSettings _movement;

		public SharkBackoffState(SharkEnemy enemy, SharkEnemyContext context, SharkAttackSettings attack, SharkMovementSettings movement)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_attack = attack;
			_movement = movement;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SharkVisualState.Backoff);
			_context.ClearAttackStartSoundDebounce();
			_context.ClearStartHuntSoundRetreatState();
			if (_context.Agent.enabled)
			{
				_context.Agent.speed = _movement.ActiveMovementSpeed;
			}
			_context.BackoffElapsed = 0f;
			_context.AttackCooldownElapsed = 0f;
			_context.LineOfSightLostElapsed = 0f;
			Vector3 position = _enemy.transform.position;
			if (_context.TargetObject != null)
			{
				Vector3 vector = position - _context.TargetObject.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.0001f)
				{
					_context.BackoffDirection = vector.normalized;
				}
				else
				{
					_context.BackoffDirection = -_enemy.transform.forward;
				}
			}
			else
			{
				_context.BackoffDirection = -_enemy.transform.forward;
			}
			_context.TrySetBackoffDestination(position, _context.BackoffDirection, _attack.BackoffDistance, _movement);
		}

		public override void OnLogic()
		{
			_context.BackoffElapsed += _enemy.GetTickDelta();
		}
	}
}
