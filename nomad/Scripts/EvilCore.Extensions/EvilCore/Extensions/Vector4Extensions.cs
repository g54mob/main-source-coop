using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Vector4Extensions
	{
		public static Vector2 With(this Vector4 vector, int axis, float value)
		{
			vector[axis] = value;
			return vector;
		}

		public static Vector4 WithX(this Vector4 vector, float x)
		{
			return vector.With(0, x);
		}

		public static Vector4 WithY(this Vector4 vector, float y)
		{
			return vector.With(1, y);
		}

		public static Vector4 WithZ(this Vector4 vector, float z)
		{
			return vector.With(2, z);
		}

		public static Vector4 WithW(this Vector4 vector, float w)
		{
			return vector.With(3, w);
		}

		public static Vector2 With(this Vector4 vector, int axis1, float value1, int axis2, float value2)
		{
			vector[axis1] = value1;
			vector[axis2] = value2;
			return vector;
		}

		public static Vector4 WithXY(this Vector4 vector, float x, float y)
		{
			return vector.With(0, x, 1, y);
		}

		public static Vector4 WithXY(this Vector4 vector, Vector2 value)
		{
			return vector.With(0, value.x, 1, value.y);
		}

		public static Vector4 WithXZ(this Vector4 vector, float x, float z)
		{
			return vector.With(0, x, 2, z);
		}

		public static Vector4 WithXZ(this Vector4 vector, Vector2 value)
		{
			return vector.With(0, value.x, 2, value.y);
		}

		public static Vector4 WithXw(this Vector4 vector, float x, float w)
		{
			return vector.With(0, x, 3, w);
		}

		public static Vector4 WithXw(this Vector4 vector, Vector2 value)
		{
			return vector.With(0, value.x, 3, value.y);
		}

		public static Vector4 WithYZ(this Vector4 vector, float y, float z)
		{
			return vector.With(1, y, 2, z);
		}

		public static Vector4 WithYZ(this Vector4 vector, Vector2 value)
		{
			return vector.With(1, value.x, 2, value.y);
		}

		public static Vector4 WithYw(this Vector4 vector, float y, float w)
		{
			return vector.With(1, y, 3, w);
		}

		public static Vector4 WithYw(this Vector4 vector, Vector2 value)
		{
			return vector.With(1, value.x, 3, value.y);
		}

		public static Vector4 WithZw(this Vector4 vector, float z, float w)
		{
			return vector.With(2, z, 3, w);
		}

		public static Vector4 WithZw(this Vector4 vector, Vector2 value)
		{
			return vector.With(2, value.x, 3, value.y);
		}

		public static Vector2 With(this Vector4 vector, int axis1, float value1, int axis2, float value2, int axis3, float value3)
		{
			vector[axis1] = value1;
			vector[axis2] = value2;
			vector[axis3] = value3;
			return vector;
		}

		public static Vector4 WithXYZ(this Vector4 vector, float x, float y, float z)
		{
			return vector.With(0, x, 1, y, 2, z);
		}

		public static Vector4 WithXYZ(this Vector4 vector, Vector3 value)
		{
			return vector.With(0, value.x, 1, value.y, 2, value.z);
		}

		public static Vector4 WithXyw(this Vector4 vector, float x, float y, float w)
		{
			return vector.With(0, x, 1, y, 3, w);
		}

		public static Vector4 WithXyw(this Vector4 vector, Vector3 value)
		{
			return vector.With(0, value.x, 1, value.y, 3, value.z);
		}

		public static Vector4 WithXzw(this Vector4 vector, float x, float z, float w)
		{
			return vector.With(0, x, 2, z, 3, w);
		}

		public static Vector4 WithXzw(this Vector4 vector, Vector3 value)
		{
			return vector.With(0, value.x, 2, value.y, 3, value.z);
		}

		public static Vector4 WithYzw(this Vector4 vector, float y, float z, float w)
		{
			return vector.With(1, y, 2, z, 3, w);
		}

		public static Vector4 WithYzw(this Vector4 vector, Vector3 value)
		{
			return vector.With(1, value.x, 2, value.y, 3, value.z);
		}

		public static Vector4 WithNegate(this Vector4 vector, int axis)
		{
			return vector.With(axis, 0f - vector[axis]);
		}

		public static Vector4 WithNegateX(this Vector4 vector)
		{
			return vector.WithNegate(0);
		}

		public static Vector4 WithNegateY(this Vector4 vector)
		{
			return vector.WithNegate(1);
		}

		public static Vector4 WithNegateZ(this Vector4 vector)
		{
			return vector.WithNegate(2);
		}

		public static Vector4 WithNegateW(this Vector4 vector)
		{
			return vector.WithNegate(3);
		}

		public static Vector4 WithNegate(this Vector4 vector, int axis1, int axis2)
		{
			vector[axis1] = 0f - vector[axis1];
			vector[axis2] = 0f - vector[axis2];
			return vector;
		}

		public static Vector4 WithNegateXY(this Vector4 vector)
		{
			return vector.WithNegate(0, 1);
		}

		public static Vector4 WithNegateXZ(this Vector4 vector)
		{
			return vector.WithNegate(0, 2);
		}

		public static Vector4 WithNegateXw(this Vector4 vector)
		{
			return vector.WithNegate(0, 3);
		}

		public static Vector4 WithNegateYZ(this Vector4 vector)
		{
			return vector.WithNegate(1, 2);
		}

		public static Vector4 WithNegateYw(this Vector4 vector)
		{
			return vector.WithNegate(1, 3);
		}

		public static Vector4 WithNegate(this Vector4 vector, int axis1, int axis2, int axis3)
		{
			vector[axis1] = 0f - vector[axis1];
			vector[axis2] = 0f - vector[axis2];
			vector[axis3] = 0f - vector[axis3];
			return vector;
		}

		public static Vector4 WithNegateXYZ(this Vector4 vector)
		{
			return vector.WithNegate(0, 1, 2);
		}

		public static Vector4 WithNegateXyw(this Vector4 vector)
		{
			return vector.WithNegate(0, 1, 3);
		}

		public static Vector4 WithNegateXzw(this Vector4 vector)
		{
			return vector.WithNegate(0, 2, 3);
		}

		public static Vector4 WithNegateYzw(this Vector4 vector)
		{
			return vector.WithNegate(1, 2, 3);
		}

		public static Vector4 Negate(this Vector4 vector)
		{
			return new Vector4(0f - vector.x, 0f - vector.y, 0f - vector.z, 0f - vector.w);
		}

		public static Vector2 Get(this Vector4 vector, int axis1, int axis2)
		{
			return new Vector2(vector[axis1], vector[axis2]);
		}

		public static Vector2 GetXY(this Vector4 vector)
		{
			return vector.Get(0, 1);
		}

		public static Vector2 GetXZ(this Vector4 vector)
		{
			return vector.Get(0, 2);
		}

		public static Vector2 GetXw(this Vector4 vector)
		{
			return vector.Get(0, 3);
		}

		public static Vector2 GetYx(this Vector4 vector)
		{
			return vector.Get(1, 0);
		}

		public static Vector2 GetYZ(this Vector4 vector)
		{
			return vector.Get(1, 2);
		}

		public static Vector2 GetYw(this Vector4 vector)
		{
			return vector.Get(1, 3);
		}

		public static Vector2 GetZx(this Vector4 vector)
		{
			return vector.Get(2, 0);
		}

		public static Vector2 GetZy(this Vector4 vector)
		{
			return vector.Get(2, 1);
		}

		public static Vector2 GetZw(this Vector4 vector)
		{
			return vector.Get(2, 3);
		}

		public static Vector2 GetWx(this Vector4 vector)
		{
			return vector.Get(3, 0);
		}

		public static Vector2 GetWy(this Vector4 vector)
		{
			return vector.Get(3, 1);
		}

		public static Vector2 GetWz(this Vector4 vector)
		{
			return vector.Get(3, 2);
		}

		public static Vector3 Get(this Vector4 vector, int axis1, int axis2, int axis3)
		{
			return new Vector3(vector[axis1], vector[axis2], vector[axis3]);
		}

		public static Vector3 GetXYZ(this Vector4 vector)
		{
			return vector.Get(0, 1, 2);
		}

		public static Vector3 GetXyw(this Vector4 vector)
		{
			return vector.Get(0, 1, 3);
		}

		public static Vector3 GetXZY(this Vector4 vector)
		{
			return vector.Get(0, 2, 1);
		}

		public static Vector3 GetXzw(this Vector4 vector)
		{
			return vector.Get(0, 2, 3);
		}

		public static Vector3 GetXwy(this Vector4 vector)
		{
			return vector.Get(0, 3, 1);
		}

		public static Vector3 GetXwz(this Vector4 vector)
		{
			return vector.Get(0, 3, 2);
		}

		public static Vector3 GetYXZ(this Vector4 vector)
		{
			return vector.Get(1, 0, 2);
		}

		public static Vector3 GetYxw(this Vector4 vector)
		{
			return vector.Get(1, 0, 3);
		}

		public static Vector3 GetYZX(this Vector4 vector)
		{
			return vector.Get(1, 2, 0);
		}

		public static Vector3 GetYzw(this Vector4 vector)
		{
			return vector.Get(1, 2, 3);
		}

		public static Vector3 GetYwx(this Vector4 vector)
		{
			return vector.Get(1, 3, 0);
		}

		public static Vector3 GetYwz(this Vector4 vector)
		{
			return vector.Get(1, 3, 2);
		}

		public static Vector3 GetZXY(this Vector4 vector)
		{
			return vector.Get(2, 0, 1);
		}

		public static Vector3 GetZxw(this Vector4 vector)
		{
			return vector.Get(2, 0, 3);
		}

		public static Vector3 GetZYX(this Vector4 vector)
		{
			return vector.Get(2, 1, 0);
		}

		public static Vector3 GetZyw(this Vector4 vector)
		{
			return vector.Get(2, 1, 3);
		}

		public static Vector3 GetZwx(this Vector4 vector)
		{
			return vector.Get(2, 3, 0);
		}

		public static Vector3 GetZwy(this Vector4 vector)
		{
			return vector.Get(2, 3, 1);
		}

		public static Vector3 GetWxy(this Vector4 vector)
		{
			return vector.Get(3, 0, 1);
		}

		public static Vector3 GetWxz(this Vector4 vector)
		{
			return vector.Get(3, 0, 2);
		}

		public static Vector3 GetWyx(this Vector4 vector)
		{
			return vector.Get(3, 1, 0);
		}

		public static Vector3 GetWyz(this Vector4 vector)
		{
			return vector.Get(3, 1, 2);
		}

		public static Vector3 GetWzx(this Vector4 vector)
		{
			return vector.Get(3, 2, 0);
		}

		public static Vector3 GetWzy(this Vector4 vector)
		{
			return vector.Get(3, 2, 1);
		}

		public static Vector4 Get(this Vector4 vector, int axis1, int axis2, int axis3, int axis4)
		{
			return new Vector4(vector[axis1], vector[axis2], vector[axis3], vector[axis4]);
		}

		public static Vector4 GetXywz(this Vector4 vector)
		{
			return vector.Get(0, 1, 3, 2);
		}

		public static Vector4 GetXzyw(this Vector4 vector)
		{
			return vector.Get(0, 2, 1, 3);
		}

		public static Vector4 GetXzwy(this Vector4 vector)
		{
			return vector.Get(0, 2, 3, 1);
		}

		public static Vector4 GetXwyz(this Vector4 vector)
		{
			return vector.Get(0, 3, 1, 2);
		}

		public static Vector4 GetXwzy(this Vector4 vector)
		{
			return vector.Get(0, 3, 2, 1);
		}

		public static Vector4 GetYxzw(this Vector4 vector)
		{
			return vector.Get(1, 0, 2, 3);
		}

		public static Vector4 GetYxwz(this Vector4 vector)
		{
			return vector.Get(1, 0, 3, 2);
		}

		public static Vector4 GetYzxw(this Vector4 vector)
		{
			return vector.Get(1, 2, 0, 3);
		}

		public static Vector4 GetYzwx(this Vector4 vector)
		{
			return vector.Get(1, 2, 3, 0);
		}

		public static Vector4 GetYwxz(this Vector4 vector)
		{
			return vector.Get(1, 3, 0, 2);
		}

		public static Vector4 GetYwzx(this Vector4 vector)
		{
			return vector.Get(1, 3, 2, 0);
		}

		public static Vector4 GetZxyw(this Vector4 vector)
		{
			return vector.Get(2, 0, 1, 3);
		}

		public static Vector4 GetZxwy(this Vector4 vector)
		{
			return vector.Get(2, 0, 3, 1);
		}

		public static Vector4 GetZyxw(this Vector4 vector)
		{
			return vector.Get(2, 1, 0, 3);
		}

		public static Vector4 GetZywx(this Vector4 vector)
		{
			return vector.Get(2, 1, 3, 0);
		}

		public static Vector4 GetZwxy(this Vector4 vector)
		{
			return vector.Get(2, 3, 0, 1);
		}

		public static Vector4 GetZwyx(this Vector4 vector)
		{
			return vector.Get(2, 3, 1, 0);
		}

		public static Vector4 GetWxyz(this Vector4 vector)
		{
			return vector.Get(3, 0, 1, 2);
		}

		public static Vector4 GetWxzy(this Vector4 vector)
		{
			return vector.Get(3, 0, 2, 1);
		}

		public static Vector4 GetWyxz(this Vector4 vector)
		{
			return vector.Get(3, 1, 0, 2);
		}

		public static Vector4 GetWyzx(this Vector4 vector)
		{
			return vector.Get(3, 1, 2, 0);
		}

		public static Vector4 GetWzxy(this Vector4 vector)
		{
			return vector.Get(3, 2, 0, 1);
		}

		public static Vector4 GetWzyx(this Vector4 vector)
		{
			return vector.Get(3, 2, 1, 0);
		}

		private static void Compare(Vector4 vector, ref int index, int compareIndex, int result)
		{
			if (vector[compareIndex].CompareTo(vector[index]) == result)
			{
				index = compareIndex;
			}
		}

		private static int CompareAllComponents(Vector4 vector, int result)
		{
			int index = 0;
			Compare(vector, ref index, 1, result);
			Compare(vector, ref index, 2, result);
			Compare(vector, ref index, 3, result);
			return index;
		}

		public static (int index, float value) MaxComponent(this Vector4 vector)
		{
			int num = CompareAllComponents(vector, 1);
			return (index: num, value: vector[num]);
		}

		public static (int index, float value) MinComponent(this Vector4 vector)
		{
			int num = CompareAllComponents(vector, -1);
			return (index: num, value: vector[num]);
		}

		public static Vector4 Remap(this Vector4 vector, float min1, float max1, float min2, float max2)
		{
			return new Vector4(vector.x.Remap(min1, max1, min2, max2), vector.y.Remap(min1, max1, min2, max2), vector.z.Remap(min1, max1, min2, max2), vector.w.Remap(min1, max1, min2, max2));
		}

		public static Vector4 Abs(this Vector4 vector)
		{
			return new Vector4(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z), Mathf.Abs(vector.w));
		}

		public static Vector4 Clamp(this Vector4 vector, float min, float max)
		{
			return new Vector4(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max), Mathf.Clamp(vector.w, min, max));
		}

		public static Vector4 Clamp01(this Vector4 vector)
		{
			return new Vector4(Mathf.Clamp01(vector.x), Mathf.Clamp01(vector.y), Mathf.Clamp01(vector.z), Mathf.Clamp01(vector.w));
		}

		public static Vector4 Divide(this Vector4 vector, Vector4 other)
		{
			return new Vector4(vector.x / other.x, vector.y / other.y, vector.z / other.z, vector.w / other.w);
		}

		public static bool IsNaN(this Vector4 vector)
		{
			if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z))
			{
				return float.IsNaN(vector.w);
			}
			return true;
		}

		public static bool IsUniform(this Vector4 vector)
		{
			if (vector.x.Approximately(vector.y) && vector.y.Approximately(vector.z))
			{
				return vector.z.Approximately(vector.w);
			}
			return false;
		}
	}
}
