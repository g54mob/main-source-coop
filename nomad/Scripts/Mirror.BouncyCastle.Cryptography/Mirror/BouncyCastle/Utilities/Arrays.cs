using System;
using System.Runtime.CompilerServices;

namespace Mirror.BouncyCastle.Utilities
{
	public static class Arrays
	{
		public static readonly byte[] EmptyBytes = new byte[0];

		public static readonly int[] EmptyInts = new int[0];

		public static bool AreEqual(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			return HaveSameContents(a, b);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool FixedTimeEquals(byte[] a, byte[] b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 |= a[i] ^ b[i];
			}
			return num2 == 0;
		}

		public static bool AreEqual(int[] a, int[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			return HaveSameContents(a, b);
		}

		private static bool HaveSameContents(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		private static bool HaveSameContents(int[] a, int[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		public static int GetHashCode(byte[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[num];
			}
			return num2;
		}

		public static int GetHashCode(byte[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[off + num];
			}
			return num2;
		}

		public static int GetHashCode(int[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[num];
			}
			return num2;
		}

		[CLSCompliant(false)]
		public static int GetHashCode(uint[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[off + num];
			}
			return num2;
		}

		[CLSCompliant(false)]
		public static int GetHashCode(ulong[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				ulong num3 = data[off + num];
				num2 *= 257;
				num2 ^= (int)num3;
				num2 *= 257;
				num2 ^= (int)(num3 >> 32);
			}
			return num2;
		}

		public static byte[] Clone(byte[] data)
		{
			if (data != null)
			{
				return (byte[])data.Clone();
			}
			return null;
		}

		public static int[] Clone(int[] data)
		{
			if (data != null)
			{
				return (int[])data.Clone();
			}
			return null;
		}

		[CLSCompliant(false)]
		public static uint[] Clone(uint[] data)
		{
			if (data != null)
			{
				return (uint[])data.Clone();
			}
			return null;
		}

		[CLSCompliant(false)]
		public static ulong[] Clone(ulong[] data)
		{
			if (data != null)
			{
				return (ulong[])data.Clone();
			}
			return null;
		}

		public static void Fill(byte[] buf, byte b)
		{
			int num = buf.Length;
			while (num > 0)
			{
				buf[--num] = b;
			}
		}

		public static void Fill(byte[] buf, int from, int to, byte b)
		{
			for (int i = from; i < to; i++)
			{
				buf[i] = b;
			}
		}

		[CLSCompliant(false)]
		public static uint[] CopyOf(uint[] data, int newLength)
		{
			uint[] array = new uint[newLength];
			Array.Copy(data, 0, array, 0, System.Math.Min(newLength, data.Length));
			return array;
		}

		public static byte[] CopyOfRange(byte[] data, int from, int to)
		{
			int length = GetLength(from, to);
			byte[] array = new byte[length];
			Array.Copy(data, from, array, 0, System.Math.Min(length, data.Length - from));
			return array;
		}

		private static int GetLength(int from, int to)
		{
			int num = to - from;
			if (num < 0)
			{
				throw new ArgumentException(from + " > " + to);
			}
			return num;
		}

		public static byte[] Concatenate(byte[] a, byte[] b)
		{
			if (a == null)
			{
				return Clone(b);
			}
			if (b == null)
			{
				return Clone(a);
			}
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		public static byte[] Prepend(byte[] a, byte b)
		{
			if (a == null)
			{
				return new byte[1] { b };
			}
			int num = a.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		public static bool IsNullOrContainsNull(object[] array)
		{
			if (array == null)
			{
				return true;
			}
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (array[i] == null)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsNullOrEmpty(byte[] array)
		{
			if (array != null)
			{
				return array.Length < 1;
			}
			return true;
		}
	}
}
