namespace Features.AIModule.Scripts
{
	public static class EnemySpawnPointOccupancyUtility
	{
		public static bool TryOccupy(EnemySpawnPointData spawnPoint, IEnemyBehaviour enemy)
		{
			if (spawnPoint?.Occupancy == null || enemy == null || !enemy.IsOccupySpawnPoint)
			{
				return false;
			}
			if (enemy.NetworkObject == null || !enemy.NetworkObject.IsValid)
			{
				return false;
			}
			if (!spawnPoint.Occupancy.TryOccupy(enemy.NetworkObject))
			{
				return false;
			}
			enemy.BindSpawnPointOccupancy(spawnPoint.Occupancy);
			return true;
		}
	}
}
