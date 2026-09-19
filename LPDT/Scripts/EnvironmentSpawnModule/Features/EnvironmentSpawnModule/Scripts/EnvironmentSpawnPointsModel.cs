using System.Collections.Generic;

namespace Features.EnvironmentSpawnModule.Scripts
{
	public class EnvironmentSpawnPointsModel
	{
		private readonly List<EnvironmentSpawnPointData> _points = new List<EnvironmentSpawnPointData>();

		public IReadOnlyList<EnvironmentSpawnPointData> Points => _points;

		public void RegisterPoint(EnvironmentSpawnPointData point)
		{
			_points.Add(point);
		}

		public void UnregisterPoint(EnvironmentSpawnPointData point)
		{
			_points.Remove(point);
		}
	}
}
