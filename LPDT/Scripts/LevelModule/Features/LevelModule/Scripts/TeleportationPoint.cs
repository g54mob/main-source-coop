using UnityEngine;

namespace Features.LevelModule.Scripts
{
	public class TeleportationPoint
	{
		public Vector3 Position { get; set; }

		public Quaternion Rotation { get; set; }

		public TeleportationPoint(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}
}
