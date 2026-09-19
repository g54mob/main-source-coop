using System.Collections.Generic;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data
{
	public class MonkeyPorterSpawnPointsModel
	{
		private readonly List<MonkeyPorterSpawnPointData> _points = new List<MonkeyPorterSpawnPointData>();

		public IReadOnlyList<MonkeyPorterSpawnPointData> Points => _points;

		public void RegisterPoint(MonkeyPorterSpawnPointData point)
		{
			_points.Add(point);
		}

		public void UnregisterPoint(MonkeyPorterSpawnPointData point)
		{
			_points.Remove(point);
		}
	}
}
