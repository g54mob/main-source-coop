using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Road Config", fileName = "RoadConfig")]
	public class EvilRoadConfig : ScriptableObject
	{
		[Header("Road Settings")]
		public float roadWidth = 10f;

		public float textureTileLength = 10f;

		public Material roadMaterial;

		[Tooltip("Unity tag applied to spawned road mesh GameObjects. Leave empty to skip tagging.")]
		public string roadTag;

		[Header("Mesh Optimization")]
		[Tooltip("Mesh detail intensity - higher = smoother road edges. 4 = ~1 segment/2m, 8 = ~1 segment/m.")]
		[Range(1f, 10f)]
		public float meshIntensity = 6f;

		[Tooltip("Enable adaptive mesh density based on road curvature (curvy roads get up to 2x density)")]
		public bool useAdaptiveMeshDensity = true;

		[Tooltip("Maximum mesh segments per road. Raise for smoother long roads (deform runs on Burst worker threads).")]
		[Range(50f, 1000f)]
		public int maxMeshSegments = 400;

		[Header("Road Shape Polish (optional, OFF by default)")]
		[Tooltip("Vary road width along its length for a more natural, less uniform look.")]
		public bool enableWidthVariation;

		[Tooltip("Fractional width variation (0 = none, 0.3 = +/-30%).")]
		[Range(0f, 0.5f)]
		public float widthVariationAmount = 0.15f;

		[Tooltip("Spatial frequency of the width variation along the road.")]
		public float widthVariationFrequency = 0.05f;

		[Tooltip("EXPERIMENTAL: bank (tilt) the road into curves. Affects the driving surface; tune carefully.")]
		public bool enableBanking;

		[Tooltip("Maximum banking edge height offset (metres) on the sharpest curves.")]
		[Range(0f, 3f)]
		public float bankingStrength = 0.6f;

		[Header("Terrain Deformation Settings")]
		[Tooltip("How far from the road edge to affect terrain (beyond road width)")]
		[Range(2f, 15f)]
		public float terrainInfluenceDistance = 8f;

		[Tooltip("Strength of terrain deformation in influence zone (0 = no effect, 1 = full effect)")]
		[Range(0.1f, 1f)]
		public float deformationStrength = 0.7f;

		[Tooltip("Smoothness of terrain blending from road to original terrain (higher = more gradual)")]
		[Range(0.5f, 4f)]
		public float blendingSmoothness = 1.5f;

		[Tooltip("Number of iterations for terrain smoothing")]
		[Range(1f, 5f)]
		public int smoothingIterations = 2;

		[Header("Precision Settings")]
		[Tooltip("Road surface elevation offset to prevent floating/clipping")]
		[Range(-0.2f, 0.2f)]
		public float roadSurfaceAdjustment = 0.01f;

		[Tooltip("Enable ultra-precise road fitting (slower but more accurate)")]
		public bool enableUltraPrecision = true;

		[Header("Smart Fill/Cut System")]
		[Tooltip("Enable smart terrain fill/cut within road width for natural integration")]
		public bool enableSmartFillCut = true;

		[Tooltip("Strength of fill/cut adjustment (higher = stronger adjustment to road level)")]
		[Range(0.3f, 1f)]
		public float cuttingStrength = 0.7f;

		[Tooltip("Number of iterative passes for terrain cleanup (higher = cleaner result, slower)")]
		[Range(1f, 5f)]
		public int terrainCleanupIterations = 3;

		[Tooltip("Enable road edge cleanup to prevent terrain bleeding at edges")]
		public bool enableEdgeCleanup = true;

		[Tooltip("Sensitivity for detecting road edges (lower = more sensitive)")]
		[Range(0.05f, 0.3f)]
		public float edgeDetectionSensitivity = 0.1f;

		[Header("Connection Settings")]
		[Tooltip("Maximum distance for road connection points to manually connect")]
		[Range(1f, 20f)]
		public float connectionDistance = 5f;

		[Header("Lazy Loading Settings")]
		[Tooltip("Enable lazy loading optimization for distant roads")]
		public bool enableLazyLoading = true;

		[Tooltip("Tag of the reference object for distance calculation (e.g., 'Player', 'Vehicle')")]
		public string lazyLoadingReferenceTag = "Player";

		[Tooltip("Distance from reference object beyond which roads are created in simplified mode (no terrain deformation, in meters)")]
		[Range(100f, 2000f)]
		public float lazyLoadingDistance = 500f;

		[Header("Dynamic Road Upgrade Settings")]
		[Tooltip("Enable dynamic upgrade of simplified roads when reference object gets close")]
		public bool enableDynamicUpgrade = true;

		[Tooltip("How often to check for road upgrades (in seconds)")]
		[Range(0.5f, 10f)]
		public float upgradeCheckInterval = 2f;

		[Tooltip("Upgrade simplified roads when reference object is within this percentage of lazyLoadingDistance (0.8 = 80%)")]
		[Range(0.5f, 1f)]
		public float upgradeDistanceThreshold = 0.8f;
	}
}
