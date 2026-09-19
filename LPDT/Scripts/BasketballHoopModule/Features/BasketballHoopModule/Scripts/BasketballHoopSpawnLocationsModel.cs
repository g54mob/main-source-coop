using UnityEngine;

namespace Features.BasketballHoopModule.Scripts
{
	public sealed class BasketballHoopSpawnLocationsModel
	{
		private Transform _spawnPoint;

		private Transform _distanceTrackPoint;

		private Transform _fallbackTransform;

		public bool IsConfigured => _fallbackTransform != null;

		public Vector3 SpawnPosition => ((_spawnPoint != null) ? _spawnPoint : _fallbackTransform).position;

		public Vector3 DistanceTrackPosition
		{
			get
			{
				if (_distanceTrackPoint != null)
				{
					return _distanceTrackPoint.position;
				}
				return SpawnPosition;
			}
		}

		public void Configure(Transform spawnPoint, Transform distanceTrackPoint, Transform fallbackTransform)
		{
			_spawnPoint = spawnPoint;
			_distanceTrackPoint = distanceTrackPoint;
			_fallbackTransform = fallbackTransform;
		}
	}
}
