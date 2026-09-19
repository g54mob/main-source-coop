using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyLastChanceState : StateBase<MonkeyStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyPlayerInteractionSettings _interactionSettings;

		private readonly MonkeyLastChanceSettings _lastChanceSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private bool _isBegging;

		public MonkeyLastChanceState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyPlayerInteractionSettings interactionSettings, MonkeyLastChanceSettings lastChanceSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_interactionSettings = interactionSettings;
			_lastChanceSettings = lastChanceSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.LastChance);
			_enemy.SetCurrentTargetPlayerId(_context.TargetPlayer.PlayerId);
			_enemy.ResetStateTimer(_lastChanceSettings.Duration);
			_isBegging = false;
			_context.EnableMovement();
			_context.Agent.stoppingDistance = _interactionSettings.InteractionCatchUpDistance;
			_context.Agent.speed = _interactionSettings.InteractionChaseSpeed;
			_enemy.RaiseRunAnimation();
			_enemy.RaiseMoveAnimation();
		}

		public override void OnLogic()
		{
			if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
			{
				_enemy.LoseInterestInTarget();
				return;
			}
			if (_context.GetDistanceToTarget() > _interactionSettings.InteractionCatchUpDistance && !_isBegging)
			{
				_isBegging = false;
				_context.Agent.stoppingDistance = _interactionSettings.InteractionCatchUpDistance;
				_context.Agent.speed = _interactionSettings.InteractionChaseSpeed;
				_context.MoveToPosition(_context.GetTargetPosition());
				return;
			}
			_context.StopAgent();
			_context.FacePosition(_context.GetTargetPosition(), _movementSettings.RotationSpeed, _enemy.GetTickDelta());
			if (!_isBegging)
			{
				_enemy.RaiseCoinRequestAnimation();
				_isBegging = true;
			}
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnCombatRequired);
			}
		}
	}
}
