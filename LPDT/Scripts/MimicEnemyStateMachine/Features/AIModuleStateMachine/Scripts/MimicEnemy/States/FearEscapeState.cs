using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class FearEscapeState : StateBase<MimicStateId>
	{
		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		public FearEscapeState(MimicEnemy enemy, MimicEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.FearEscape);
			_enemy.SetVisualState(MimicVisualState.FearEscape);
			_context.NeedToFindTargetPosition = true;
			_context.MimicMoveStatesSetupSystem.Enable();
			_context.MimicMoveStatesSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetSearchRangeSetupSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.FindRandomSafePositionSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.CompleteDistanceSetupSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_context.PlayerVisibleSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MimicMoveStatesSetupSystem.Disable();
			_context.MimicMoveStatesSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.TargetSearchRangeSetupSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.FindRandomSafePositionSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.CompleteDistanceSetupSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
			_context.PlayerVisibleSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.DetectedPlayers.Count <= 0 && _context.VisiblePlayers.Count <= 0)
			{
				if (_context.IsDespawnAfterFear)
				{
					_enemy.TriggerEvent(MimicEvent.OnDespawnRequested);
					return;
				}
				_enemy.MarkFearEscapeCompleted();
				_enemy.TriggerEvent(MimicEvent.OnFearEscapeCompleted);
			}
		}
	}
}
