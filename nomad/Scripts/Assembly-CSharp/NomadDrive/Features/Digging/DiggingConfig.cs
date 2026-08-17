using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Digging
{
	[CreateAssetMenu(menuName = "NomadDrive/Digging/Digging Config", fileName = "DiggingConfig")]
	public class DiggingConfig : ScriptableObject
	{
		public const string ResourcePath = "DiggingConfig";

		private static DiggingConfig _instance;

		[Header("Sand Decal (shared)")]
		[Tooltip("SandDecals prefab (sand/rock only, Step-01..04). Used for both buried-treasure decals and no-treasure dig marks.")]
		public GameObject sandDecalPrefab;

		[Tooltip("Lifts the decal along the terrain normal so it rests on top of the ground instead of sinking in.")]
		public float decalSurfaceOffset = 0.05f;

		[Tooltip("Seconds before a no-treasure dig mark is destroyed. 0 = permanent for the session.")]
		[Min(0f)]
		public float markLifetimeSeconds = 120f;

		[Tooltip("Give NO-TREASURE dig marks a random yaw around the surface normal so they don't look uniform. Buried-treasure decals always use the prefab's original authored rotation (never randomized).")]
		public bool randomizeDecalYaw = true;

		[Header("Terrain / Surface Detection (shared)")]
		[Tooltip("Terrain layer(s): used by the shovel aim ray and by the server surface probe for buried treasures.")]
		public LayerMask terrainMask;

		[Tooltip("Solid layers that BLOCK a dig when they sit between the camera and the terrain point (building floors/walls/structures/props). Must NOT include the Terrain layer, the Player, or triggers. Stops digging through a floor into the terrain below.")]
		public LayerMask digObstructionMask;

		[Tooltip("How far above a buried treasure the server probes (downward) to find the terrain surface.")]
		[Min(1f)]
		public float surfaceProbeHeight = 50f;

		[Tooltip("Used only if the surface probe finds no terrain: rise the treasure root by this height instead.")]
		[Min(0f)]
		public float fallbackRiseHeight = 1.5f;

		[Header("Rise (shared)")]
		[Min(0.05f)]
		public float riseDuration = 0.6f;

		public Ease riseEase = Ease.OutCubic;

		public static DiggingConfig Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<DiggingConfig>("DiggingConfig");
				}
				return _instance;
			}
		}
	}
}
