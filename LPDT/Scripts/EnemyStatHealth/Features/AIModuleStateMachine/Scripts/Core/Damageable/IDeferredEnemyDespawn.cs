namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	public interface IDeferredEnemyDespawn
	{
		bool TryStartDeferredDespawn(EnemyDissolveReason reason = EnemyDissolveReason.Death);
	}
}
