using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class FacePicker
	{
		public static bool TryPick(Ray worldRay, VoxelModel model, Collider collider, float maxDistance, out Vector3Int voxelPosition, out Vector3Int faceNormal)
		{
			float hitDistance;
			return TryPick(worldRay, model, collider, maxDistance, out voxelPosition, out faceNormal, out hitDistance);
		}

		public static bool TryPick(Ray worldRay, VoxelModel model, Collider collider, float maxDistance, out Vector3Int voxelPosition, out Vector3Int faceNormal, out float hitDistance)
		{
			voxelPosition = default(Vector3Int);
			faceNormal = default(Vector3Int);
			hitDistance = float.MaxValue;
			if (collider == null || !collider.Raycast(worldRay, out var hitInfo, maxDistance))
			{
				return false;
			}
			hitDistance = hitInfo.distance;
			Transform transform = model.transform;
			Vector3 vector = transform.InverseTransformPoint(hitInfo.point);
			Vector3 normal = transform.InverseTransformDirection(hitInfo.normal);
			faceNormal = RoundToAxis(normal);
			Vector3 vector2 = vector - (Vector3)faceNormal * 0.5f;
			voxelPosition = new Vector3Int(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y), Mathf.FloorToInt(vector2.z));
			return model.Grid.Contains(voxelPosition);
		}

		private static Vector3Int RoundToAxis(Vector3 normal)
		{
			float num = Mathf.Abs(normal.x);
			float num2 = Mathf.Abs(normal.y);
			float num3 = Mathf.Abs(normal.z);
			if (num >= num2 && num >= num3)
			{
				return new Vector3Int((normal.x > 0f) ? 1 : (-1), 0, 0);
			}
			if (num2 >= num && num2 >= num3)
			{
				return new Vector3Int(0, (normal.y > 0f) ? 1 : (-1), 0);
			}
			return new Vector3Int(0, 0, (normal.z > 0f) ? 1 : (-1));
		}
	}
}
