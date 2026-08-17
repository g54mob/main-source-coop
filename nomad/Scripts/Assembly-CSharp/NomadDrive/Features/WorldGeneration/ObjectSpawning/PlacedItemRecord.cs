using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public readonly struct PlacedItemRecord
	{
		public readonly Vector3 Center;

		public readonly Vector3 HalfExtents;

		public PlacedItemRecord(Vector3 center, Vector3 halfExtents)
		{
			Center = center;
			HalfExtents = halfExtents;
		}
	}
}
