using System.Collections.Generic;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class LevelSpawnPointsModel
	{
		private readonly List<LevelObjectSpawnPointData> _levelSpawnPointsPool = new List<LevelObjectSpawnPointData>();

		public IReadOnlyList<LevelObjectSpawnPointData> LevelSpawnPointsPool => _levelSpawnPointsPool;

		public void RegisterSpawnPoint(LevelObjectSpawnPointData enemySpawnPointData)
		{
			_levelSpawnPointsPool.Add(enemySpawnPointData);
		}

		public void UnRegisterSpawnPoint(LevelObjectSpawnPointData enemySpawnPointData)
		{
			_levelSpawnPointsPool.Remove(enemySpawnPointData);
		}

		public int Clear()
		{
			int count = _levelSpawnPointsPool.Count;
			_levelSpawnPointsPool.Clear();
			return count;
		}
	}
}
