namespace Features.AIModule.Scripts
{
	public class EnemySpawnConfigurationProvider : IEnemySpawnConfigurationProvider
	{
		private EnemySpawnConfigurationsHolder _enemySpawnConfigurationsHolder;

		private EnemySpawnConfigurationModel _enemySpawnConfigurationModel;

		public EnemySpawnConfigurationProvider(EnemySpawnConfigurationsHolder enemySpawnConfigurationsHolder, EnemySpawnConfigurationModel enemySpawnConfigurationModel)
		{
			_enemySpawnConfigurationModel = enemySpawnConfigurationModel;
			_enemySpawnConfigurationsHolder = enemySpawnConfigurationsHolder;
		}

		public EnemySpawnConfiguration GetEnemySpawnConfiguration()
		{
			return _enemySpawnConfigurationsHolder.GetEnemySpawnConfiguration(_enemySpawnConfigurationModel.CurrentSpawnConfiguration);
		}
	}
}
