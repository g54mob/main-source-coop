using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public abstract class SharkBiteAttackStateBase : StateBase<SharkStateId>
	{
		protected readonly SharkEnemy _enemy;

		protected readonly SharkEnemyContext _context;

		protected readonly SharkAttackSettings _attack;

		protected abstract SharkVisualState Visual { get; }

		protected abstract float Damage { get; }

		protected abstract bool ApplyKnock { get; }

		protected SharkBiteAttackStateBase(SharkEnemy enemy, SharkEnemyContext context, SharkAttackSettings attack)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_attack = attack;
		}

		public override void OnEnter()
		{
			if (!_enemy.CanEnterPlayerBite())
			{
				_enemy.LosePlayerTarget();
				return;
			}
			_context.AttackStateElapsed = 0f;
			_context.AttackDamageApplied = false;
			_context.AttackExitSent = false;
			_enemy.SetVisualState(Visual);
			_enemy.RaiseAttackStartSound();
		}

		protected abstract SharkStateId ResolveAttackStateId();

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.AttackStateElapsed += tickDelta;
			if (!_context.AttackDamageApplied && _context.AttackStateElapsed >= _attack.AttackWindupDuration)
			{
				_context.AttackDamageApplied = true;
				_enemy.ApplyBiteDamage(Damage, ApplyKnock);
			}
			if (!_context.AttackExitSent && _context.AttackStateElapsed >= _attack.AttackWindupDuration + _attack.AttackActiveDuration)
			{
				_context.AttackExitSent = true;
				_enemy.TriggerEvent(SharkEvent.OnAttackFinished);
			}
		}
	}
}
