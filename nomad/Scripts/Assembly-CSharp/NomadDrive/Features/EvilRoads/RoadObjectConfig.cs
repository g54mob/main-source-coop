using System.Collections.Generic;
using NomadDrive.Features.EvilRoads.Cable;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Road Object Config", fileName = "RoadObjectConfig")]
	public class RoadObjectConfig : ScriptableObject
	{
		[Header("Object Settings")]
		[Tooltip("The prefab to place along the road")]
		public GameObject roadObject;

		[Header("Placement Settings")]
		[Tooltip("How to distribute objects along the spline")]
		public ObjectDistributionMode distributionMode = ObjectDistributionMode.Repetitive;

		[Tooltip("Position along the spline (0 = start, 1 = end) - Only used in AtSplinePoint mode")]
		[Range(0f, 1f)]
		public float singleObjectPosition = 0.5f;

		[Tooltip("Distance from road center (negative = left, positive = right)")]
		public float distanceToRoadOrigin;

		[Tooltip("Distance between each object along the spline - Only used in Repetitive mode")]
		[Range(0.1f, 100f)]
		public float offsetBetweenOtherObjects = 10f;

		[Header("Positioning Options")]
		[Tooltip("How to place objects relative to the road")]
		public RoadObjectPlacementMode placementMode;

		[Tooltip("Symmetric only: rotate the object on the opposite side of the road 180° so each side faces its own lane's traffic (e.g. signs readable from both driving directions).")]
		public bool flipRotationOnOppositeSide;

		[Tooltip("Symmetric only: shift the opposite-side object this many metres ALONG the road so the two sides aren't perfectly aligned (staggered). 0 = perfectly aligned.")]
		public float symmetricAlongRoadOffset;

		[Tooltip("How to rotate objects")]
		public RoadObjectRotationMode rotationMode;

		[Tooltip("Custom rotation angle in degrees (0-360) - Only used in CustomRotation mode")]
		[Range(0f, 360f)]
		public float customRotationAngle;

		[Tooltip("How to handle object height positioning")]
		public RoadObjectSnappingMode snappingMode;

		[Tooltip("Custom height offset from terrain (0-10m) - Only used in Custom snapping mode")]
		[Range(0f, 10f)]
		public float customHeight;

		[Header("Surface Alignment")]
		[Tooltip("Align object rotation to surface normal (terrain slope/road banking) when using Snap modes")]
		public bool alignToSurfaceNormal = true;

		[Header("Cable Connection")]
		[Tooltip("Enable cable/rope connections between consecutive objects of this type")]
		public bool enableCableConnections;

		[Tooltip("Cable connection configuration. Required if enableCableConnections is true.")]
		public CableConnectionConfig cableConfig;

		[Header("Random Scattered (Signs)")]
		[Tooltip("Sign prefab variants chosen at random points. Used ONLY in RandomScattered distribution mode (replaces the single Road Object prefab).")]
		public List<RoadObjectVariant> randomVariants = new List<RoadObjectVariant>();

		[Tooltip("Minimum gap (metres) between consecutive scattered signs. Used only in RandomScattered mode.")]
		[Min(0.1f)]
		public float minSpacing = 25f;

		[Tooltip("Maximum gap (metres) between consecutive scattered signs. Used only in RandomScattered mode.")]
		[Min(0.1f)]
		public float maxSpacing = 80f;

		[Tooltip("A picked variant cannot be re-picked until this many other signs were placed (anti-repetition). Clamped to (variants - 1) at runtime.")]
		[Min(0f)]
		public int noRepeatWindow = 1;

		[Tooltip("Skip this many metres at each road end so signs don't crowd boundaries. Used only in RandomScattered mode.")]
		[Min(0f)]
		public float startMargin = 10f;

		[Tooltip("Salt mixed into the deterministic per-road seed. Give multiple scattered sign configs on the same road different salts so they don't overlap identically.")]
		public int placementSeedSalt;
	}
}
