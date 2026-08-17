using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	[CreateAssetMenu(fileName = "MinorPOISpawnRules", menuName = "NomadDrive/World Generation/Minor Spawn Rules", order = 1)]
	public class MinorPoiSpawnRules : ScriptableObject
	{
		[Header("POI Entries")]
		[Tooltip("Weighted minor POI pool. Weight = chance-to-be-seen / draw-order bias.")]
		[FormerlySerializedAs("poiTypes")]
		public PoiWeightEntry[] poiEntries = new PoiWeightEntry[0];

		[Header("Positioning")]
		[Tooltip("Minimum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float minOffsetX = 10f;

		[Tooltip("Maximum offset from chunk center (X axis, meters)")]
		[Range(-128f, 128f)]
		public float maxOffsetX = 80f;

		[Tooltip("Minimum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float minOffsetZ = 50f;

		[Tooltip("Maximum offset from chunk center (Z axis, meters)")]
		[Range(-128f, 128f)]
		public float maxOffsetZ = 200f;

		[Tooltip("Allowed road sides for spawning")]
		public RoadSide allowedRoadSides = RoadSide.Both;

		[Header("Road Avoidance")]
		[Tooltip("Minimum clearance from road edge in meters")]
		[Range(0f, 30f)]
		public float minRoadClearance = 5f;

		[Header("Rotation")]
		[Tooltip("Minimum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float minRotationY = -45f;

		[Tooltip("Maximum Y rotation (degrees)")]
		[Range(-180f, 180f)]
		public float maxRotationY = 45f;
	}
}
