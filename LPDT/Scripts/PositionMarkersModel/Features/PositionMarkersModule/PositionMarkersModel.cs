using System.Collections.Generic;
using UnityEngine;

namespace Features.PositionMarkersModule
{
	public class PositionMarkersModel
	{
		private Dictionary<PositionName, List<PositionMarker>> _positionMarkers = new Dictionary<PositionName, List<PositionMarker>>();

		public void RegisterPositionMarker(PositionMarker marker)
		{
			if (!_positionMarkers.ContainsKey(marker.positionName))
			{
				_positionMarkers[marker.positionName] = new List<PositionMarker>();
			}
			_positionMarkers[marker.positionName].Add(marker);
		}

		public void UnregisterPositionMarker(PositionMarker marker)
		{
			if (_positionMarkers.ContainsKey(marker.positionName))
			{
				_positionMarkers[marker.positionName].Remove(marker);
			}
		}

		public PositionMarker GetRandomPositionMarker(PositionName positionName)
		{
			if (_positionMarkers.ContainsKey(positionName) && _positionMarkers[positionName].Count > 0)
			{
				List<PositionMarker> list = _positionMarkers[positionName];
				int index = Random.Range(0, list.Count);
				return list[index];
			}
			return null;
		}

		public List<PositionMarker> GetAllPositionMarkers(PositionName positionName)
		{
			if (_positionMarkers.ContainsKey(positionName))
			{
				return _positionMarkers[positionName];
			}
			return new List<PositionMarker>();
		}

		public PositionMarker GetFirstPositionMarker(PositionName positionName)
		{
			if (_positionMarkers.ContainsKey(positionName) && _positionMarkers[positionName].Count > 0)
			{
				return _positionMarkers[positionName][0];
			}
			return null;
		}
	}
}
