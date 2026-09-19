using Features.LevelModule.Scripts;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public interface ILevelObjectsSpawnService
	{
		void SpawnItemsByLevel(LevelType targetLevel);

		void SpawnItemAtPoint(LevelObjectSpawnPointData spawnPoint, LevelObjectType objectType, LevelType targetLevel);

		void PrepareSpawnItems(LevelType targetLevel);

		void SpawnPreparedItems();

		void InvalidatePreparedSpawns();
	}
}
