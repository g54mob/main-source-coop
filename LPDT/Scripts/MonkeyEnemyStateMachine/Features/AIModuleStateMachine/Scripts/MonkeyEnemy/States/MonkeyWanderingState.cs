using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyWanderingState : StateBase<MonkeyStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyMovementSettings _movementSettings;

		public MonkeyWanderingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Wandering);
			_enemy.SetCurrentTargetPlayerId(-1);
			_enemy.RaiseMoveSlowAnimation();
			_context.TargetPlayer = PlayerRef.None;
			_context.ClearTargetItem();
			_context.EnableMovement();
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			UpdateMovement();
		}

		public override void OnLogic()
		{
			_context.MovementUpdateElapsed += _enemy.GetTickDelta();
			if (!(_context.MovementUpdateElapsed < _context.MovementUpdateInterval))
			{
				UpdateMovement();
			}
		}

		private void UpdateMovement()
		{
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			_context.MoveToPosition(_context.GetRandomNavmeshPosition(_context.transform.position, _movementSettings.WanderRadius));
			ResetMovementTimer();
		}

		private void ResetMovementTimer()
		{
			_context.MovementUpdateElapsed = 0f;
			_context.MovementUpdateInterval = Random.Range(_movementSettings.MinWanderPositionUpdateTime, _movementSettings.MaxWanderPositionUpdateTime);
		}
	}
}
