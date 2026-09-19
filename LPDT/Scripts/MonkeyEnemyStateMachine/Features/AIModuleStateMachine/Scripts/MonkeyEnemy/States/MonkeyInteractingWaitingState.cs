using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyInteractingWaitingState : StateBase<MonkeyPlayerInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyPlayerInteractionSettings _interactionSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		public MonkeyInteractingWaitingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyPlayerInteractionSettings interactionSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_interactionSettings = interactionSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.InteractingWaiting);
			_enemy.ResetStateTimerRandom(_interactionSettings.MinStateDuration, _interactionSettings.MaxStateDuration);
			_enemy.RaiseCoinRequestAnimation();
			_context.StopAgent();
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
			_context.FacePosition(_context.GetTargetPosition(), _movementSettings.RotationSpeed, _enemy.GetTickDelta());
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnInteractionStepCompleted);
			}
		}
	}
}
