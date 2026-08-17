using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class TargetOverlapHelper
	{
		public static int OverlapColliderAtTarget(Collider col, Quaternion deltaRot, Vector3 rootPos, Vector3 targetPos, int mask, float shrink, Collider[] buffer)
		{
			if (col is BoxCollider boxCollider)
			{
				Vector3 vector = col.transform.TransformPoint(boxCollider.center);
				Vector3 center = deltaRot * (vector - rootPos) + targetPos;
				Quaternion orientation = deltaRot * col.transform.rotation;
				Vector3 lossyScale = col.transform.lossyScale;
				Vector3 halfExtents = new Vector3(boxCollider.size.x * 0.5f * Mathf.Abs(lossyScale.x) * shrink, boxCollider.size.y * 0.5f * Mathf.Abs(lossyScale.y) * shrink, boxCollider.size.z * 0.5f * Mathf.Abs(lossyScale.z) * shrink);
				return Physics.OverlapBoxNonAlloc(center, halfExtents, buffer, orientation, mask, QueryTriggerInteraction.Ignore);
			}
			if (col is SphereCollider sphereCollider)
			{
				Vector3 vector2 = col.transform.TransformPoint(sphereCollider.center);
				Vector3 position = deltaRot * (vector2 - rootPos) + targetPos;
				Vector3 lossyScale2 = col.transform.lossyScale;
				float num = Mathf.Max(Mathf.Abs(lossyScale2.x), Mathf.Max(Mathf.Abs(lossyScale2.y), Mathf.Abs(lossyScale2.z)));
				float radius = sphereCollider.radius * num * shrink;
				return Physics.OverlapSphereNonAlloc(position, radius, buffer, mask, QueryTriggerInteraction.Ignore);
			}
			if (col is CapsuleCollider capsuleCollider)
			{
				Vector3 vector3 = col.transform.TransformPoint(capsuleCollider.center);
				Vector3 vector4 = deltaRot * (vector3 - rootPos) + targetPos;
				Quaternion quaternion = deltaRot * col.transform.rotation;
				Vector3 lossyScale3 = col.transform.lossyScale;
				float num2;
				float num3;
				Vector3 vector5;
				switch (capsuleCollider.direction)
				{
				case 0:
					num2 = Mathf.Abs(lossyScale3.x);
					num3 = Mathf.Max(Mathf.Abs(lossyScale3.y), Mathf.Abs(lossyScale3.z));
					vector5 = Vector3.right;
					break;
				case 2:
					num2 = Mathf.Abs(lossyScale3.z);
					num3 = Mathf.Max(Mathf.Abs(lossyScale3.x), Mathf.Abs(lossyScale3.y));
					vector5 = Vector3.forward;
					break;
				default:
					num2 = Mathf.Abs(lossyScale3.y);
					num3 = Mathf.Max(Mathf.Abs(lossyScale3.x), Mathf.Abs(lossyScale3.z));
					vector5 = Vector3.up;
					break;
				}
				float num4 = capsuleCollider.radius * num3 * shrink;
				float num5 = Mathf.Max(capsuleCollider.height * num2 * shrink * 0.5f - num4, 0f);
				Vector3 vector6 = quaternion * vector5;
				Vector3 point = vector4 + vector6 * num5;
				Vector3 point2 = vector4 - vector6 * num5;
				return Physics.OverlapCapsuleNonAlloc(point, point2, num4, buffer, mask, QueryTriggerInteraction.Ignore);
			}
			if (col is MeshCollider meshCollider && meshCollider.sharedMesh != null)
			{
				Bounds bounds = meshCollider.sharedMesh.bounds;
				Vector3 vector7 = col.transform.TransformPoint(bounds.center);
				Vector3 center2 = deltaRot * (vector7 - rootPos) + targetPos;
				Quaternion orientation2 = deltaRot * col.transform.rotation;
				Vector3 lossyScale4 = col.transform.lossyScale;
				Vector3 halfExtents2 = new Vector3(bounds.extents.x * Mathf.Abs(lossyScale4.x) * shrink, bounds.extents.y * Mathf.Abs(lossyScale4.y) * shrink, bounds.extents.z * Mathf.Abs(lossyScale4.z) * shrink);
				return Physics.OverlapBoxNonAlloc(center2, halfExtents2, buffer, orientation2, mask, QueryTriggerInteraction.Ignore);
			}
			Bounds bounds2 = col.bounds;
			Vector3 center3 = deltaRot * (bounds2.center - rootPos) + targetPos;
			Vector3 halfExtents3 = bounds2.extents * shrink;
			return Physics.OverlapBoxNonAlloc(center3, halfExtents3, buffer, deltaRot, mask, QueryTriggerInteraction.Ignore);
		}
	}
}
