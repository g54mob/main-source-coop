using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Vector3Extensions
	{
		public static Vector3 With(this Vector3 vector, int axis, float value)
		{
			vector[axis] = value;
			return vector;
		}

		public static Vector3 WithX(this Vector3 vector, float x)
		{
			return vector.With(0, x);
		}

		public static Vector3 WithY(this Vector3 vector, float y)
		{
			return vector.With(1, y);
		}

		public static Vector3 WithZ(this Vector3 vector, float z)
		{
			return vector.With(2, z);
		}

		public static Vector3 With(this Vector3 vector, int axis1, float value1, int axis2, float value2)
		{
			vector[axis1] = value1;
			vector[axis2] = value2;
			return vector;
		}

		public static Vector3 WithXY(this Vector3 vector, float x, float y)
		{
			return vector.With(0, x, 1, y);
		}

		public static Vector3 WithXY(this Vector3 vector, Vector2 value)
		{
			return vector.With(0, value.x, 1, value.y);
		}

		public static Vector3 WithXZ(this Vector3 vector, float x, float z)
		{
			return vector.With(0, x, 2, z);
		}

		public static Vector3 WithXZ(this Vector3 vector, Vector2 value)
		{
			return vector.With(0, value.x, 2, value.y);
		}

		public static Vector3 WithYZ(this Vector3 vector, float y, float z)
		{
			return vector.With(1, y, 2, z);
		}

		public static Vector3 WithYZ(this Vector3 vector, Vector2 value)
		{
			return vector.With(1, value.x, 2, value.y);
		}

		public static Vector3 WithNegate(this Vector3 vector, int axis)
		{
			return vector.With(axis, 0f - vector[axis]);
		}

		public static Vector3 WithNegateX(this Vector3 vector)
		{
			return vector.WithNegate(0);
		}

		public static Vector3 WithNegateY(this Vector3 vector)
		{
			return vector.WithNegate(1);
		}

		public static Vector3 WithNegateZ(this Vector3 vector)
		{
			return vector.WithNegate(2);
		}

		public static Vector3 WithNegate(this Vector3 vector, int axis1, int axis2)
		{
			vector[axis1] = 0f - vector[axis1];
			vector[axis2] = 0f - vector[axis2];
			return vector;
		}

		public static Vector3 WithNegateXY(this Vector3 vector)
		{
			return vector.WithNegate(0, 1);
		}

		public static Vector3 WithNegateXZ(this Vector3 vector)
		{
			return vector.WithNegate(0, 2);
		}

		public static Vector3 WithNegateYZ(this Vector3 vector)
		{
			return vector.WithNegate(1, 2);
		}

		public static Vector3 Negate(this Vector3 vector)
		{
			return new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
		}

		public static Vector2 Get(this Vector3 vector, int axis1, int axis2)
		{
			return new Vector2(vector[axis1], vector[axis2]);
		}

		public static Vector2 GetXY(this Vector3 vector)
		{
			return vector.Get(0, 1);
		}

		public static Vector2 GetXZ(this Vector3 vector)
		{
			return vector.Get(0, 2);
		}

		public static Vector2 GetYx(this Vector3 vector)
		{
			return vector.Get(1, 0);
		}

		public static Vector2 GetYZ(this Vector3 vector)
		{
			return vector.Get(1, 2);
		}

		public static Vector2 GetZx(this Vector3 vector)
		{
			return vector.Get(2, 0);
		}

		public static Vector2 GetZy(this Vector3 vector)
		{
			return vector.Get(2, 1);
		}

		public static Vector3 Get(this Vector3 vector, int axis1, int axis2, int axis3)
		{
			return new Vector3(vector[axis1], vector[axis2], vector[axis3]);
		}

		public static Vector3 GetXZY(this Vector3 vector)
		{
			return vector.Get(0, 2, 1);
		}

		public static Vector3 GetYXZ(this Vector3 vector)
		{
			return vector.Get(1, 0, 2);
		}

		public static Vector3 GetYZX(this Vector3 vector)
		{
			return vector.Get(1, 2, 0);
		}

		public static Vector3 GetZXY(this Vector3 vector)
		{
			return vector.Get(2, 0, 1);
		}

		public static Vector3 GetZYX(this Vector3 vector)
		{
			return vector.Get(2, 1, 0);
		}

		public static Vector4 InsertX(this Vector3 vector, float x = 0f)
		{
			return new Vector4(x, vector.x, vector.y, vector.z);
		}

		public static Vector4 InsertY(this Vector3 vector, float y = 0f)
		{
			return new Vector4(vector.x, y, vector.y, vector.z);
		}

		public static Vector4 InsertZ(this Vector3 vector, float z = 0f)
		{
			return new Vector4(vector.x, vector.y, z, vector.z);
		}

		public static Vector4 InsertW(this Vector3 vector, float w = 0f)
		{
			return new Vector4(vector.x, vector.y, vector.z, w);
		}

		private static void Compare(Vector3 vector, ref int index, int compareIndex, int result)
		{
			if (vector[compareIndex].CompareTo(vector[index]) == result)
			{
				index = compareIndex;
			}
		}

		private static int CompareAllComponents(Vector3 vector, int result)
		{
			int index = 0;
			Compare(vector, ref index, 1, result);
			Compare(vector, ref index, 2, result);
			return index;
		}

		public static int MaxComponentIndex(this Vector3 vector)
		{
			return CompareAllComponents(vector, 1);
		}

		public static float MaxComponent(this Vector3 vector)
		{
			return vector[vector.MaxComponentIndex()];
		}

		public static int MinComponentIndex(this Vector3 vector)
		{
			return CompareAllComponents(vector, -1);
		}

		public static float MinComponent(this Vector3 vector)
		{
			return vector[vector.MinComponentIndex()];
		}

		public static Vector3 Remap(this Vector3 vector, float min1, float max1, float min2, float max2)
		{
			return new Vector3(vector.x.Remap(min1, max1, min2, max2), vector.y.Remap(min1, max1, min2, max2), vector.z.Remap(min1, max1, min2, max2));
		}

		public static Vector3 Abs(this Vector3 vector)
		{
			return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

		public static Vector3 Clamp(this Vector3 vector, float min, float max)
		{
			return new Vector3(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max));
		}

		public static Vector3 Clamp01(this Vector3 vector)
		{
			return new Vector3(Mathf.Clamp01(vector.x), Mathf.Clamp01(vector.y), Mathf.Clamp01(vector.z));
		}

		public static Vector3 Divide(this Vector3 vector, Vector3 other)
		{
			return new Vector3(vector.x / other.x, vector.y / other.y, vector.z / other.z);
		}

		public static bool IsUniform(this Vector3 vector)
		{
			if (vector.x.Approximately(vector.y))
			{
				return vector.y.Approximately(vector.z);
			}
			return false;
		}

		public static Vector3 EventlyDistributedPointOnSphere(int index, float radius, int count)
		{
			float num = (float)index + 0.5f;
			float f = Mathf.Acos(1f - 2f * num / (float)count);
			float f2 = (float)Math.PI * (1f + Mathf.Sqrt(5f)) * num;
			float x = Mathf.Cos(f2) * Mathf.Sin(f);
			float y = Mathf.Sin(f2) * Mathf.Sin(f);
			float z = Mathf.Cos(f);
			return new Vector3(x, y, z) * radius;
		}

		public static (Vector3 point, int index) GetClosestPoint(this Vector3 point, params Vector3[] points)
		{
			return point.GetClosestPoint((IEnumerable<Vector3>)points);
		}

		public static (Vector3 point, int index) GetClosestPoint(this Vector3 point, IEnumerable<Vector3> points)
		{
			IEnumerator<Vector3> enumerator = points.GetEnumerator();
			int num = -1;
			int item = -1;
			Vector3 item2 = Vector3.zero;
			float num2 = 3.4028235E+38f;
			while (enumerator.MoveNext())
			{
				num++;
				float num3 = Vector3.Distance(point, enumerator.Current);
				if (num3 < num2)
				{
					item = num;
					num2 = num3;
					item2 = enumerator.Current;
				}
			}
			return (point: item2, index: item);
		}

		public static (Vector3 point, float distance) GetClosestPointOnRay(this Vector3 point, Vector3 origin, Vector3 direction)
		{
			float num = Vector3.Dot(point - origin, direction);
			return (point: origin + direction * num, distance: num);
		}

		public static (Vector3 point, float distance) GetClosestPointOnSegment(this Vector3 point, Vector3 start, Vector3 end)
		{
			Vector3 vector = end - start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = Mathf.Clamp(Vector3.Dot(point - start, vector), 0f, magnitude);
			return (point: start + vector * num, distance: num);
		}
	}
}
