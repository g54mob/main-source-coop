using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class PlayerTutorialGuideIdleState : StateBase<PlayerTutorialGuideStateId>
	{
		private readonly PlayerTutorialGuideEnemy _enemy;

		private readonly PlayerTutorialGuideEnemyContext _context;

		public PlayerTutorialGuideIdleState(PlayerTutorialGuideEnemy enemy, PlayerTutorialGuideEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_context.UpdateBottomPartVisibility(enabledVis: true);
			_enemy.SetCurrentStateId(PlayerTutorialGuideStateId.Idle);
			_context.SetIsAggressive(value: false);
			_context.PlayerTutorialGuideAnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
		}

		public override void OnExit()
		{
			_context.PlayerTutorialGuideAnimationSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.HasPendingDestination)
			{
				_enemy.TriggerEvent(PlayerTutorialGuideEvent.OnDestinationAssigned);
			}
		}
	}
}
