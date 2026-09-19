using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabChaseState : StateBase<CrabStateId>
	{
		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		public CrabChaseState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(CrabStateId.Chase);
			_context.SetVisualState(CrabVisualState.Locomotion);
			_context.CrabDamageAggrSystem.Enable();
			_context.CrabItemCarrySystem.DropCarriedItem();
			_context.CrabMoveSpeedSetupSystem.Enable();
			_context.CrabBurstMovementSystem.Enable();
			_context.CrabTurnSpeedSystem.Enable();
			_context.CrabSearchRangeSetupSystem.Enable();
			_context.FindTargetPlayerPositionSystem.Enable();
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.CrabVisionDetectingSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.CrabMoveSpeedSetupSystem.Disable();
			_context.CrabBurstMovementSystem.Disable();
			_context.CrabTurnSpeedSystem.Disable();
			_context.CrabSearchRangeSetupSystem.Disable();
			_context.FindTargetPlayerPositionSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.CrabVisionDetectingSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null || !_context.CanEnemyTargetPriorityPlayer())
			{
				_enemy.TriggerEvent(CrabEvent.OnTargetLost);
			}
			else if (Vector3.Distance(_enemy.transform.position, priorityPlayer.NetworkObject.transform.position) <= _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(CrabEvent.OnGrabbed);
			}
		}
	}
}
