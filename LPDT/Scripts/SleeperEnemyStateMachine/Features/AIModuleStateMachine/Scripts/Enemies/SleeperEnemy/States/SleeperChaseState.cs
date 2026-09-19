using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperChaseState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperChaseState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Chase);
			_context.SetVisualState(SleeperVisualState.Chase);
			_context.SetMoveSpeed(_context.GetStatValue(EntityStatType.SprintSpeed));
			_context.DistanceToAttack = _settings.DistanceToAttack;
			_context.NavMeshAgent.stoppingDistance = _settings.AttackStopDistance;
			_context.SleeperSearchRangeSetupSystem.Enable();
			_context.FindTargetPlayerPositionSystem.Enable();
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.SleeperChaseGiveUpSystem.Enable();
			_context.AttackCooldownSystem.Enable();
		}

		public override void OnExit()
		{
			_context.SleeperSearchRangeSetupSystem.Disable();
			_context.FindTargetPlayerPositionSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.SleeperChaseGiveUpSystem.Disable();
			_context.AttackCooldownSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown > 0f)
			{
				_enemy.TriggerEvent(SleeperEvent.OnChaseTimeout);
				return;
			}
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer != null && !(priorityPlayer.NetworkObject == null) && Vector3.Distance(_enemy.transform.position, priorityPlayer.NetworkObject.transform.position) <= _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(SleeperEvent.OnInAttackRange);
			}
		}
	}
}
