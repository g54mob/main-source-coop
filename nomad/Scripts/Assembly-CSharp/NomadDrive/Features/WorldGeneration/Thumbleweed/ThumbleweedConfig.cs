using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.Thumbleweed
{
	[CreateAssetMenu(fileName = "ThumbleweedConfig", menuName = "NomadDrive/WorldGeneration/Thumbleweed Config", order = 0)]
	public class ThumbleweedConfig : ScriptableObject
	{
		[Header("Prefabs")]
		[SerializeField]
		private List<ThumbleweedPrefabVariant> prefabVariants = new List<ThumbleweedPrefabVariant>();

		[Header("Density")]
		[SerializeField]
		private Vector2Int spawnCountPerChunkRange = new Vector2Int(0, 3);

		[Range(0f, 1f)]
		[SerializeField]
		private float spawnChancePerChunk = 0.6f;

		[Min(0f)]
		[SerializeField]
		private int globalMaxActive = 80;

		[Header("Spawn Constraints")]
		[Min(0f)]
		[SerializeField]
		private float spawnHeightEpsilon = 0.2f;

		[Range(0f, 89f)]
		[SerializeField]
		private float maxSlopeAngle = 35f;

		[SerializeField]
		private Vector2 scaleRange = new Vector2(0.6f, 1.1f);

		[Min(0f)]
		[SerializeField]
		private float minDistanceBetweenSpawns = 8f;

		[Header("Wind (per-chunk seeded)")]
		[SerializeField]
		private Vector2 baseWindDirectionXZ = new Vector2(1f, 0f);

		[Range(0f, 180f)]
		[SerializeField]
		private float windDirectionJitterDegrees = 30f;

		[SerializeField]
		private Vector2 windStrengthRange = new Vector2(3f, 7f);

		[SerializeField]
		private Vector2 initialSpinRange = new Vector2(2f, 6f);

		[Header("Physics")]
		[Min(0.01f)]
		[SerializeField]
		private float rigidbodyMass = 0.4f;

		[Min(0f)]
		[SerializeField]
		private float linearDrag = 0.15f;

		[Min(0f)]
		[SerializeField]
		private float angularDrag = 0.4f;

		[Min(0.05f)]
		[SerializeField]
		private float sphereRadius = 0.5f;

		[SerializeField]
		private PhysicsMaterial physicsMaterial;

		[Header("Lifecycle")]
		[Min(1f)]
		[SerializeField]
		private float maxLifetimeSeconds = 90f;

		[Min(1f)]
		[SerializeField]
		private float maxTravelDistance = 200f;

		[Header("Culling")]
		[Min(1f)]
		[SerializeField]
		private float enableDistance = 100f;

		[Min(1f)]
		[SerializeField]
		private float disableDistance = 140f;

		[Min(1f)]
		[SerializeField]
		private float destroyDistance = 220f;

		[Min(0.1f)]
		[SerializeField]
		private float cullCheckInterval = 1f;

		public IReadOnlyList<ThumbleweedPrefabVariant> PrefabVariants => prefabVariants;

		public Vector2Int SpawnCountPerChunkRange => spawnCountPerChunkRange;

		public float SpawnChancePerChunk => spawnChancePerChunk;

		public int GlobalMaxActive => globalMaxActive;

		public float SpawnHeightEpsilon => spawnHeightEpsilon;

		public float MaxSlopeAngle => maxSlopeAngle;

		public Vector2 ScaleRange => scaleRange;

		public float MinDistanceBetweenSpawns => minDistanceBetweenSpawns;

		public Vector2 BaseWindDirectionXZ => baseWindDirectionXZ;

		public float WindDirectionJitterDegrees => windDirectionJitterDegrees;

		public Vector2 WindStrengthRange => windStrengthRange;

		public Vector2 InitialSpinRange => initialSpinRange;

		public float RigidbodyMass => rigidbodyMass;

		public float LinearDrag => linearDrag;

		public float AngularDrag => angularDrag;

		public float SphereRadius => sphereRadius;

		public PhysicsMaterial PhysicsMaterial => physicsMaterial;

		public float MaxLifetimeSeconds => maxLifetimeSeconds;

		public float MaxTravelDistance => maxTravelDistance;

		public float EnableDistance => enableDistance;

		public float DisableDistance => disableDistance;

		public float DestroyDistance => destroyDistance;

		public float CullCheckInterval => cullCheckInterval;

		public bool HasValidVariants()
		{
			if (prefabVariants == null || prefabVariants.Count == 0)
			{
				return false;
			}
			foreach (ThumbleweedPrefabVariant prefabVariant in prefabVariants)
			{
				if (prefabVariant != null && prefabVariant.Prefab != null && prefabVariant.Weight > 0f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
