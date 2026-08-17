using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	public class IntVector4
	{
		public int x;

		public int y;

		public int z;

		public int q;

		public IntVector4()
		{
			x = 0;
			y = 0;
			z = 0;
			q = 0;
		}

		public IntVector4(int P_0, int P_1, int P_2, int P_3)
		{
			x = P_0;
			y = P_1;
			z = P_2;
			q = P_3;
		}

		public IntVector4 Clone()
		{
			return new IntVector4(x, y, z, q);
		}

		public static IntVector4 operator +(IntVector4 value1, IntVector4 value2)
		{
			return new IntVector4(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z, value1.q + value2.q);
		}

		public static IntVector4 operator -(IntVector4 value1, IntVector4 value2)
		{
			return new IntVector4(value1.x - value2.x, value1.y - value2.y, value1.z - value2.z, value1.q - value2.q);
		}

		public static IntVector4 operator *(IntVector4 value1, IntVector4 value2)
		{
			return new IntVector4(value1.x * value2.x, value1.y * value2.y, value1.z * value2.z, value1.q * value2.q);
		}

		public static IntVector4 operator /(IntVector4 value1, IntVector4 value2)
		{
			return new IntVector4(value1.x / value2.x, value1.y / value2.y, value1.z / value2.z, value1.q / value2.q);
		}

		public static IntVector4 operator +(IntVector4 value1, int value2)
		{
			return new IntVector4(value1.x + value2, value1.y + value2, value1.z + value2, value1.q + value2);
		}

		public static IntVector4 operator -(IntVector4 value1, int value2)
		{
			return new IntVector4(value1.x - value2, value1.y - value2, value1.z - value2, value1.q - value2);
		}

		public static IntVector4 operator *(IntVector4 value1, int value2)
		{
			return new IntVector4(value1.x * value2, value1.y * value2, value1.z * value2, value1.q * value2);
		}

		public static IntVector4 operator /(IntVector4 value1, int value2)
		{
			return new IntVector4(value1.x / value2, value1.y / value2, value1.z / value2, value1.q / value2);
		}

		public static Vector4 operator +(IntVector4 value1, float value2)
		{
			return new Vector4((float)value1.x + value2, (float)value1.y + value2, (float)value1.z + value2, (float)value1.q + value2);
		}

		public static Vector4 operator -(IntVector4 value1, float value2)
		{
			return new Vector4((float)value1.x - value2, (float)value1.y - value2, (float)value1.z - value2, (float)value1.q - value2);
		}

		public static Vector4 operator *(IntVector4 value1, float value2)
		{
			return new Vector4((float)value1.x * value2, (float)value1.y * value2, (float)value1.z * value2, (float)value1.q * value2);
		}

		public static Vector4 operator /(IntVector4 value1, float value2)
		{
			return new Vector4((float)value1.x / value2, (float)value1.y / value2, (float)value1.z / value2, (float)value1.q / value2);
		}
	}
}
