using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	[Serializable]
	public class PoiWeightEntry
	{
		[Tooltip("Direct reference to POI prefab (GUID-based, survives rename/move)")]
		public AssetReferenceGameObject poiPrefab;

		[Tooltip("Chance to be seen / draw-order bias. Higher weight => this POI tends to appear earlier within each shuffle-bag cycle. Every POI in the pool still appears exactly once per cycle before any repeats.")]
		[Range(0f, 100f)]
		public float weight = 10f;

		[Header("Terrain Settings")]
		[Tooltip("Terrain deformation mode for this POI")]
		public TerrainDeformMode terrainDeformMode = TerrainDeformMode.RaiseToMax;

		[Tooltip("Terrain deformation radius in meters")]
		[Range(5f, 100f)]
		public float terrainDeformRadius = 10f;

		[Tooltip("Terrain deformation falloff (smooth edges)")]
		[Range(0f, 1f)]
		public float terrainDeformFalloff = 0.3f;

		[Tooltip("Raises terrain above POI base to prevent z-fighting. Value in meters.")]
		[Range(0f, 0.5f)]
		public float terrainHeightOffset = 0.05f;

		[Tooltip("Raises POI above the foundation platform to prevent terrain clipping. Only used in Foundation mode. Value in meters.")]
		[Range(0f, 1f)]
		public float foundationYOffset = 0.1f;

		[Tooltip("Clear vegetation before spawning")]
		public bool vegetationCleaning = true;

		[Header("Road Branch")]
		[Tooltip("Build a dead-end spur road toward this POI (subject to RoadGenerationConfig branch settings). Turn off for off-road landmark POIs.")]
		public bool generateBranchRoad = true;

		[Header("Road Placement")]
		[Tooltip("Place this POI at the road edge instead of using the random lateral offset + road clearance. Default off keeps the normal off-road placement.")]
		public bool placeNearRoad;

		[Tooltip("Gap (meters) between the road edge and the POI origin when Place Near Road is on.")]
		[Range(0f, 30f)]
		public float roadEdgeDistance = 5f;

		[Tooltip("Rotate this POI so its front faces the road. Default off keeps the random rotation.")]
		public bool alignToRoad;

		[Tooltip("Extra yaw (degrees) added on top of the road-facing rotation to correct the prefab's forward axis.")]
		[Range(-180f, 180f)]
		public float roadAlignmentYawOffset;

		public bool HasValidReference
		{
			get
			{
				if (poiPrefab != null)
				{
					return !string.IsNullOrEmpty(poiPrefab.AssetGUID);
				}
				return false;
			}
		}

		public string AssetGuid => poiPrefab?.AssetGUID;
	}
}
