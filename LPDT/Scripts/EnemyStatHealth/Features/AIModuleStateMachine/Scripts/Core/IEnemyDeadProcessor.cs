namespace Features.AIModuleStateMachine.Scripts.Core
{
	public interface IEnemyDeadProcessor
	{
		bool IsNeedToSpawnItem { get; set; }

		void ProcessEnemyDeath();
	}
}
