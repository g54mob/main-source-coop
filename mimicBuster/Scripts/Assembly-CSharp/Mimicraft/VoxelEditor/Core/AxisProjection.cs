using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class AxisProjection
	{
		private const float MinDenominator = 0.02f;

		public static bool TryGetDistanceAlongAxis(Ray ray, Vector3 axisPoint, Vector3 axisDir, out float distanceAlongAxis)
		{
			Vector3 direction = ray.direction;
			Vector3 rhs = ray.origin - axisPoint;
			float num = Vector3.Dot(direction, direction);
			float num2 = Vector3.Dot(direction, axisDir);
			float num3 = Vector3.Dot(axisDir, axisDir);
			float num4 = Vector3.Dot(direction, rhs);
			float num5 = Vector3.Dot(axisDir, rhs);
			float num6 = num * num3 - num2 * num2;
			if (Mathf.Abs(num6) < 0.02f)
			{
				distanceAlongAxis = 0f;
				return false;
			}
			distanceAlongAxis = (num * num5 - num2 * num4) / num6;
			return true;
		}
	}
}
