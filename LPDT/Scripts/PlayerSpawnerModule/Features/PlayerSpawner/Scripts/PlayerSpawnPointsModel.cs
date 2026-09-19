using System.Collections.Generic;

namespace Features.PlayerSpawner.Scripts
{
	public class PlayerSpawnPointsModel
	{
		private readonly List<PlayerSpawnPointData> _points = new List<PlayerSpawnPointData>();

		public IReadOnlyList<PlayerSpawnPointData> Points => _points;

		public void RegisterPoint(PlayerSpawnPointData point)
		{
			_points.Add(point);
		}

		public void UnregisterPoint(PlayerSpawnPointData point)
		{
			_points.Remove(point);
		}
	}
}
