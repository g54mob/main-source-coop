using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class PlayerTutorialGuideMoveToDestinationState : StateBase<PlayerTutorialGuideStateId>
	{
		private readonly PlayerTutorialGuideEnemy _enemy;

		private readonly PlayerTutorialGuideEnemyContext _context;

		public PlayerTutorialGuideMoveToDestinationState(PlayerTutorialGuideEnemy enemy, PlayerTutorialGuideEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_context.UpdateBottomPartVisibility(enabledVis: true);
			_enemy.SetCurrentStateId(PlayerTutorialGuideStateId.MoveToDestination);
			_context.SetIsAggressive(value: false);
			_context.SetMoveSpeed(_context.NavMeshAgent.speed);
			_context.ConsumeDestination();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerTutorialGuideAnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerTutorialGuideAnimationSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(PlayerTutorialGuideEvent.OnDestinationReached);
			}
		}
	}
}
