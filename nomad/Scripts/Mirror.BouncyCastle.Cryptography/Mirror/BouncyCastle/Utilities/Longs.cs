using System;
using Mirror.BouncyCastle.Math.Raw;

namespace Mirror.BouncyCastle.Utilities
{
	public static class Longs
	{
		private static readonly byte[] DeBruijnTZ = new byte[64]
		{
			63, 0, 1, 52, 2, 6, 53, 26, 3, 37,
			40, 7, 33, 54, 47, 27, 61, 4, 38, 45,
			43, 41, 21, 8, 23, 34, 58, 55, 48, 17,
			28, 10, 62, 51, 5, 25, 36, 39, 32, 46,
			60, 44, 42, 20, 22, 57, 16, 9, 50, 24,
			35, 31, 59, 19, 56, 15, 49, 30, 18, 14,
			29, 13, 12, 11
		};

		public static int NumberOfLeadingZeros(long i)
		{
			int num = (int)(i >> 32);
			int num2 = 0;
			if (num == 0)
			{
				num2 = 32;
				num = (int)i;
			}
			return num2 + Integers.NumberOfLeadingZeros(num);
		}

		[CLSCompliant(false)]
		public static ulong Reverse(ulong i)
		{
			i = Bits.BitPermuteStepSimple(i, 6148914691236517205uL, 1);
			i = Bits.BitPermuteStepSimple(i, 3689348814741910323uL, 2);
			i = Bits.BitPermuteStepSimple(i, 1085102592571150095uL, 4);
			return ReverseBytes(i);
		}

		[CLSCompliant(false)]
		public static ulong ReverseBytes(ulong i)
		{
			return RotateLeft(i & 0xFF000000FF000000uL, 8) | RotateLeft(i & 0xFF000000FF0000L, 24) | RotateLeft(i & 0xFF000000FF00L, 40) | RotateLeft(i & 0xFF000000FFL, 56);
		}

		[CLSCompliant(false)]
		public static ulong RotateLeft(ulong i, int distance)
		{
			return (i << distance) | (i >> -distance);
		}
	}
}
