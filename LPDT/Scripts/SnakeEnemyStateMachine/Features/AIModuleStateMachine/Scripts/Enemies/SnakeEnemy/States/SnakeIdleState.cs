using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States
{
	public class SnakeIdleState : StateBase<SnakeStateId>
	{
		private readonly SnakeEnemy _enemy;

		private readonly SnakeEnemyContext _context;

		public SnakeIdleState(SnakeEnemy enemy, SnakeEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SnakeStateId.Idle);
			_context.SetVisualState(SnakeVisualState.Idle);
			_context.ApplyBaseMoveSpeed();
			_context.NeedToFindTargetPosition = true;
			_context.SetTargetPosition(_enemy.transform.position);
			_context.SetTargetPositionCompleted(isCompleted: false);
			_context.FindSnakeRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.SnakeVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SnakeDamageReactionSystem.Enable();
		}

		public override void OnExit()
		{
			_context.FindSnakeRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.SnakeVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SnakeDamageReactionSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!(_context.PriorityPlayer?.NetworkObject == null) && !_context.IsWrapEscapeCooldownActive)
			{
				int playerId = _context.PriorityPlayer.NetworkObject.InputAuthority.PlayerId;
				if (!_context.IsPlayerOutsideGate(playerId) && !_context.IsPriorityPlayerInSafeZone())
				{
					_enemy.TriggerEvent(SnakeEvent.OnTargetAcquired);
				}
			}
		}
	}
}
