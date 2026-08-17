using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public struct PoiSpawnPlan
	{
		public Vector2Int ChunkCoord;

		public string PoiAssetGuid;

		public POICategory Category;

		public Vector2 SpawnXZ;

		public Vector3 RotationEuler;

		public TerrainDeformMode DeformMode;

		public float DeformRadius;

		public float DeformFalloff;

		public float TerrainHeightOffset;

		public float FoundationYOffset;

		public bool VegetationCleaning;

		public bool GenerateBranchRoad;

		public bool PlaceNearRoad;
	}
}
