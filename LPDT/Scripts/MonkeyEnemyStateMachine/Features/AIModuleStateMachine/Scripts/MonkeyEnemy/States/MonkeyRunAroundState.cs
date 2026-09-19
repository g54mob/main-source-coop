using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyRunAroundState : StateBase<MonkeyPlayerInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyPlayerInteractionSettings _interactionSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		public MonkeyRunAroundState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyPlayerInteractionSettings interactionSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_interactionSettings = interactionSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.RunAround);
			_enemy.RaiseMoveMiddleAnimation();
			_enemy.RaiseMoveAnimation();
			_enemy.ResetStateTimerRandom(_interactionSettings.MinStateDuration, _interactionSettings.MaxStateDuration);
			_context.MovementUpdateElapsed = _interactionSettings.RunAroundUpdateInterval;
			_context.EnableMovement();
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
			_context.Agent.speed = _interactionSettings.RunAroundSpeed;
		}

		public override void OnLogic()
		{
			if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
			{
				_enemy.LoseInterestInTarget();
				return;
			}
			if (_enemy.IsStarving)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnCombatRequired);
				return;
			}
			_context.MovementUpdateElapsed += _enemy.GetTickDelta();
			if (_context.MovementUpdateElapsed >= _interactionSettings.RunAroundUpdateInterval)
			{
				_context.MoveToPosition(_context.GetRandomNavmeshPosition(_context.GetTargetPosition(), _interactionSettings.RunAroundSearchRadius));
				_context.MovementUpdateElapsed = 0f;
			}
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnInteractionStepCompleted);
			}
		}
	}
}
