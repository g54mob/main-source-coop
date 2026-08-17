using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public struct LootPlacementResult
	{
		public bool Success;

		public Vector3 FinalPosition;

		public Quaternion FinalRotation;

		public bool UsedFallback;

		public static LootPlacementResult Failed => new LootPlacementResult
		{
			Success = false
		};
	}
}
