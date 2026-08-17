using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class PlacementGeometry
	{
		public static float CalculateDynamicNormalOffset(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfaceNormal)
		{
			return CalculateDynamicNormalOffset(renderers, pivotPosition, surfaceNormal, Quaternion.identity);
		}

		public static float CalculateDynamicNormalOffset(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfaceNormal, Quaternion deltaRotation)
		{
			if (renderers == null || renderers.Length == 0)
			{
				return 0f;
			}
			float num = 3.4028235E+38f;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (!(meshRenderer == null) && meshRenderer.enabled)
				{
					Bounds localBounds = meshRenderer.localBounds;
					if (!(localBounds.size == Vector3.zero))
					{
						Transform transform = meshRenderer.transform;
						Vector3 extents = localBounds.extents;
						Vector3 lhs = deltaRotation * (transform.right * extents.x);
						Vector3 lhs2 = deltaRotation * (transform.up * extents.y);
						Vector3 lhs3 = deltaRotation * (transform.forward * extents.z);
						float num2 = Mathf.Abs(Vector3.Dot(lhs, surfaceNormal)) + Mathf.Abs(Vector3.Dot(lhs2, surfaceNormal)) + Mathf.Abs(Vector3.Dot(lhs3, surfaceNormal));
						float b = Vector3.Dot(deltaRotation * (transform.TransformPoint(localBounds.center) - pivotPosition), surfaceNormal) - num2;
						num = Mathf.Min(num, b);
					}
				}
			}
			if (num == 3.4028235E+38f)
			{
				return 0f;
			}
			return 0f - num;
		}

		public static Vector3 CalculateTangentialOffset(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfaceNormal)
		{
			return CalculateTangentialOffset(renderers, pivotPosition, surfaceNormal, Quaternion.identity);
		}

		public static Vector3 CalculateTangentialOffset(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfaceNormal, Quaternion deltaRotation)
		{
			if (renderers == null || renderers.Length == 0)
			{
				return Vector3.zero;
			}
			Vector3 zero = Vector3.zero;
			int num = 0;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (!(meshRenderer == null) && meshRenderer.enabled && !(meshRenderer.localBounds.size == Vector3.zero))
				{
					zero += deltaRotation * (meshRenderer.transform.TransformPoint(meshRenderer.localBounds.center) - pivotPosition);
					num++;
				}
			}
			if (num == 0)
			{
				return Vector3.zero;
			}
			Vector3 vector = zero / num;
			float num2 = Vector3.Dot(vector, surfaceNormal);
			return vector - num2 * surfaceNormal;
		}

		public static Vector3 CalculateTargetPosition(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfacePoint, Vector3 surfaceNormal, Vector3 positionOffset = default(Vector3), Transform objectTransform = null)
		{
			return CalculateTargetPosition(renderers, pivotPosition, surfacePoint, surfaceNormal, Quaternion.identity, positionOffset, objectTransform);
		}

		public static Vector3 CalculateTargetPosition(MeshRenderer[] renderers, Vector3 pivotPosition, Vector3 surfacePoint, Vector3 surfaceNormal, Quaternion deltaRotation, Vector3 positionOffset = default(Vector3), Transform objectTransform = null)
		{
			float num = CalculateDynamicNormalOffset(renderers, pivotPosition, surfaceNormal, deltaRotation);
			Vector3 vector = surfacePoint + surfaceNormal * num;
			Vector3 vector2 = CalculateTangentialOffset(renderers, pivotPosition, surfaceNormal, deltaRotation);
			Vector3 result = vector - vector2;
			if (positionOffset != Vector3.zero && objectTransform != null)
			{
				Vector3 vector3 = deltaRotation * objectTransform.TransformVector(positionOffset);
				float num2 = Vector3.Dot(vector3, surfaceNormal);
				result += vector3 - num2 * surfaceNormal;
			}
			return result;
		}

		public static bool TryCalculateRootLocalColliderBounds(Transform root, Collider[] colliders, out Bounds localBounds)
		{
			localBounds = default(Bounds);
			if (root == null || colliders == null || colliders.Length == 0)
			{
				return false;
			}
			bool hasValid = false;
			Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
			foreach (Collider collider in colliders)
			{
				if (collider == null || !collider.enabled || collider.isTrigger)
				{
					continue;
				}
				Matrix4x4 toRoot = worldToLocalMatrix * collider.transform.localToWorldMatrix;
				Collider collider2 = collider;
				if (!(collider2 is BoxCollider boxCollider))
				{
					if (!(collider2 is MeshCollider meshCollider))
					{
						if (!(collider2 is SphereCollider sphereCollider))
						{
							if (collider2 is CapsuleCollider capsule)
							{
								EncapsulateTransformedCapsule(ref localBounds, ref hasValid, toRoot, capsule);
							}
						}
						else
						{
							EncapsulateTransformedSphere(ref localBounds, ref hasValid, toRoot, sphereCollider.center, sphereCollider.radius);
						}
					}
					else if (meshCollider.sharedMesh != null)
					{
						EncapsulateTransformedAabb(ref localBounds, ref hasValid, toRoot, meshCollider.sharedMesh.bounds);
					}
				}
				else
				{
					EncapsulateTransformedAabb(ref localBounds, ref hasValid, toRoot, new Bounds(boxCollider.center, boxCollider.size));
				}
			}
			return hasValid;
		}

		private static void EncapsulateTransformedAabb(ref Bounds union, ref bool hasValid, Matrix4x4 toRoot, Bounds localAabb)
		{
			if (!(localAabb.size == Vector3.zero))
			{
				Vector3 center = localAabb.center;
				Vector3 extents = localAabb.extents;
				for (int i = 0; i < 8; i++)
				{
					Vector3 point = new Vector3(center.x + (((i & 1) == 0) ? (0f - extents.x) : extents.x), center.y + (((i & 2) == 0) ? (0f - extents.y) : extents.y), center.z + (((i & 4) == 0) ? (0f - extents.z) : extents.z));
					EncapsulatePoint(ref union, ref hasValid, toRoot.MultiplyPoint3x4(point));
				}
			}
		}

		private static void EncapsulateTransformedSphere(ref Bounds union, ref bool hasValid, Matrix4x4 toRoot, Vector3 center, float radius)
		{
			if (!(radius <= 0f))
			{
				float num = radius * MaxColumnScale(toRoot);
				Vector3 vector = toRoot.MultiplyPoint3x4(center);
				Vector3 vector2 = Vector3.one * num;
				EncapsulatePoint(ref union, ref hasValid, vector - vector2);
				EncapsulatePoint(ref union, ref hasValid, vector + vector2);
			}
		}

		private static void EncapsulateTransformedCapsule(ref Bounds union, ref bool hasValid, Matrix4x4 toRoot, CapsuleCollider capsule)
		{
			if (!(capsule.radius <= 0f))
			{
				Vector3 vector = capsule.direction switch
				{
					0 => Vector3.right, 
					2 => Vector3.forward, 
					_ => Vector3.up, 
				};
				float num = Mathf.Max(0f, capsule.height * 0.5f - capsule.radius);
				float num2 = capsule.radius * MaxPerpendicularScale(toRoot, capsule.direction);
				Vector3 vector2 = Vector3.one * num2;
				Vector3 vector3 = toRoot.MultiplyPoint3x4(capsule.center + vector * num);
				Vector3 vector4 = toRoot.MultiplyPoint3x4(capsule.center - vector * num);
				EncapsulatePoint(ref union, ref hasValid, vector3 - vector2);
				EncapsulatePoint(ref union, ref hasValid, vector3 + vector2);
				EncapsulatePoint(ref union, ref hasValid, vector4 - vector2);
				EncapsulatePoint(ref union, ref hasValid, vector4 + vector2);
			}
		}

		private static void EncapsulatePoint(ref Bounds union, ref bool hasValid, Vector3 point)
		{
			if (!hasValid)
			{
				union = new Bounds(point, Vector3.zero);
				hasValid = true;
			}
			else
			{
				union.Encapsulate(point);
			}
		}

		private static float MaxColumnScale(Matrix4x4 m)
		{
			float magnitude = ((Vector3)m.GetColumn(0)).magnitude;
			float magnitude2 = ((Vector3)m.GetColumn(1)).magnitude;
			float magnitude3 = ((Vector3)m.GetColumn(2)).magnitude;
			return Mathf.Max(magnitude, Mathf.Max(magnitude2, magnitude3));
		}

		private static float MaxPerpendicularScale(Matrix4x4 m, int directionAxis)
		{
			float magnitude = ((Vector3)m.GetColumn((directionAxis + 1) % 3)).magnitude;
			float magnitude2 = ((Vector3)m.GetColumn((directionAxis + 2) % 3)).magnitude;
			return Mathf.Max(magnitude, magnitude2);
		}
	}
}
