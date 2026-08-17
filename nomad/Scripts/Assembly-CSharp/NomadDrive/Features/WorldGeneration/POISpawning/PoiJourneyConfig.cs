using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	[CreateAssetMenu(fileName = "POIJourneyConfig", menuName = "NomadDrive/World Generation/Journey Config", order = 0)]
	public class PoiJourneyConfig : ScriptableObject
	{
		[Header("Balance - Majors vs Minors")]
		[Tooltip("Minimum guaranteed minor POIs between two consecutive major POIs.")]
		[Range(0f, 10f)]
		public int minMinorsBetweenMajors = 1;

		[Tooltip("Maximum minor POIs between two consecutive major POIs.")]
		[Range(0f, 10f)]
		public int maxMinorsBetweenMajors = 2;

		[Header("Spacing")]
		[Tooltip("Minimum empty chunks between two consecutive POIs.")]
		[Range(1f, 20f)]
		public int minChunksBetweenPois = 1;

		[Tooltip("Maximum empty chunks between two consecutive POIs.")]
		[Range(1f, 40f)]
		public int maxChunksBetweenPois = 3;

		[Tooltip("Extra empty chunks added right after a major POI (breathing room).")]
		[Range(0f, 10f)]
		public int forcedEmptyChunksAfterMajor = 2;

		[Header("Journey Start")]
		[Tooltip("Minimum chunks from the origin before the first POI in each direction.")]
		[Range(1f, 20f)]
		public int minFirstOffsetChunks = 1;

		[Tooltip("Maximum chunks from the origin before the first POI in each direction.")]
		[Range(1f, 40f)]
		public int maxFirstOffsetChunks = 3;

		[FormerlySerializedAs("tinyPOIRules")]
		[Header("Spawn Rules")]
		[Tooltip("Minor POI pool + placement settings")]
		public MinorPoiSpawnRules minorPoiRules;

		[FormerlySerializedAs("majorPOIRules")]
		[Tooltip("Major POI pool + placement settings")]
		public MajorPoiSpawnRules majorPoiRules;

		[Header("Debug")]
		[Tooltip("Enable detailed debug logging")]
		public bool enableDebugLogs = true;

		private void OnValidate()
		{
			if (maxMinorsBetweenMajors < minMinorsBetweenMajors)
			{
				maxMinorsBetweenMajors = minMinorsBetweenMajors;
			}
			if (maxChunksBetweenPois < minChunksBetweenPois)
			{
				maxChunksBetweenPois = minChunksBetweenPois;
			}
			if (maxFirstOffsetChunks < minFirstOffsetChunks)
			{
				maxFirstOffsetChunks = minFirstOffsetChunks;
			}
			_ = minorPoiRules == null;
			_ = majorPoiRules == null;
		}
	}
}
