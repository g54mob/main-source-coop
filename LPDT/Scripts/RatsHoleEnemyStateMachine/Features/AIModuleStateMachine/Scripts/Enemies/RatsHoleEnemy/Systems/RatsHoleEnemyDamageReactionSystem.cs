using Features.AIModuleStateMachine.Scripts.Core.Systems.Damage;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class RatsHoleEnemyDamageReactionSystem : EnemyDamageReactionSystemBase
	{
		private RatsHoleEnemy _enemy;

		private RatsHoleEnemyContext _context;

		[Inject]
		public void InjectDependencies(RatsHoleEnemy enemy, RatsHoleEnemyContext context)
		{
			_enemy = enemy;
			_context = context;
		}

		protected override void React(PlayerDataHolder attacker, DamageData damageData)
		{
			_context.SetPriorityPlayer(attacker);
			_enemy.TriggerEvent(RatsHoleEnemyEvent.OnDamaged);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
