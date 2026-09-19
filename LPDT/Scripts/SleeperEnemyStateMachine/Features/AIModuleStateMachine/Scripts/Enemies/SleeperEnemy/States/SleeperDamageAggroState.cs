using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperDamageAggroState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperDamageAggroState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.DamageAggro);
			_context.SetVisualState(SleeperVisualState.DamageAggro);
			_context.SetMoveSpeed(_context.GetStatValue(EntityStatType.SprintSpeed));
			_context.DistanceToAttack = _settings.DistanceToAttack;
			_context.NavMeshAgent.stoppingDistance = _settings.AttackStopDistance;
			_context.FindTargetPlayerPositionSystem.Enable();
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
		}

		public override void OnExit()
		{
			_context.FindTargetPlayerPositionSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				_enemy.TriggerEvent(SleeperEvent.OnDamageAggroTimeout);
			}
			else if (Vector3.Distance(_enemy.transform.position, priorityPlayer.NetworkObject.transform.position) <= _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(SleeperEvent.OnInAttackRange);
			}
			else if (_context.CurrentStateTime >= _settings.DamageAggroDuration)
			{
				_enemy.TriggerEvent(SleeperEvent.OnDamageAggroTimeout);
			}
		}
	}
}
