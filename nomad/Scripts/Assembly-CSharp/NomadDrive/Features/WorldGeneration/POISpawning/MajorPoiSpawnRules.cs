using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	[CreateAssetMenu(fileName = "MajorPOISpawnRules", menuName = "NomadDrive/World Generation/Major Spawn Rules", order = 2)]
	public class MajorPoiSpawnRules : ScriptableObject
	{
		[Header("POI Entries")]
		[Tooltip("Weighted major POI pool. Weight = chance-to-be-seen / draw-order bias.")]
		[FormerlySerializedAs("poiTypes")]
		public PoiWeightEntry[] poiEntries = new PoiWeightEntry[0];

		[Header("Origin-Only POIs")]
		[Tooltip("POIs that only spawn at the origin chunk (0,0), e.g., StarterHouse. Excluded from both arms' sequences. Weight is ignored.")]
		public PoiWeightEntry[] originOnlyPois = new PoiWeightEntry[0];

		[Header("Origin-Only Positioning")]
		[Tooltip("Minimum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float originMinOffsetX = 30f;

		[Tooltip("Maximum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float originMaxOffsetX = 60f;

		[Tooltip("Minimum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float originMinOffsetZ = 30f;

		[Tooltip("Maximum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float originMaxOffsetZ = 60f;

		[Tooltip("Allowed road sides for origin POI spawning")]
		public RoadSide originAllowedRoadSides = RoadSide.Left;

		[Header("Origin-Only Rotation")]
		[Tooltip("Minimum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float originMinRotationY;

		[Tooltip("Maximum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float originMaxRotationY;

		[Header("Road Avoidance")]
		[Tooltip("Minimum clearance from road edge in meters")]
		[Range(0f, 30f)]
		public float minRoadClearance = 8f;

		[Header("Positioning")]
		[Tooltip("Minimum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float minOffsetX = 20f;

		[Tooltip("Maximum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float maxOffsetX = 80f;

		[Tooltip("Minimum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float minOffsetZ = 100f;

		[Tooltip("Maximum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float maxOffsetZ = 200f;

		[Header("Rotation")]
		[Tooltip("Minimum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float minRotationY = -90f;

		[Tooltip("Maximum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float maxRotationY = 90f;
	}
}
