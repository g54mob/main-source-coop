using UnityEngine;

namespace Den.Tools
{
	public static class Collisions
	{
		public static bool RayhitBoundingBox(Ray ray, Vector3 minB, Vector3 maxB, out Vector3 coord)
		{
			Vector3 origin = ray.origin;
			Vector3 normalized = ray.direction.normalized;
			coord = default(Vector3);
			bool flag = true;
			Vector3Int vector3Int = default(Vector3Int);
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			for (int i = 0; i < 3; i++)
			{
				if (origin[i] < minB[i])
				{
					vector3Int[i] = 1;
					vector2[i] = minB[i];
					flag = false;
				}
				else if (origin[i] > maxB[i])
				{
					vector3Int[i] = 0;
					vector2[i] = maxB[i];
					flag = false;
				}
				else
				{
					vector3Int[i] = 2;
				}
			}
			if (flag)
			{
				coord = origin;
				return true;
			}
			for (int j = 0; j < 3; j++)
			{
				if (vector3Int[j] != 2 && (double)normalized[j] != 0.0)
				{
					vector[j] = (vector2[j] - origin[j]) / normalized[j];
				}
				else
				{
					vector[j] = -1f;
				}
			}
			int num = 0;
			for (int k = 1; k < 3; k++)
			{
				if (vector[num] < vector[k])
				{
					num = k;
				}
			}
			if ((double)vector[num] < 0.0)
			{
				return false;
			}
			for (int l = 0; l < 3; l++)
			{
				if (num != l)
				{
					coord[l] = origin[l] + vector[num] * normalized[l];
					if (coord[l] < minB[l] || coord[l] > maxB[l])
					{
						return false;
					}
				}
				else
				{
					coord[l] = vector2[l];
				}
			}
			return true;
		}
	}
}
