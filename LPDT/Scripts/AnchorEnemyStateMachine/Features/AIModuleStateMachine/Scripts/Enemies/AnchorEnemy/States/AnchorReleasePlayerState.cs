using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorReleasePlayerState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		private bool _thrown;

		public AnchorReleasePlayerState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.ReleasePlayer);
			_context.SetVisualState(AnchorVisualState.ReleasePlayer);
			_context.SetSmoothedVelocity(0f);
			_context.ClearPlayerThrowRelease();
			_thrown = false;
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AnchorPlayerGrabSystem.Disable();
			_context.AttackCooldown = _context.ThrowCooldown;
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_thrown)
			{
				return;
			}
			if (_context.AnchorPlayerGrabSystem.ConsumeHeldPlayerDeparted() || !_context.AnchorPlayerGrabSystem.IsHolding || !_context.IsPlayerSessionActive(_context.AnchorPlayerGrabSystem.GrabbedPlayerId))
			{
				CompleteReleaseWithoutThrow();
				return;
			}
			bool playerThrowReleaseRequested = _context.PlayerThrowReleaseRequested;
			bool flag = _context.CurrentStateTime >= _context.ReleaseDuration;
			if (playerThrowReleaseRequested || flag)
			{
				_context.AnchorPlayerGrabSystem.ThrowHeldPlayer();
				_context.ClearPlayerThrowRelease();
				_thrown = true;
				_enemy.TriggerEvent(AnchorEvent.OnReleaseCompleted);
			}
		}

		private void CompleteReleaseWithoutThrow()
		{
			_context.AnchorPlayerGrabSystem.Disable();
			_context.ClearPlayerThrowRelease();
			_thrown = true;
			_enemy.TriggerEvent(AnchorEvent.OnReleaseCompleted);
		}
	}
}
