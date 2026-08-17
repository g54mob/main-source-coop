using UnityEngine;

namespace EvilCore.DynamicCasting
{
	public static class MeshRayUtility
	{
		private const float EPSILON = 1E-07f;

		public static bool RayTriangleIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 v0, Vector3 v1, Vector3 v2, out float t, out float u, out float v)
		{
			t = 0f;
			u = 0f;
			v = 0f;
			Vector3 vector = v1 - v0;
			Vector3 vector2 = v2 - v0;
			Vector3 rhs = Vector3.Cross(rayDirection, vector2);
			float num = Vector3.Dot(vector, rhs);
			if (num > -1E-07f && num < 1E-07f)
			{
				return false;
			}
			float num2 = 1f / num;
			Vector3 lhs = rayOrigin - v0;
			u = num2 * Vector3.Dot(lhs, rhs);
			if (u < 0f || u > 1f)
			{
				return false;
			}
			Vector3 rhs2 = Vector3.Cross(lhs, vector);
			v = num2 * Vector3.Dot(rayDirection, rhs2);
			if (v < 0f || u + v > 1f)
			{
				return false;
			}
			t = num2 * Vector3.Dot(vector2, rhs2);
			return t > 1E-07f;
		}

		public static Vector3 BarycentricInterpolate(Vector3 v0, Vector3 v1, Vector3 v2, float u, float v)
		{
			return (1f - u - v) * v0 + u * v1 + v * v2;
		}

		public static Vector2 BarycentricInterpolate(Vector2 v0, Vector2 v1, Vector2 v2, float u, float v)
		{
			return (1f - u - v) * v0 + u * v1 + v * v2;
		}

		public static Vector3 GetBarycentricCoord(float u, float v)
		{
			return new Vector3(1f - u - v, u, v);
		}

		public static bool RayIntersectsBounds(Vector3 rayOrigin, Vector3 rayDirection, Bounds bounds)
		{
			return bounds.IntersectRay(new Ray(rayOrigin, rayDirection));
		}

		public static bool RayIntersectsBounds(Vector3 rayOrigin, Vector3 rayDirection, Bounds bounds, out float distance)
		{
			Ray ray = new Ray(rayOrigin, rayDirection);
			return bounds.IntersectRay(ray, out distance);
		}
	}
}
