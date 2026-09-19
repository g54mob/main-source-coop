using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkAttackLethalState : SharkBiteAttackStateBase
	{
		protected override SharkVisualState Visual => SharkVisualState.BiteLethal;

		protected override float Damage => _attack.LethalDamage;

		protected override bool ApplyKnock => true;

		public SharkAttackLethalState(SharkEnemy enemy, SharkEnemyContext context, SharkAttackSettings attack)
			: base(enemy, context, attack)
		{
		}

		protected override SharkStateId ResolveAttackStateId()
		{
			return SharkStateId.AttackLethal;
		}
	}
}
