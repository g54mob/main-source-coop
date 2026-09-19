using Features.AIModuleStateMachine.Scripts.Core.Settings;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyReturnToHoleDespawnState : RatsHoleEnemyHoleAbsorbDespawnStateBase
	{
		public RatsHoleEnemyReturnToHoleDespawnState(RatsHoleEnemy enemy, RatsHoleEnemyContext context, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings)
			: base(enemy, context, fearHoleAbsorbAnimationSettings, RatsHoleEnemyStateId.ReturnToHoleDespawn, RatsHoleEnemyVisualState.Wander)
		{
		}

		protected override bool TryPrepareDestination()
		{
			return base.Enemy.TryPrepareHomeThenNearestHoleAbsorbDestination();
		}

		protected override bool TryInterruptForReacquire()
		{
			return base.Enemy.TryReacquireAvailableTarget();
		}
	}
}
