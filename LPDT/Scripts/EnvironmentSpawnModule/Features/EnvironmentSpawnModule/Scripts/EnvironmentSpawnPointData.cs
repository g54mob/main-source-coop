using UnityEngine;

namespace Features.EnvironmentSpawnModule.Scripts
{
	public class EnvironmentSpawnPointData
	{
		public Vector3 Position { get; }

		public Quaternion Rotation { get; }

		public GameObject Prefab { get; }

		public int Count { get; }

		public bool UseOriginalRotation { get; }

		public EnvironmentSpawnPointData(Vector3 position, Quaternion rotation, GameObject prefab, int count, bool useOriginalRotation = false)
		{
			Count = count;
			Position = position;
			Rotation = rotation;
			Prefab = prefab;
			UseOriginalRotation = useOriginalRotation;
		}
	}
}
