using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class TargetChasingState : StateBase<MimicStateId>
	{
		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		public TargetChasingState(MimicEnemy enemy, MimicEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.TargetChasing);
			_enemy.SetVisualState(MimicVisualState.TargetChasing);
			_context.MimicMoveStatesSetupSystem.Enable();
			_context.TimeToAttackSetupSystem.Enable();
			_context.TargetSearchRangeSetupSystem.Enable();
			_context.DistanceToAttackSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.MimicMoveStatesSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.FindTargetPlayerPositionSystem.Enable();
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.MimicTargetAttackSystem.Enable();
			_context.CompleteDistanceSetupSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_context.PlayerVisibleSystem.Enable();
			_context.MimicAnalyticsSystem.Enable();
		}

		public override void OnExit()
		{
			_context.ClearPinnedPriorityPlayer();
			_context.MimicMoveStatesSetupSystem.Disable();
			_context.TimeToAttackSetupSystem.Disable();
			_context.TargetSearchRangeSetupSystem.Disable();
			_context.DistanceToAttackSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.MimicMoveStatesSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.FindTargetPlayerPositionSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.MimicTargetAttackSystem.Disable();
			_context.CompleteDistanceSetupSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
			_context.PlayerVisibleSystem.Disable();
			_context.MimicAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_enemy.CanEnemyInteractWithPriorityPlayer())
			{
				if (_context.IsAttackInProcess)
				{
					_context.MimicTargetAttackSystem.Clear();
				}
				_enemy.TriggerEvent(MimicEvent.OnTargetLost);
			}
			else if (_context.IsAttackOnCooldown)
			{
				_enemy.TriggerEvent(MimicEvent.OnAttackCooldown);
			}
			else if (!_context.IsAttackInProcess && _context.PriorityPlayer == null)
			{
				_enemy.TriggerEvent(MimicEvent.OnTargetLost);
			}
		}
	}
}
