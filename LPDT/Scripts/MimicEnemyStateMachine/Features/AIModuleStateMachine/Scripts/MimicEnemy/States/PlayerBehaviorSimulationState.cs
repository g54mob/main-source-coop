using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class PlayerBehaviorSimulationState : StateBase<MimicStateId>
	{
		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		public PlayerBehaviorSimulationState(MimicEnemy enemy, MimicEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.PlayerBehaviorSimulation);
			_enemy.SetVisualState(MimicVisualState.PlayerBehaviorSimulation);
			_context.ReadyForAttackTrackSystem.Enable();
			_context.TimeToAttackSetupSystem.Enable();
			_context.MimicMoveStatesSetupSystem.Enable();
			_context.DistanceToAttackSetupSystem.Enable();
			_context.TargetSearchRangeSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.MimicMoveStatesSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.FindInterestPointPositionResetSystem.Enable();
			_context.FindInterestPointPositionSystem.Enable();
			_context.MimicInterestPointsDetectSystem.Enable();
			_context.CompleteDistanceSetupSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_context.PlayerVisibleSystem.Enable();
			_context.MimicAnalyticsSystem.Enable();
		}

		public override void OnExit()
		{
			_context.ReadyForAttackTrackSystem.Disable();
			_context.TimeToAttackSetupSystem.Disable();
			_context.MimicMoveStatesSetupSystem.Disable();
			_context.DistanceToAttackSetupSystem.Disable();
			_context.TargetSearchRangeSetupSystem.Disable();
			_context.MimicMoveStatesSystem.Disable();
			_context.MoveSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.FindInterestPointPositionResetSystem.Disable();
			_context.FindInterestPointPositionSystem.Disable();
			_context.MimicInterestPointsDetectSystem.Disable();
			_context.CompleteDistanceSetupSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
			_context.PlayerVisibleSystem.Disable();
			_context.MimicAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime > _context.BehaviorSimulationTime)
			{
				_enemy.TriggerEvent(MimicEvent.OnSimulationComplete);
			}
			else if (_context.IsReadyForAttack)
			{
				_context.IsReadyForAttack = false;
				_enemy.TriggerEvent(MimicEvent.OnReadyForAttack);
			}
		}
	}
}
