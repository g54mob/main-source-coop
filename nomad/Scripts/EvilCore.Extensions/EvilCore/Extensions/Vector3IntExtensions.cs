using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Vector3IntExtensions
	{
		public static Vector3Int With(this Vector3Int vector, int axis, int value)
		{
			vector[axis] = value;
			return vector;
		}

		public static Vector3Int WithX(this Vector3Int vector, int x)
		{
			return vector.With(0, x);
		}

		public static Vector3Int WithY(this Vector3Int vector, int y)
		{
			return vector.With(1, y);
		}

		public static Vector3Int WithZ(this Vector3Int vector, int z)
		{
			return vector.With(2, z);
		}

		public static Vector3Int With(this Vector3Int vector, int axis1, int value1, int axis2, int value2)
		{
			vector[axis1] = value1;
			vector[axis2] = value2;
			return vector;
		}

		public static Vector3Int WithXY(this Vector3Int vector, int x, int y)
		{
			return vector.With(0, x, 1, y);
		}

		public static Vector3 WithXY(this Vector3Int vector, Vector2Int value)
		{
			return vector.With(0, value.x, 1, value.y);
		}

		public static Vector3Int WithXZ(this Vector3Int vector, int x, int z)
		{
			return vector.With(0, x, 2, z);
		}

		public static Vector3 WithXZ(this Vector3Int vector, Vector2Int value)
		{
			return vector.With(0, value.x, 2, value.y);
		}

		public static Vector3Int WithYZ(this Vector3Int vector, int y, int z)
		{
			return vector.With(1, y, 2, z);
		}

		public static Vector3 WithYZ(this Vector3Int vector, Vector2Int value)
		{
			return vector.With(1, value.x, 2, value.y);
		}

		public static Vector3Int WithNegate(this Vector3Int vector, int axis)
		{
			return vector.With(axis, -vector[axis]);
		}

		public static Vector3Int WithNegateX(this Vector3Int vector)
		{
			return vector.WithNegate(0);
		}

		public static Vector3Int WithNegateY(this Vector3Int vector)
		{
			return vector.WithNegate(1);
		}

		public static Vector3Int WithNegateZ(this Vector3Int vector)
		{
			return vector.WithNegate(2);
		}

		public static Vector3Int WithNegate(this Vector3Int vector, int axis1, int axis2)
		{
			vector[axis1] = -vector[axis1];
			vector[axis2] = -vector[axis2];
			return vector;
		}

		public static Vector3Int WithNegateXY(this Vector3Int vector)
		{
			return vector.WithNegate(0, 1);
		}

		public static Vector3Int WithNegateXZ(this Vector3Int vector)
		{
			return vector.WithNegate(0, 2);
		}

		public static Vector3Int WithNegateYZ(this Vector3Int vector)
		{
			return vector.WithNegate(1, 2);
		}

		public static Vector3Int Negate(this Vector3Int vector)
		{
			return new Vector3Int(-vector.x, -vector.y, -vector.z);
		}

		public static Vector2Int Get(this Vector3Int vector, int axis1, int axis2)
		{
			return new Vector2Int(vector[axis1], vector[axis2]);
		}

		public static Vector2Int GetXY(this Vector3Int vector)
		{
			return vector.Get(0, 1);
		}

		public static Vector2Int GetXZ(this Vector3Int vector)
		{
			return vector.Get(0, 2);
		}

		public static Vector2Int GetYx(this Vector3Int vector)
		{
			return vector.Get(1, 0);
		}

		public static Vector2Int GetYZ(this Vector3Int vector)
		{
			return vector.Get(1, 2);
		}

		public static Vector2Int GetZx(this Vector3Int vector)
		{
			return vector.Get(2, 0);
		}

		public static Vector2Int GetZy(this Vector3Int vector)
		{
			return vector.Get(2, 1);
		}

		public static Vector3Int Get(this Vector3Int vector, int axis1, int axis2, int axis3)
		{
			return new Vector3Int(vector[axis1], vector[axis2], vector[axis3]);
		}

		public static Vector3Int GetXZY(this Vector3Int vector)
		{
			return vector.Get(0, 2, 1);
		}

		public static Vector3Int GetYXZ(this Vector3Int vector)
		{
			return vector.Get(1, 0, 2);
		}

		public static Vector3Int GetYZX(this Vector3Int vector)
		{
			return vector.Get(1, 2, 0);
		}

		public static Vector3Int GetZXY(this Vector3Int vector)
		{
			return vector.Get(2, 0, 1);
		}

		public static Vector3Int GetZYX(this Vector3Int vector)
		{
			return vector.Get(2, 1, 0);
		}

		private static void Compare(Vector3Int vector, ref int index, int compareIndex, int result)
		{
			if (vector[compareIndex].CompareTo(vector[index]) == result)
			{
				index = compareIndex;
			}
		}

		private static int CompareAllComponents(Vector3Int vector, int result)
		{
			int index = 0;
			Compare(vector, ref index, 1, result);
			Compare(vector, ref index, 2, result);
			return index;
		}

		public static (int index, float value) MaxComponent(this Vector3Int vector)
		{
			int num = CompareAllComponents(vector, 1);
			return (index: num, value: vector[num]);
		}

		public static (int index, float value) MinComponent(this Vector3Int vector)
		{
			int num = CompareAllComponents(vector, -1);
			return (index: num, value: vector[num]);
		}

		public static Vector3Int Abs(this Vector3Int vector)
		{
			return new Vector3Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

		public static Vector3Int Clamp(this Vector3Int vector, int min, int max)
		{
			return new Vector3Int(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max));
		}

		public static Vector3Int Divide(this Vector3Int vector, Vector3Int other)
		{
			return new Vector3Int(vector.x / other.x, vector.y / other.y, vector.z / other.z);
		}

		public static bool IsUniform(this Vector3Int vector)
		{
			if (vector.x == vector.y)
			{
				return vector.y == vector.z;
			}
			return false;
		}

		public static (Vector3Int point, int index) GetClosestPoint(this Vector3Int point, params Vector3Int[] points)
		{
			return point.GetClosestPoint((IEnumerable<Vector3Int>)points);
		}

		public static (Vector3Int point, int index) GetClosestPoint(this Vector3Int point, IEnumerable<Vector3Int> points)
		{
			IEnumerator<Vector3Int> enumerator = points.GetEnumerator();
			int num = -1;
			int item = -1;
			Vector3Int item2 = Vector3Int.zero;
			float num2 = 3.4028235E+38f;
			while (enumerator.MoveNext())
			{
				num++;
				float num3 = Vector3Int.Distance(point, enumerator.Current);
				if (num3 < num2)
				{
					item = num;
					num2 = num3;
					item2 = enumerator.Current;
				}
			}
			return (point: item2, index: item);
		}

		public static (Vector3 point, float distance) GetClosestPointOnRay(this Vector3Int point, Vector3 origin, Vector3 direction)
		{
			return Vector3Extensions.GetClosestPointOnRay(point, origin, direction);
		}

		public static (Vector3 point, float distance) GetClosestPointOnSegment(this Vector3 point, Vector3 start, Vector3 end)
		{
			return Vector3Extensions.GetClosestPointOnSegment(point, start, end);
		}
	}
}
