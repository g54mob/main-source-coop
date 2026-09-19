using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class MoveToNewAreaState : StateBase<MimicStateId>
	{
		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		public MoveToNewAreaState(MimicEnemy enemy, MimicEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.MoveToNewArea);
			_enemy.SetVisualState(MimicVisualState.MoveToNewArea);
			_context.NeedToFindTargetPosition = true;
			_context.MimicMoveStatesSetupSystem.Enable();
			_context.TimeToAttackSetupSystem.Enable();
			_context.DistanceToAttackSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.MimicMoveStatesSystem.Enable();
			_context.FindRandomSafePositionSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.CompleteDistanceSetupSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_context.PlayerVisibleSystem.Enable();
			_context.MimicAnalyticsSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MimicMoveStatesSetupSystem.Disable();
			_context.TimeToAttackSetupSystem.Disable();
			_context.DistanceToAttackSetupSystem.Disable();
			_context.MimicMoveStatesSystem.Disable();
			_context.MoveSystem.Disable();
			_context.FindRandomSafePositionSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.CompleteDistanceSetupSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
			_context.PlayerVisibleSystem.Disable();
			_context.MimicAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(MimicEvent.OnTargetPositionCompleted);
			}
		}
	}
}
