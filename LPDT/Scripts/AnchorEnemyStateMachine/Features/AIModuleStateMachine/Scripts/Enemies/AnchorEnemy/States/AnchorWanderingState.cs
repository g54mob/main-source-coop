using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorWanderingState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorWanderingState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.Wandering);
			_context.SetVisualState(AnchorVisualState.Wandering);
			_context.NeedToFindTargetPosition = true;
			_context.ClearAuxTarget();
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.AnchorAreaRelocateSystem.Enable();
			_context.AnchorAreaRoamSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.MoveSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.AnchorVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.AnchorAreaRelocateSystem.Disable();
			_context.AnchorAreaRoamSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.AnchorVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.HasLivePriorityPlayer())
			{
				_enemy.TriggerEvent(AnchorEvent.OnTargetAcquired);
			}
			else if (_context.TryAcquireAuxTarget(_enemy.GetTickDelta()))
			{
				_enemy.TriggerEvent(AnchorEvent.OnTargetAcquired);
			}
		}
	}
}
