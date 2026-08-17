using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	public class IntVector3
	{
		public int x;

		public int y;

		public int z;

		public IntVector3()
		{
			x = 0;
			y = 0;
			z = 0;
		}

		public IntVector3(int P_0, int P_1, int P_2)
		{
			x = P_0;
			y = P_1;
			z = P_2;
		}

		public IntVector3 Clone()
		{
			return new IntVector3(x, y, z);
		}

		public static IntVector3 operator +(IntVector3 value1, IntVector3 value2)
		{
			return new IntVector3(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z);
		}

		public static IntVector3 operator -(IntVector3 value1, IntVector3 value2)
		{
			return new IntVector3(value1.x - value2.x, value1.y - value2.y, value1.z - value2.z);
		}

		public static IntVector3 operator *(IntVector3 value1, IntVector3 value2)
		{
			return new IntVector3(value1.x * value2.x, value1.y * value2.y, value1.z * value2.z);
		}

		public static IntVector3 operator /(IntVector3 value1, IntVector3 value2)
		{
			return new IntVector3(value1.x / value2.x, value1.y / value2.y, value1.z / value2.z);
		}

		public static IntVector3 operator +(IntVector3 value1, int value2)
		{
			return new IntVector3(value1.x + value2, value1.y + value2, value1.z + value2);
		}

		public static IntVector3 operator -(IntVector3 value1, int value2)
		{
			return new IntVector3(value1.x - value2, value1.y - value2, value1.z - value2);
		}

		public static IntVector3 operator *(IntVector3 value1, int value2)
		{
			return new IntVector3(value1.x * value2, value1.y * value2, value1.z * value2);
		}

		public static IntVector3 operator /(IntVector3 value1, int value2)
		{
			return new IntVector3(value1.x / value2, value1.y / value2, value1.z / value2);
		}

		public static Vector3 operator +(IntVector3 value1, float value2)
		{
			return new Vector3((float)value1.x + value2, (float)value1.y + value2, (float)value1.z + value2);
		}

		public static Vector3 operator -(IntVector3 value1, float value2)
		{
			return new Vector3((float)value1.x - value2, (float)value1.y - value2, (float)value1.z - value2);
		}

		public static Vector3 operator *(IntVector3 value1, float value2)
		{
			return new Vector3((float)value1.x * value2, (float)value1.y * value2, (float)value1.z * value2);
		}

		public static Vector3 operator /(IntVector3 value1, float value2)
		{
			return new Vector3((float)value1.x / value2, (float)value1.y / value2, (float)value1.z / value2);
		}
	}
}
