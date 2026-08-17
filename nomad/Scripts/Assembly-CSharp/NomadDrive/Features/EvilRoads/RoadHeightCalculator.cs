using Unity.Mathematics;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public static class RoadHeightCalculator
	{
		public static float GetTerrainHeight(Vector3 worldPosition, Terrain terrain)
		{
			if (terrain == null || terrain.terrainData == null)
			{
				return worldPosition.y;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 vector = worldPosition - position;
			if (vector.x < 0f || vector.x >= terrainData.size.x || vector.z < 0f || vector.z >= terrainData.size.z)
			{
				return worldPosition.y;
			}
			float x = Mathf.Clamp01(vector.x / terrainData.size.x);
			float y = Mathf.Clamp01(vector.z / terrainData.size.z);
			float interpolatedHeight = terrainData.GetInterpolatedHeight(x, y);
			return position.y + interpolatedHeight;
		}

		public static float? GetRoadMeshHeight(Vector3 worldPosition, Mesh roadMesh, Transform roadTransform)
		{
			if (roadMesh == null || roadTransform == null)
			{
				return null;
			}
			Vector3 point = roadTransform.InverseTransformPoint(worldPosition);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 v = vertices[triangles[i]];
				Vector3 v2 = vertices[triangles[i + 1]];
				Vector3 v3 = vertices[triangles[i + 2]];
				if (IsPointInTriangleExpanded(new Vector2(point.x, point.z), new Vector2(v.x, v.z), new Vector2(v2.x, v2.z), new Vector2(v3.x, v3.z), 0.05f))
				{
					float y = InterpolateHeight(point, v, v2, v3);
					return roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
				}
			}
			return null;
		}

		private static bool IsPointInTriangleExpanded(Vector2 point, Vector2 a, Vector2 b, Vector2 c, float expansion = 0f)
		{
			if (expansion <= 0f)
			{
				return IsPointInTriangle(point, a, b, c);
			}
			Vector2 vector = (a + b + c) / 3f;
			Vector2 a2 = a + (a - vector).normalized * expansion;
			Vector2 b2 = b + (b - vector).normalized * expansion;
			Vector2 c2 = c + (c - vector).normalized * expansion;
			return IsPointInTriangle(point, a2, b2, c2);
		}

		public static float CalculateBlendingWeight(float distance, float influenceDistance, float smoothness)
		{
			if (distance >= influenceDistance)
			{
				return 0f;
			}
			if (distance <= 0f)
			{
				return 1f;
			}
			float num = distance / influenceDistance;
			return Mathf.Clamp01(Mathf.Pow(1f - num, smoothness));
		}

		private static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
		{
			float num = (b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return false;
			}
			float num2 = ((b.y - c.y) * (point.x - c.x) + (c.x - b.x) * (point.y - c.y)) / num;
			float num3 = ((c.y - a.y) * (point.x - c.x) + (a.x - c.x) * (point.y - c.y)) / num;
			float num4 = 1f - num2 - num3;
			if (num2 >= 0f && num3 >= 0f)
			{
				return num4 >= 0f;
			}
			return false;
		}

		private static float InterpolateHeight(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector2 vector = new Vector2(point.x, point.z);
			Vector2 vector2 = new Vector2(v0.x, v0.z);
			Vector2 vector3 = new Vector2(v1.x, v1.z);
			Vector2 vector4 = new Vector2(v2.x, v2.z);
			float num = (vector3.y - vector4.y) * (vector2.x - vector4.x) + (vector4.x - vector3.x) * (vector2.y - vector4.y);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return v0.y;
			}
			float num2 = ((vector3.y - vector4.y) * (vector.x - vector4.x) + (vector4.x - vector3.x) * (vector.y - vector4.y)) / num;
			float num3 = ((vector4.y - vector2.y) * (vector.x - vector4.x) + (vector2.x - vector4.x) * (vector.y - vector4.y)) / num;
			float num4 = 1f - num2 - num3;
			return num2 * v0.y + num3 * v1.y + num4 * v2.y;
		}

		public static bool HasNaN(float3 vector)
		{
			if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z) && !float.IsInfinity(vector.x) && !float.IsInfinity(vector.y))
			{
				return float.IsInfinity(vector.z);
			}
			return true;
		}

		public static bool HasNaN(Vector3 vector)
		{
			if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z) && !float.IsInfinity(vector.x) && !float.IsInfinity(vector.y))
			{
				return float.IsInfinity(vector.z);
			}
			return true;
		}
	}
}
