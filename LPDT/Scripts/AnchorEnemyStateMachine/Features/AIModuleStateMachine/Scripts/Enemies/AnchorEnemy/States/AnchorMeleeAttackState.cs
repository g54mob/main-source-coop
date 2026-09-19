using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorMeleeAttackState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorMeleeAttackState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.MeleeAttack);
			_context.SetVisualState(AnchorVisualState.MeleeAttack);
			_context.SetSmoothedVelocity(0f);
			_context.ClearMeleeHit();
			_context.MeleeCooldown = _context.MeleeCooldownTime;
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.AnchorFaceTargetSystem.Enable();
			_context.AnchorMeleeAttackSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.AnchorFaceTargetSystem.Disable();
			_context.AnchorMeleeAttackSystem.Disable();
			_context.ClearMeleeHit();
		}

		public override void OnLogic()
		{
			if (!_context.HasLivePriorityPlayer() && !_context.HasAuxTarget)
			{
				_enemy.TriggerEvent(AnchorEvent.OnTargetLost);
			}
			else if (_context.AnchorMeleeAttackSystem.HitApplied)
			{
				_enemy.TriggerEvent(AnchorEvent.OnMeleeCompleted);
			}
		}
	}
}
