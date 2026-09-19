namespace Features.AIModuleStateMachine.Scripts.Services
{
	public interface IEnemyPlayerAttackabilityService
	{
		bool CanEnemyTargetPlayer(int playerId);

		bool CanEnemyAttackPlayer(int playerId);
	}
}
