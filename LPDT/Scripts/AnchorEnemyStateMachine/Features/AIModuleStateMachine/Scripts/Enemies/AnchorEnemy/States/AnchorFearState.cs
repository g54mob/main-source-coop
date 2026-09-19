using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorFearState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		private float _previousTargetSearchRange;

		public AnchorFearState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.Fear);
			_context.SetVisualState(AnchorVisualState.Fear);
			_context.NeedToFindTargetPosition = true;
			_context.AnchorPlayerGrabSystem.Disable();
			_context.AnchorHookControlSystem.ResetHook();
			_previousTargetSearchRange = _context.TargetSearchRange;
			_context.TargetSearchRange = _context.FearDespawnRadius;
			_context.FindRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.TargetSearchRange = _previousTargetSearchRange;
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.DetectedPlayers.Count <= 0)
			{
				if (_enemy.IsDespawnAfterFear)
				{
					_enemy.RequestDespawn();
					return;
				}
				_enemy.MarkFearCompleted();
				_enemy.TriggerEvent(AnchorEvent.OnFearEscapeCompleted);
			}
		}
	}
}
