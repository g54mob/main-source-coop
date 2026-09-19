using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperFearState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		public SleeperFearState(SleeperEnemy enemy, SleeperEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Fear);
			_context.SetVisualState(SleeperVisualState.Fear);
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.SleeperMoveSpeedSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			if (_enemy.IsDespawnAfterFear)
			{
				EnableFleeSystems();
				return;
			}
			_context.SetTargetPosition(_context.HomePosition);
			_context.SetTargetPositionCompleted(isCompleted: false);
		}

		public override void OnExit()
		{
			DisableFleeSystems();
			_context.SleeperMoveSpeedSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_enemy.IsDespawnAfterFear)
			{
				if (!(_context.CurrentStateTime < 0.15f) && _context.DetectedPlayers.Count <= 0)
				{
					_enemy.RequestDespawn();
				}
			}
			else if (_context.TargetPositionCompleted)
			{
				_enemy.PositionOnFearEnd = _enemy.transform.position;
				_enemy.MarkFearCompleted();
				_enemy.TriggerEvent(SleeperEvent.OnFearEscapeCompleted);
			}
		}

		private void EnableFleeSystems()
		{
			_context.NeedToFindTargetPosition = true;
			_context.CurrentStateTime = 0f;
			_context.SleeperSearchRangeSetupSystem.Enable();
			_context.FindRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		private void DisableFleeSystems()
		{
			_context.SleeperSearchRangeSetupSystem.Disable();
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}
	}
}
