using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class BeachInteractableSpawnData
	{
		public Vector3 Position { get; set; }

		public Quaternion Rotation { get; set; }

		public List<BeachInteractableType> AllowedInteractables { get; set; }

		public int Marker { get; set; }

		public BeachInteractableSpawnData(Vector3 position, Quaternion rotation, List<BeachInteractableType> allowedInteractables, int marker)
		{
			Position = position;
			Rotation = rotation;
			AllowedInteractables = allowedInteractables;
			Marker = marker;
		}
	}
}
