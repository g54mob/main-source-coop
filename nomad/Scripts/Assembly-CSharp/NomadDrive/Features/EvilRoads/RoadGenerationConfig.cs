using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Road Generation Config", fileName = "RoadGenerationConfig")]
	public class RoadGenerationConfig : SerializedScriptableObject
	{
		public string roadName = "Road";

		public float naturalRoadHeight = 2f;

		public float segmentDistance = 50f;

		public float roadCurveAmplitude = 30f;

		public float roadCurveFrequency = 0.005f;

		public bool usePerlinNoise = true;

		public float poiCurveAmplitude = 20f;

		public bool enablePoiCurve = true;

		[Header("Meander (winding road shape)")]
		[Tooltip("Use the world-Z continuous multi-octave meander instead of the legacy faded per-tile curve. Continuous across tile seams by construction.")]
		public bool useMeander = true;

		[Tooltip("Number of noise octaves. Octave 0 = big sweeping bends, higher octaves = finer wiggle.")]
		[Range(1f, 5f)]
		public int meanderOctaves = 3;

		[Tooltip("Lateral amplitude (metres) of the first octave (the big sweeping bends).")]
		public float meanderBaseAmplitude = 120f;

		[Tooltip("Frequency of the first octave. Lower = longer wavelength bends spanning several tiles.")]
		public float meanderBaseFrequency = 0.0015f;

		[Tooltip("Amplitude multiplier per octave (each finer octave is this fraction of the previous).")]
		[Range(0.1f, 0.9f)]
		public float meanderPersistence = 0.45f;

		[Tooltip("Frequency multiplier per octave (each finer octave bends this many times faster).")]
		[Range(1.5f, 4f)]
		public float meanderLacunarity = 2.3f;

		[Tooltip("Hard clamp on total lateral offset (metres). Also auto-clamped to stay on the tile terrain.")]
		public float maxMeanderAmplitude = 200f;

		[Header("POI Branches (dead-end spurs toward POIs)")]
		[Tooltip("Spawn dead-end spur roads reaching toward off-road POIs.")]
		public bool enableBranches = true;

		[Tooltip("Per-POI deterministic chance (0-1) to create a spur toward it.")]
		[Range(0f, 1f)]
		public float branchToPoiChance = 0.85f;

		[Tooltip("POIs closer than this (metres) to the main road are already road-adjacent; no spur is made.")]
		public float branchMinPoiDistance = 25f;

		[Tooltip("Maximum spur length (metres). POIs farther than this from the road get no spur.")]
		public float branchMaxLength = 220f;

		[Tooltip("The spur dead-ends this many metres beyond the POI's deform radius (at its doorstep, not inside it).")]
		public float branchEndMargin = 4f;

		[Tooltip("Spur starts this many metres OUTSIDE the main-road edge. >0 = offset off the road, 0 = touch the edge, <0 = tuck under.")]
		public float branchEdgeOffset = 0.5f;

		[Tooltip("Length (m) of the straight perpendicular stub leaving the main-road edge so the spur departs at ~90deg. Auto-capped on short spurs.")]
		public float branchConnectBlend = 8f;

		[Tooltip("Embankment height (m) of the branch BODY above terrain (independent of the main road's naturalRoadHeight). Lower = hugs the terrain; 0 = flush; a small value avoids z-fighting. The junction still meets the main-road surface and ramps down to this.")]
		public float branchRoadHeight = 0.25f;
	}
}
