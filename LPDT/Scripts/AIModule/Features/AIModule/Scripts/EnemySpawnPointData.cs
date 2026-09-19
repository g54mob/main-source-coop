using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemySpawnPointData
	{
		public Vector3 Position { get; private set; }

		public Quaternion Rotation { get; }

		public Vector3 AreaPosition { get; private set; }

		public bool IsOneTimeSpawnPoint { get; }

		public IEnemySpawnPointOccupancy Occupancy { get; }

		public EnemySpawnPointData(Vector3 position, Quaternion rotation, Vector3 areaPosition, bool isOneTimeSpawnPoint = false, IEnemySpawnPointOccupancy occupancy = null)
		{
			Position = position;
			Rotation = rotation;
			AreaPosition = areaPosition;
			IsOneTimeSpawnPoint = isOneTimeSpawnPoint;
			Occupancy = occupancy ?? new LocalEnemySpawnPointOccupancy();
		}

		public void UpdatePose(Vector3 position, Vector3 areaPosition)
		{
			Position = position;
			AreaPosition = areaPosition;
		}
	}
}
