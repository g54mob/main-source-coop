using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorGrabPlayerState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorGrabPlayerState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.GrabPlayer);
			_context.SetVisualState(AnchorVisualState.GrabPlayer);
			_context.SetSmoothedVelocity(0f);
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.AnchorHookControlSystem.ResetHook();
			_context.AnchorPlayerGrabSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_context.AnchorPlayerGrabSystem.IsHolding || !_context.IsPlayerSessionActive(_context.AnchorPlayerGrabSystem.GrabbedPlayerId))
			{
				EscapeToChase();
			}
			else if (_context.CurrentStateTime >= _context.HoldPlayerDuration)
			{
				_enemy.TriggerEvent(AnchorEvent.OnGrabReleased);
			}
		}

		private void EscapeToChase()
		{
			_context.AnchorPlayerGrabSystem.Disable();
			_enemy.TriggerEvent(AnchorEvent.OnGrabEscaped);
		}
	}
}
