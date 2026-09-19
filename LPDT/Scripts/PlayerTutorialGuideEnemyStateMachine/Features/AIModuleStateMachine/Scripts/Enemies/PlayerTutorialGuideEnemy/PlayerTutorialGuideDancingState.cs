using Features.Movement.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class PlayerTutorialGuideDancingState : StateBase<PlayerTutorialGuideStateId>
	{
		private readonly PlayerTutorialGuideEnemy _enemy;

		private readonly PlayerTutorialGuideEnemyContext _context;

		private readonly PlayerMovableModel _playerMovableModel;

		private Vector3 _rotationDirection;

		private bool _hasRotationDirection;

		public PlayerTutorialGuideDancingState(PlayerTutorialGuideEnemy enemy, PlayerTutorialGuideEnemyContext context, PlayerMovableModel playerMovableModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_playerMovableModel = playerMovableModel;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(PlayerTutorialGuideStateId.Dancing);
			_context.UpdateBottomPartVisibility(enabledVis: true);
			_context.SetIsDancing(value: true);
		}
	}
}
