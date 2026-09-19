using UnityEngine;

namespace Features.HeadwearModule.Scripts
{
	public static class HeadwearRicochetSolver
	{
		private const float DEGENERATE_SQR = 1E-06f;

		public static bool TrySolve(Vector3 center, float radius, float projectileRadius, Vector3 travelPoint, Vector3 travelDirection, float travelBack, out Vector3 point, out Vector3 normal)
		{
			point = travelPoint;
			normal = Vector3.up;
			if (radius <= 0f)
			{
				return false;
			}
			float num = radius + projectileRadius;
			Vector3 vector = travelPoint - travelDirection * travelBack;
			Vector3 lhs = vector - center;
			float num2 = Vector3.Dot(lhs, travelDirection);
			float num3 = num2 * num2 - (lhs.sqrMagnitude - num * num);
			if (num3 >= 0f)
			{
				float num4 = 0f - num2 - Mathf.Sqrt(num3);
				if (num4 >= 0f && num4 <= travelBack)
				{
					point = vector + travelDirection * num4;
					normal = (point - center).normalized;
					return true;
				}
			}
			Vector3 vector2 = travelPoint - center;
			if (vector2.sqrMagnitude > num * num)
			{
				return false;
			}
			normal = ((vector2.sqrMagnitude < 1E-06f) ? (-travelDirection) : vector2.normalized);
			return Vector3.Dot(travelDirection, normal) < 0f;
		}
	}
}
