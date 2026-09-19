using System.Collections.Generic;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleSpawnPointsModel
	{
		private readonly List<RatsHoleSpawnPointData> _spawnPoints = new List<RatsHoleSpawnPointData>();

		public IReadOnlyList<RatsHoleSpawnPointData> SpawnPoints => _spawnPoints;

		public bool AreSpawned { get; set; }

		public void Register(RatsHoleSpawnPointData spawnPoint)
		{
			if (spawnPoint != null && !_spawnPoints.Contains(spawnPoint))
			{
				_spawnPoints.Add(spawnPoint);
			}
		}

		public void Unregister(RatsHoleSpawnPointData spawnPoint)
		{
			if (spawnPoint != null)
			{
				_spawnPoints.Remove(spawnPoint);
				if (_spawnPoints.Count == 0)
				{
					AreSpawned = false;
				}
			}
		}
	}
}
