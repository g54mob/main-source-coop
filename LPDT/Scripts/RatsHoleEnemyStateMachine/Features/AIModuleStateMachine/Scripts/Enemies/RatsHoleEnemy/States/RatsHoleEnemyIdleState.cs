using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyIdleState : StateBase<RatsHoleEnemyStateId>
	{
		private readonly RatsHoleEnemy _enemy;

		private readonly RatsHoleEnemyContext _context;

		public RatsHoleEnemyIdleState(RatsHoleEnemy enemy, RatsHoleEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(RatsHoleEnemyStateId.Idle);
			_context.SetVisualState(RatsHoleEnemyVisualState.Idle);
			_context.StopMovement();
			_context.SetSmoothedVelocity(0f);
			_context.TargetSearchRange = _context.AggroRange;
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_enemy.UpdateHostMigrationRecovery())
			{
				return;
			}
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				if (!_enemy.IsIdleDespawnBlockedAfterHostMigration())
				{
					_enemy.StartNoTargetPatrol();
				}
				return;
			}
			if (_enemy.IsPriorityPlayerInSafeZone())
			{
				_enemy.StartNoTargetPatrol();
				return;
			}
			if (_enemy.IsPriorityPlayerOutsideHomeAggro())
			{
				_enemy.StartNoTargetPatrol();
				return;
			}
			Vector3 position = priorityPlayer.NetworkObject.transform.position;
			if (GetHorizontalDistance(_enemy.transform.position, position) <= _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(RatsHoleEnemyEvent.OnInAttackRange);
			}
			else
			{
				_enemy.TriggerEvent(RatsHoleEnemyEvent.OnTargetAcquired);
			}
		}

		private static float GetHorizontalDistance(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return new Vector2(vector.x, vector.z).magnitude;
		}
	}
}
