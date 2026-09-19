using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabMoveToNewPosState : StateBase<CrabStateId>
	{
		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		public CrabMoveToNewPosState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(CrabStateId.MoveToNewPos);
			_context.SetVisualState(CrabVisualState.Locomotion);
			_context.CrabDamageAggrSystem.Enable();
			_context.NeedToFindTargetPosition = true;
			_context.CrabMoveSpeedSetupSystem.Enable();
			_context.CrabBurstMovementSystem.Enable();
			_context.CrabTurnSpeedSystem.Enable();
			_context.CrabSearchRangeSetupSystem.Enable();
			_context.FindRandomSafePositionSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.CrabVisionDetectingSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.CrabMoveSpeedSetupSystem.Disable();
			_context.CrabBurstMovementSystem.Disable();
			_context.CrabTurnSpeedSystem.Disable();
			_context.CrabSearchRangeSetupSystem.Disable();
			_context.FindRandomSafePositionSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.CrabVisionDetectingSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown <= 0f && _context.CanEnemyTargetPriorityPlayer())
			{
				_enemy.TriggerEvent(CrabEvent.OnTargetAcquired);
			}
			else if (_context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(CrabEvent.OnNewPosReached);
			}
		}
	}
}
