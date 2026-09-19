using System.Collections.Generic;

namespace Features.AIModule.Scripts
{
	public class EnemySpawnPointsModel
	{
		private readonly Dictionary<EnemyType, List<EnemySpawnPointData>> _spawnPointsPool = new Dictionary<EnemyType, List<EnemySpawnPointData>>();

		public IReadOnlyDictionary<EnemyType, List<EnemySpawnPointData>> SpawnPointsPool => _spawnPointsPool;

		public void RegisterSpawnPoint(EnemyType enemyType, EnemySpawnPointData enemySpawnPointData)
		{
			if (!_spawnPointsPool.TryAdd(enemyType, new List<EnemySpawnPointData> { enemySpawnPointData }))
			{
				_spawnPointsPool[enemyType].Add(enemySpawnPointData);
			}
		}

		public void UnRegisterSpawnPoint(EnemyType enemyType, EnemySpawnPointData enemySpawnPointData)
		{
			if (_spawnPointsPool.TryGetValue(enemyType, out var value))
			{
				value.Remove(enemySpawnPointData);
			}
		}

		public void ClearOneTimeSpawnPoints()
		{
			foreach (List<EnemySpawnPointData> value in _spawnPointsPool.Values)
			{
				value.RemoveAll((EnemySpawnPointData spawnPoint) => spawnPoint.IsOneTimeSpawnPoint);
			}
		}
	}
}
