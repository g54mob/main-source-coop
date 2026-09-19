using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyCombatChasingState : StateBase<MonkeyCombatStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyCombatSettings _combatSettings;

		public MonkeyCombatChasingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyCombatSettings combatSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_combatSettings = combatSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.CombatChasing);
			_enemy.SetCurrentTargetPlayerId(_context.TargetPlayer.PlayerId);
			_enemy.RaiseRunAnimation();
			_enemy.RaiseMoveAnimation();
			_context.EnableMovement();
			_context.Agent.stoppingDistance = _combatSettings.CatchUpDistance;
			_context.Agent.speed = _combatSettings.ChasingSpeed;
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
				return;
			}
			if (_enemy.IsFull)
			{
				_enemy.LoseInterestInTarget();
				return;
			}
			_context.Agent.stoppingDistance = _combatSettings.CatchUpDistance;
			_context.Agent.speed = _combatSettings.ChasingSpeed;
			_context.MoveToPosition(_context.GetTargetPosition());
		}
	}
}
