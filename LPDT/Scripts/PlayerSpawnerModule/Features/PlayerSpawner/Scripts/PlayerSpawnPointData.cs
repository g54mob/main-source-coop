using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	public class PlayerSpawnPointData
	{
		public Vector3 Position { get; }

		public Quaternion Rotation { get; }

		public int Priority { get; }

		public PlayerSpawnPointData(Vector3 position, Quaternion rotation, int priority)
		{
			Position = position;
			Rotation = rotation;
			Priority = priority;
		}
	}
}
