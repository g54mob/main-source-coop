using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.AIModule.Scripts.ManInShadows
{
	public class ManInShadowsPointsModel : ISessionCleanup
	{
		private List<Transform> _points = new List<Transform>();

		public List<Transform> Points => _points;

		public void RegisterPoint(Transform point)
		{
			if (!_points.Contains(point))
			{
				_points.Add(point);
			}
		}

		public void UnregisterPoint(Transform point)
		{
			if (_points.Contains(point))
			{
				_points.Remove(point);
			}
		}

		public void Cleanup()
		{
			_points.Clear();
		}
	}
}
