using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkAttackSimpleState : SharkBiteAttackStateBase
	{
		protected override SharkVisualState Visual => SharkVisualState.BiteSimple;

		protected override float Damage => _attack.SimpleDamage;

		protected override bool ApplyKnock => true;

		public SharkAttackSimpleState(SharkEnemy enemy, SharkEnemyContext context, SharkAttackSettings attack)
			: base(enemy, context, attack)
		{
		}

		protected override SharkStateId ResolveAttackStateId()
		{
			return SharkStateId.AttackSimple;
		}
	}
}
