using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperGoHomeState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperGoHomeState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.GoHome);
			_context.SetVisualState(SleeperVisualState.GoHome);
			_context.SetMoveSpeed(_context.GetStatValue(EntityStatType.WalkSpeed));
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.DistanceToAttack = _settings.DistanceToAttack;
			_context.SetTargetPosition(_context.HomePosition);
			_context.SetTargetPositionCompleted(isCompleted: false);
			_context.SleeperSearchRangeSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
			_context.AttackCooldownSystem.Enable();
		}

		public override void OnExit()
		{
			_context.SleeperSearchRangeSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
			_context.AttackCooldownSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown <= 0f && _context.PriorityPlayer != null)
			{
				_enemy.TriggerEvent(SleeperEvent.OnInAttackRange);
			}
			else if (_context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(SleeperEvent.OnReachedHome);
			}
		}
	}
}
