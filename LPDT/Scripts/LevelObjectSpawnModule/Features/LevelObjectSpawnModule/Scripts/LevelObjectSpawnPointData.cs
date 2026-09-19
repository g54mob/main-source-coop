using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class LevelObjectSpawnPointData
	{
		public Vector3 Position { get; }

		public Quaternion Rotation { get; }

		public bool UseOriginalRotation { get; }

		public List<LevelObjectType> TargetObjects { get; }

		public bool IsOccupied { get; set; }

		public LevelObjectSpawnPointData(Vector3 position, Quaternion rotation, List<LevelObjectType> targetObjects, bool useOriginalRotation = false)
		{
			Position = position;
			Rotation = rotation;
			TargetObjects = targetObjects;
			UseOriginalRotation = useOriginalRotation;
			IsOccupied = false;
		}
	}
}
