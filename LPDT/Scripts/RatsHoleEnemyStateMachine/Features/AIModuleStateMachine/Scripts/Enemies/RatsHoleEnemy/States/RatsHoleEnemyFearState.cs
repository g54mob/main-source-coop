using Features.AIModuleStateMachine.Scripts.Core.Settings;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyFearState : RatsHoleEnemyHoleAbsorbDespawnStateBase
	{
		public RatsHoleEnemyFearState(RatsHoleEnemy enemy, RatsHoleEnemyContext context, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings)
			: base(enemy, context, fearHoleAbsorbAnimationSettings, RatsHoleEnemyStateId.Fear, RatsHoleEnemyVisualState.Fear)
		{
		}

		protected override bool TryPrepareDestination()
		{
			return base.Enemy.TryPrepareNearestHoleAbsorbDestination();
		}
	}
}
