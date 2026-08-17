using System;
using UnityEngine;

namespace EvilCore.DynamicCasting
{
	[Serializable]
	public struct CastRequest
	{
		public Transform Origin;

		public Vector3 Offset;

		public bool UseTransformForward;

		public Vector3 Direction;

		public CastType Type;

		public float Distance;

		public LayerMask LayerMask;

		public QueryTriggerInteraction TriggerInteraction;

		public float Radius;

		public Vector3 HalfExtents;

		public float CapsuleHeight;

		public UpdateMode UpdateMode;

		public int MaxHits;

		public Type ComponentFilter;

		public bool searchInParent;

		public bool UseRegisteredMeshes;

		public static CastRequest Ray(Transform origin, float distance, LayerMask layer, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore)
		{
			return new CastRequest
			{
				Origin = origin,
				UseTransformForward = true,
				Type = CastType.Ray,
				Distance = distance,
				LayerMask = layer,
				TriggerInteraction = triggerInteraction,
				UpdateMode = UpdateMode.Update,
				MaxHits = 8
			};
		}

		public static CastRequest Ray(Transform origin, Vector3 offset, float distance, LayerMask layer)
		{
			CastRequest result = Ray(origin, distance, layer);
			result.Offset = offset;
			return result;
		}

		public static CastRequest Ray(Transform origin, Vector3 direction, float distance, LayerMask layer, bool useTransformForward)
		{
			CastRequest result = Ray(origin, distance, layer);
			result.UseTransformForward = useTransformForward;
			result.Direction = direction;
			return result;
		}

		public static CastRequest Sphere(Transform origin, float radius, float distance, LayerMask layer)
		{
			return new CastRequest
			{
				Origin = origin,
				UseTransformForward = true,
				Type = CastType.Sphere,
				Radius = radius,
				Distance = distance,
				LayerMask = layer,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				UpdateMode = UpdateMode.FixedUpdate,
				MaxHits = 8
			};
		}

		public static CastRequest Box(Transform origin, Vector3 halfExtents, float distance, LayerMask layer)
		{
			return new CastRequest
			{
				Origin = origin,
				UseTransformForward = true,
				Type = CastType.Box,
				HalfExtents = halfExtents,
				Distance = distance,
				LayerMask = layer,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				UpdateMode = UpdateMode.FixedUpdate,
				MaxHits = 8
			};
		}

		public static CastRequest OverlapSphere(Transform origin, float radius, LayerMask layer)
		{
			return new CastRequest
			{
				Origin = origin,
				Type = CastType.OverlapSphere,
				Radius = radius,
				LayerMask = layer,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				UpdateMode = UpdateMode.FixedUpdate,
				MaxHits = 16
			};
		}

		public static CastRequest OverlapBox(Transform origin, Vector3 halfExtents, LayerMask layer)
		{
			return new CastRequest
			{
				Origin = origin,
				Type = CastType.OverlapBox,
				HalfExtents = halfExtents,
				LayerMask = layer,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				UpdateMode = UpdateMode.FixedUpdate,
				MaxHits = 16
			};
		}

		public static CastRequest MeshRay(Transform origin, float distance, LayerMask layer = default(LayerMask))
		{
			return new CastRequest
			{
				Origin = origin,
				UseTransformForward = true,
				Type = CastType.MeshRay,
				Distance = distance,
				LayerMask = layer,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				UpdateMode = UpdateMode.LateUpdate,
				MaxHits = 8,
				UseRegisteredMeshes = true
			};
		}

		public static CastRequest MeshRay(Transform origin, Vector3 direction, float distance, LayerMask layer = default(LayerMask), bool useTransformForward = false)
		{
			CastRequest result = MeshRay(origin, distance, layer);
			result.UseTransformForward = useTransformForward;
			result.Direction = direction;
			return result;
		}

		public CastRequest OnlyRegisteredMeshes()
		{
			UseRegisteredMeshes = true;
			LayerMask = 0;
			return this;
		}

		public CastRequest RequireComponent<T>(bool searchInParent = false) where T : class
		{
			ComponentFilter = typeof(T);
			this.searchInParent = searchInParent;
			return this;
		}
	}
}
