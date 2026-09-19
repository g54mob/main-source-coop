using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.NavigationModule.Scripts
{
	public class PlayerRaycastPointsModel
	{
		private readonly Dictionary<PlayerRef, PlayerRaycastPointsData> _raycastPoints = new Dictionary<PlayerRef, PlayerRaycastPointsData>();

		public IReadOnlyDictionary<PlayerRef, PlayerRaycastPointsData> RaycastPoints => _raycastPoints;

		public void RegisterRaycastPoint(PlayerRef owner, PlayerRaycastPoint point, Transform pointTransform)
		{
			if (!_raycastPoints.ContainsKey(owner))
			{
				_raycastPoints[owner] = new PlayerRaycastPointsData();
			}
			_raycastPoints[owner].RaycastPoints[point] = pointTransform;
		}

		public void UnregisterRaycastPoint(PlayerRef owner, PlayerRaycastPoint point)
		{
			if (_raycastPoints.TryGetValue(owner, out var value))
			{
				value.RaycastPoints.Remove(point);
				if (value.RaycastPoints.Count == 0)
				{
					_raycastPoints.Remove(owner);
				}
			}
		}

		public void UnregisterPlayer(PlayerRef owner)
		{
			_raycastPoints.Remove(owner);
		}

		public bool TryGetRaycastPoint(PlayerRef owner, PlayerRaycastPoint targetPoint, out Transform point)
		{
			point = null;
			if (!_raycastPoints.TryGetValue(owner, out var value))
			{
				return false;
			}
			if (value.RaycastPoints.Count == 0)
			{
				return false;
			}
			if (value.RaycastPoints.TryGetValue(targetPoint, out var value2) && value2 != null)
			{
				point = value2;
				return true;
			}
			foreach (KeyValuePair<PlayerRaycastPoint, Transform> raycastPoint in value.RaycastPoints)
			{
				if (raycastPoint.Value != null)
				{
					point = raycastPoint.Value;
					return true;
				}
			}
			return false;
		}
	}
}
