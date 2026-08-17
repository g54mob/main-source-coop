using UnityEngine;

namespace EvilCore.Extensions
{
	public static class QuaternionExtensions
	{
		public static Quaternion With(this Quaternion quaternion, int axis, float value)
		{
			quaternion[axis] = value;
			return quaternion;
		}

		public static Quaternion WithX(this Quaternion quaternion, float x)
		{
			return quaternion.With(0, x);
		}

		public static Quaternion WithY(this Quaternion quaternion, float y)
		{
			return quaternion.With(1, y);
		}

		public static Quaternion WithZ(this Quaternion quaternion, float z)
		{
			return quaternion.With(2, z);
		}

		public static Quaternion WithW(this Quaternion quaternion, float w)
		{
			return quaternion.With(3, w);
		}

		public static Quaternion With(this Quaternion quaternion, int axis1, float value1, int axis2, float value2)
		{
			quaternion[axis1] = value1;
			quaternion[axis2] = value2;
			return quaternion;
		}

		public static Quaternion WithXY(this Quaternion quaternion, float x, float y)
		{
			return quaternion.With(0, x, 1, y);
		}

		public static Quaternion WithXY(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(0, values.x, 1, values.y);
		}

		public static Quaternion WithXZ(this Quaternion quaternion, float x, float z)
		{
			return quaternion.With(0, x, 2, z);
		}

		public static Quaternion WithXZ(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(0, values.x, 2, values.y);
		}

		public static Quaternion WithYZ(this Quaternion quaternion, float y, float z)
		{
			return quaternion.With(1, y, 2, z);
		}

		public static Quaternion WithYZ(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(1, values.x, 2, values.y);
		}

		public static Quaternion WithXw(this Quaternion quaternion, float x, float w)
		{
			return quaternion.With(0, x, 3, w);
		}

		public static Quaternion WithXw(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(0, values.x, 3, values.y);
		}

		public static Quaternion WithYw(this Quaternion quaternion, float y, float w)
		{
			return quaternion.With(1, y, 3, w);
		}

		public static Quaternion WithYw(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(1, values.x, 3, values.y);
		}

		public static Quaternion WithZw(this Quaternion quaternion, float z, float w)
		{
			return quaternion.With(2, z, 3, w);
		}

		public static Quaternion WithZw(this Quaternion quaternion, Vector2 values)
		{
			return quaternion.With(2, values.x, 3, values.y);
		}

		public static Quaternion With(this Quaternion quaternion, int axis1, float value1, int axis2, float value2, int axis3, float value3)
		{
			quaternion[axis1] = value1;
			quaternion[axis2] = value2;
			quaternion[axis3] = value3;
			return quaternion;
		}

		public static Quaternion WithXYZ(this Quaternion quaternion, float x, float y, float z)
		{
			return quaternion.With(0, x, 1, y, 2, z);
		}

		public static Quaternion WithXYZ(this Quaternion quaternion, Vector3 values)
		{
			return quaternion.With(0, values.x, 1, values.y, 2, values.z);
		}

		public static Quaternion WithXyw(this Quaternion quaternion, float x, float y, float w)
		{
			return quaternion.With(0, x, 1, y, 3, w);
		}

		public static Quaternion WithXyw(this Quaternion quaternion, Vector3 values)
		{
			return quaternion.With(0, values.x, 1, values.y, 3, values.z);
		}

		public static Quaternion WithXzw(this Quaternion quaternion, float x, float z, float w)
		{
			return quaternion.With(0, x, 2, z, 3, w);
		}

		public static Quaternion WithXzw(this Quaternion quaternion, Vector3 values)
		{
			return quaternion.With(0, values.x, 2, values.y, 3, values.z);
		}

		public static Quaternion WithYzw(this Quaternion quaternion, float y, float z, float w)
		{
			return quaternion.With(1, y, 2, z, 3, w);
		}

		public static Quaternion WithYzw(this Quaternion quaternion, Vector3 values)
		{
			return quaternion.With(1, values.x, 2, values.y, 3, values.z);
		}
	}
}
