using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyInteractionChasingState : StateBase<MonkeyPlayerInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyPlayerInteractionSettings _interactionSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		public MonkeyInteractionChasingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyPlayerInteractionSettings interactionSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_interactionSettings = interactionSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.InteractionChasing);
			_enemy.SetCurrentTargetPlayerId(_context.TargetPlayer.PlayerId);
			_enemy.RaiseRunAnimation();
			_enemy.RaiseMoveAnimation();
			_context.EnableMovement();
			_context.Agent.stoppingDistance = _interactionSettings.InteractionCatchUpDistance;
			_context.Agent.speed = _interactionSettings.InteractionChaseSpeed;
		}

		public override void OnLogic()
		{
			if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
			{
				_enemy.LoseInterestInTarget();
			}
			else if (_enemy.IsStarving)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnCombatRequired);
			}
			else if (_context.GetDistanceToTarget() >= _interactionSettings.DistanceToContinueChasing)
			{
				_context.Agent.stoppingDistance = _interactionSettings.InteractionCatchUpDistance;
				_context.Agent.speed = _interactionSettings.InteractionChaseSpeed;
				_context.MoveToPosition(_context.GetTargetPosition());
			}
			else if (!(_context.GetDistanceToTarget() <= _interactionSettings.InteractionCatchUpDistance))
			{
				_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
				_context.MoveToPosition(_context.GetTargetPosition());
			}
		}
	}
}
