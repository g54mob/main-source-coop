using System;
using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class SolidSpaceProbe
	{
		private const float ProbeDistance = 5000f;

		private static readonly Vector3[] AxisDirections = new Vector3[6]
		{
			Vector3.up,
			Vector3.down,
			Vector3.left,
			Vector3.right,
			Vector3.forward,
			Vector3.back
		};

		private const float ConvexProbeRadius = 0.01f;

		public static bool IsTouchingOrInsideSolid(Vector3 worldCenter, Vector3 up, float worldRadius, float worldHeight, Transform ignoreRoot, out Collider blocker)
		{
			return IsCapsuleBuriedInSolid(worldCenter, up, worldRadius, worldHeight, 0f, ignoreRoot, out blocker);
		}

		public static bool IsCapsuleBuriedInSolid(Vector3 worldCenter, Vector3 up, float worldRadius, float worldHeight, float margin, Transform ignoreRoot, out Collider blocker)
		{
			blocker = null;
			float num = Mathf.Max(worldRadius - margin, 0.01f);
			float num2 = Mathf.Max(Mathf.Max(worldHeight - margin * 2f, num * 2f) * 0.5f - num, 0f);
			Vector3 point = worldCenter + up * num2;
			Vector3 point2 = worldCenter - up * num2;
			Collider[] array = Physics.OverlapCapsule(point, point2, num, -1, QueryTriggerInteraction.Ignore);
			foreach (Collider collider in array)
			{
				if (Counts(collider, ignoreRoot))
				{
					blocker = collider;
					return true;
				}
			}
			return IsCenterInsideSolid(worldCenter, ignoreRoot, out blocker);
		}

		public static bool IsCenterInsideSolid(Vector3 worldCenter, Transform ignoreRoot, out Collider blocker)
		{
			bool queriesHitBackfaces = Physics.queriesHitBackfaces;
			Physics.queriesHitBackfaces = true;
			try
			{
				return PointInsideSolid(worldCenter, ignoreRoot, out blocker);
			}
			finally
			{
				Physics.queriesHitBackfaces = queriesHitBackfaces;
			}
		}

		private static bool PointInsideSolid(Vector3 point, Transform ignoreRoot, out Collider blocker)
		{
			blocker = null;
			Collider[] array = Physics.OverlapSphere(point, 0.01f, -1, QueryTriggerInteraction.Ignore);
			foreach (Collider collider in array)
			{
				if (Counts(collider, ignoreRoot) && IsConvex(collider) && collider.ClosestPoint(point) == point)
				{
					blocker = collider;
					return true;
				}
			}
			Collider collider2 = null;
			Vector3[] axisDirections = AxisDirections;
			foreach (Vector3 direction in axisDirections)
			{
				if (!FirstCountedHitIsBackface(point, direction, ignoreRoot, out var wall))
				{
					return false;
				}
				if ((object)collider2 == null)
				{
					collider2 = wall;
				}
			}
			blocker = collider2;
			return true;
		}

		private static bool FirstCountedHitIsBackface(Vector3 point, Vector3 direction, Transform ignoreRoot, out Collider wall)
		{
			wall = null;
			RaycastHit[] array = Physics.RaycastAll(point, direction, 5000f, -1, QueryTriggerInteraction.Ignore);
			Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			RaycastHit[] array2 = array;
			for (int num = 0; num < array2.Length; num++)
			{
				RaycastHit raycastHit = array2[num];
				if (Counts(raycastHit.collider, ignoreRoot) && !IsConvex(raycastHit.collider))
				{
					if (Vector3.Dot(raycastHit.normal, direction) <= 0f)
					{
						return false;
					}
					wall = raycastHit.collider;
					return true;
				}
			}
			return false;
		}

		private static bool Counts(Collider collider, Transform ignoreRoot)
		{
			if (collider == null)
			{
				return false;
			}
			if (collider.GetComponentInParent<VoxelModel>() != null)
			{
				return false;
			}
			if (collider is CharacterController)
			{
				return false;
			}
			if (ignoreRoot != null && (collider.transform == ignoreRoot || collider.transform.IsChildOf(ignoreRoot)))
			{
				return false;
			}
			return true;
		}

		private static bool IsConvex(Collider collider)
		{
			if (!(collider is BoxCollider) && !(collider is SphereCollider) && !(collider is CapsuleCollider))
			{
				if (collider is MeshCollider meshCollider)
				{
					return meshCollider.convex;
				}
				return false;
			}
			return true;
		}
	}
}
