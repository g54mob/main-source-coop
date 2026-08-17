using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	[Serializable]
	public class SpawnedPoiRecord
	{
		public Vector2Int ChunkCoord;

		public string PoiAssetGuid;

		public POICategory Category;

		public Vector2 Position;

		public Vector3 RotationEuler;

		public bool VegetationClearing;

		public TerrainDeformMode DeformMode;

		public float DeformRadius;

		public float DeformFalloff;

		public float TerrainHeightOffset;

		public float FoundationYOffset;

		public float SpawnTime;

		public bool GenerateBranchRoad;

		public bool PlaceNearRoad;
	}
}
