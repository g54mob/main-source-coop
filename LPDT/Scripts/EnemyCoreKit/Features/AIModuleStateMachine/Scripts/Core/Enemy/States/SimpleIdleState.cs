using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Core.Enemy.States
{
	public class SimpleIdleState : StateBase<SimpleEnemyStateId>
	{
		private readonly MinimalEnemy _enemy;

		private readonly EnemyContextBase _context;

		public SimpleIdleState(MinimalEnemy enemy, EnemyContextBase context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SimpleEnemyStateId.Idle);
			_context.NeedToFindTargetPosition = true;
			_context.FindRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.PriorityPlayer != null)
			{
				_enemy.TriggerEvent(SimpleEnemyEvent.OnTargetAcquired);
			}
		}
	}
}
