using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Vector2Extensions
	{
		public static Vector2 With(this Vector2 vector, int axis, float value)
		{
			vector[axis] = value;
			return vector;
		}

		public static Vector2 WithX(this Vector2 vector, float x)
		{
			return vector.With(0, x);
		}

		public static Vector2 WithY(this Vector2 vector, float y)
		{
			return vector.With(1, y);
		}

		public static Vector2 WithNegate(this Vector2 vector, int axis)
		{
			return vector.With(axis, 0f - vector[axis]);
		}

		public static Vector2 WithNegateX(this Vector2 vector)
		{
			return vector.WithNegate(0);
		}

		public static Vector2 WithNegateY(this Vector2 vector)
		{
			return vector.WithNegate(1);
		}

		public static Vector2 Negate(this Vector2 vector)
		{
			return new Vector2(0f - vector.x, 0f - vector.y);
		}

		public static Vector2 GetYx(this Vector2 vector)
		{
			return new Vector2(vector.y, vector.x);
		}

		public static Vector3 InsertX(this Vector2 vector, float x = 0f)
		{
			return new Vector3(x, vector.x, vector.y);
		}

		public static Vector3 InsertY(this Vector2 vector, float y = 0f)
		{
			return new Vector3(vector.x, y, vector.y);
		}

		public static Vector3 InsertZ(this Vector2 vector, float z = 0f)
		{
			return new Vector3(vector.x, vector.y, z);
		}

		public static int MaxComponentIndex(this Vector2 vector)
		{
			if (!(vector.x >= vector.y))
			{
				return 1;
			}
			return 0;
		}

		public static float MaxComponent(this Vector2 vector)
		{
			return vector[vector.MaxComponentIndex()];
		}

		public static int MinComponentIndex(this Vector2 vector)
		{
			if (!(vector.x <= vector.y))
			{
				return 1;
			}
			return 0;
		}

		public static float MinComponent(this Vector2 vector)
		{
			return vector[vector.MinComponentIndex()];
		}

		public static Vector2 Remap(this Vector2 vector, float min1, float max1, float min2, float max2)
		{
			return new Vector2(vector.x.Remap(min1, max1, min2, max2), vector.y.Remap(min1, max1, min2, max2));
		}

		public static Vector2 Abs(this Vector2 vector)
		{
			return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}

		public static Vector2 Clamp(this Vector2 vector, float min, float max)
		{
			return new Vector2(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max));
		}

		public static Vector2 Clamp01(this Vector2 vector)
		{
			return new Vector2(Mathf.Clamp01(vector.x), Mathf.Clamp01(vector.y));
		}

		public static Vector2 Divide(this Vector2 vector, Vector2 other)
		{
			return vector / other;
		}

		public static bool IsUniform(this Vector2 vector)
		{
			return vector.x.Approximately(vector.y);
		}

		public static Vector2 EventlyDistributedPointOnCircle(int index, float radius, int count)
		{
			float num = (float)index + 0.5f;
			float num2 = Mathf.Sqrt(num / (float)count);
			float f = (float)Math.PI * (1f + Mathf.Sqrt(5f)) * num;
			float x = num2 * Mathf.Cos(f) * radius;
			float y = num2 * Mathf.Sin(f) * radius;
			return new Vector2(x, y);
		}

		public static (Vector2 point, int index) GetClosestPoint(this Vector2 point, params Vector2[] points)
		{
			return point.GetClosestPoint((IEnumerable<Vector2>)points);
		}

		public static (Vector2 point, int index) GetClosestPoint(this Vector2 point, IEnumerable<Vector2> points)
		{
			IEnumerator<Vector2> enumerator = points.GetEnumerator();
			int num = -1;
			int item = -1;
			Vector2 item2 = Vector2.zero;
			float num2 = 3.4028235E+38f;
			while (enumerator.MoveNext())
			{
				num++;
				float num3 = Vector2.Distance(point, enumerator.Current);
				if (num3 < num2)
				{
					item = num;
					num2 = num3;
					item2 = enumerator.Current;
				}
			}
			return (point: item2, index: item);
		}

		public static (Vector2 point, float distance) GetClosestPointOnRay(this Vector2 point, Vector2 origin, Vector2 direction)
		{
			float num = Vector2.Dot(point - origin, direction);
			return (point: origin + direction * num, distance: num);
		}

		public static (Vector2 point, float distance) GetClosestPointOnSegment(this Vector2 point, Vector2 start, Vector2 end)
		{
			Vector2 vector = end - start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = Mathf.Clamp(Vector2.Dot(point - start, vector), 0f, magnitude);
			return (point: start + vector * num, distance: num);
		}
	}
}
