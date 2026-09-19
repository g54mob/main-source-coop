using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyPatrolState : StateBase<RatsHoleEnemyStateId>
	{
		private readonly RatsHoleEnemy _enemy;

		private readonly RatsHoleEnemyContext _context;

		private readonly RatsHoleEnemyMovementSettings _movementSettings;

		public RatsHoleEnemyPatrolState(RatsHoleEnemy enemy, RatsHoleEnemyContext context, RatsHoleEnemyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(RatsHoleEnemyStateId.Patrol);
			_context.SetVisualState(RatsHoleEnemyVisualState.Wander);
			_context.SetPriorityPlayer(null);
			_context.MoveSpeedSetupSystem.Enable();
			_context.TargetSearchRange = _context.AggroRange;
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.NeedToFindTargetPosition = true;
			_context.FindRandomPositionSystem.ConfigureSearchRadius(_movementSettings.PatrolRadius);
			_context.FindRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSpeedSetupSystem.Disable();
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_enemy.TryReacquireAvailableTarget() && _context.CurrentStateTime >= Mathf.Max(0f, _movementSettings.PatrolDuration))
			{
				_enemy.TriggerEvent(RatsHoleEnemyEvent.OnPatrolTimeout);
			}
		}
	}
}
