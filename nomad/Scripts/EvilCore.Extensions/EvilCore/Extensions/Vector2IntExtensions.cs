using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Vector2IntExtensions
	{
		public static Vector2Int With(this Vector2Int vector, int axis, int value)
		{
			vector[axis] = value;
			return vector;
		}

		public static Vector2Int WithX(this Vector2Int vector, int x)
		{
			return vector.With(0, x);
		}

		public static Vector2Int WithY(this Vector2Int vector, int y)
		{
			return vector.With(1, y);
		}

		public static Vector2Int WithNegate(this Vector2Int vector, int axis)
		{
			return vector.With(axis, -vector[axis]);
		}

		public static Vector2Int WithNegateX(this Vector2Int vector)
		{
			return vector.WithNegate(0);
		}

		public static Vector2Int WithNegateY(this Vector2Int vector)
		{
			return vector.WithNegate(1);
		}

		public static Vector2Int Negate(this Vector2Int vector)
		{
			return new Vector2Int(-vector.x, -vector.y);
		}

		public static Vector2Int GetYx(this Vector2Int vector)
		{
			return new Vector2Int(vector.y, vector.x);
		}

		public static Vector3Int InsertX(this Vector2Int vector, int x = 0)
		{
			return new Vector3Int(x, vector.x, vector.y);
		}

		public static Vector3Int InsertY(this Vector2Int vector, int y = 0)
		{
			return new Vector3Int(vector.x, y, vector.y);
		}

		public static Vector3Int InsertZ(this Vector2Int vector, int z = 0)
		{
			return new Vector3Int(vector.x, vector.y, z);
		}

		public static (int index, int value) MaxComponent(this Vector2Int vector)
		{
			int num = ((vector.x < vector.y) ? 1 : 0);
			return (index: num, value: vector[num]);
		}

		public static (int index, int value) MinComponent(this Vector2Int vector)
		{
			int num = ((vector.x > vector.y) ? 1 : 0);
			return (index: num, value: vector[num]);
		}

		public static Vector2Int Abs(this Vector2Int vector)
		{
			return new Vector2Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}

		public static Vector2Int Clamp(this Vector2Int vector, int min, int max)
		{
			return new Vector2Int(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max));
		}

		public static Vector2Int Divide(this Vector2Int vector, Vector2Int other)
		{
			return new Vector2Int(vector.x / other.x, vector.y / other.y);
		}

		public static bool IsUniform(this Vector2Int vector)
		{
			return vector.x == vector.y;
		}

		public static (Vector2Int point, int index) GetClosestPoint(this Vector2Int point, params Vector2Int[] points)
		{
			return point.GetClosestPoint((IEnumerable<Vector2Int>)points);
		}

		public static (Vector2Int point, int index) GetClosestPoint(this Vector2Int point, IEnumerable<Vector2Int> points)
		{
			IEnumerator<Vector2Int> enumerator = points.GetEnumerator();
			int num = -1;
			int item = -1;
			Vector2Int item2 = Vector2Int.zero;
			float num2 = 3.4028235E+38f;
			while (enumerator.MoveNext())
			{
				num++;
				float num3 = Vector2Int.Distance(point, enumerator.Current);
				if (num3 < num2)
				{
					item = num;
					num2 = num3;
					item2 = enumerator.Current;
				}
			}
			return (point: item2, index: item);
		}

		public static (Vector2 point, float distance) GetClosestPointOnRay(this Vector2Int point, Vector2 origin, Vector2 direction)
		{
			return Vector2Extensions.GetClosestPointOnRay(point, origin, direction);
		}

		public static (Vector2 point, float distance) GetClosestPointOnSegment(this Vector2Int point, Vector2 start, Vector2 end)
		{
			return Vector2Extensions.GetClosestPointOnSegment(point, start, end);
		}
	}
}
