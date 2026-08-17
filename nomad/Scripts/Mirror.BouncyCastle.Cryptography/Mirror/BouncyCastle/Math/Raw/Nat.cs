using System;
using Mirror.BouncyCastle.Crypto.Utilities;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Math.Raw
{
	internal static class Nat
	{
		public static uint Add(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0uL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)((long)x[i] + (long)y[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		public static uint Add33To(int len, uint x, uint[] z)
		{
			ulong num = (ulong)z[0] + (ulong)x;
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)((long)z[1] + 1L);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return IncAt(len, z, 2);
			}
			return 0u;
		}

		public static uint AddBothTo(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0uL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)((long)x[i] + (long)y[i] + z[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		public static uint AddTo(int len, uint[] x, uint[] z)
		{
			ulong num = 0uL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)((long)x[i] + (long)z[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			ulong num = 0uL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)((long)x[xOff + i] + (long)z[zOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		public static uint AddWordAt(int len, uint x, uint[] z, int zPos)
		{
			ulong num = (ulong)x + (ulong)z[zPos];
			z[zPos] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return IncAt(len, z, zPos + 1);
			}
			return 0u;
		}

		public static uint AddWordTo(int len, uint x, uint[] z)
		{
			ulong num = (ulong)x + (ulong)z[0];
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return IncAt(len, z, 1);
			}
			return 0u;
		}

		public static uint CAdd(int len, int mask, uint[] x, uint[] y, uint[] z)
		{
			uint num = (uint)(-(mask & 1));
			ulong num2 = 0uL;
			for (int i = 0; i < len; i++)
			{
				num2 += (ulong)((long)x[i] + (long)(y[i] & num));
				z[i] = (uint)num2;
				num2 >>= 32;
			}
			return (uint)num2;
		}

		public static uint CAddTo(int len, int mask, uint[] x, uint[] z)
		{
			uint num = (uint)(-(mask & 1));
			ulong num2 = 0uL;
			for (int i = 0; i < len; i++)
			{
				num2 += (ulong)((long)z[i] + (long)(x[i] & num));
				z[i] = (uint)num2;
				num2 >>= 32;
			}
			return (uint)num2;
		}

		public static int Compare(int len, uint[] x, uint[] y)
		{
			for (int num = len - 1; num >= 0; num--)
			{
				uint num2 = x[num];
				uint num3 = y[num];
				if (num2 < num3)
				{
					return -1;
				}
				if (num2 > num3)
				{
					return 1;
				}
			}
			return 0;
		}

		public static void Copy(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			Array.Copy(x, xOff, z, zOff, len);
		}

		public static uint[] Create(int len)
		{
			return new uint[len];
		}

		public static ulong[] Create64(int len)
		{
			return new ulong[len];
		}

		public static int Dec(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				if (--z[i] != 4294967295u)
				{
					return 0;
				}
			}
			return -1;
		}

		public static int DecAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				if (--z[i] != 4294967295u)
				{
					return 0;
				}
			}
			return -1;
		}

		public static bool Eq(int len, uint[] x, uint[] y)
		{
			for (int num = len - 1; num >= 0; num--)
			{
				if (x[num] != y[num])
				{
					return false;
				}
			}
			return true;
		}

		public static uint[] FromBigInteger(int bits, BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > bits)
			{
				throw new ArgumentException();
			}
			int lengthForBits = GetLengthForBits(bits);
			uint[] array = Create(lengthForBits);
			array[0] = (uint)x.IntValue;
			for (int i = 1; i < lengthForBits; i++)
			{
				x = x.ShiftRight(32);
				array[i] = (uint)x.IntValue;
			}
			return array;
		}

		public static ulong[] FromBigInteger64(int bits, BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > bits)
			{
				throw new ArgumentException();
			}
			int lengthForBits = GetLengthForBits64(bits);
			ulong[] array = Create64(lengthForBits);
			array[0] = (ulong)x.LongValue;
			for (int i = 1; i < lengthForBits; i++)
			{
				x = x.ShiftRight(64);
				array[i] = (ulong)x.LongValue;
			}
			return array;
		}

		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1;
			}
			int num = bit >> 5;
			if (num < 0 || num >= x.Length)
			{
				return 0u;
			}
			int num2 = bit & 0x1F;
			return (x[num] >> num2) & 1;
		}

		public static int GetBitLength(int len, uint[] x)
		{
			for (int num = len - 1; num >= 0; num--)
			{
				uint num2 = x[num];
				if (num2 != 0)
				{
					return num * 32 + 32 - Integers.NumberOfLeadingZeros((int)num2);
				}
			}
			return 0;
		}

		public static int GetLengthForBits(int bits)
		{
			if (bits < 1)
			{
				throw new ArgumentException();
			}
			return (int)((uint)(bits + 31) >> 5);
		}

		public static int GetLengthForBits64(int bits)
		{
			if (bits < 1)
			{
				throw new ArgumentException();
			}
			return (int)((uint)(bits + 63) >> 6);
		}

		public static bool Gte(int len, uint[] x, uint[] y)
		{
			for (int num = len - 1; num >= 0; num--)
			{
				uint num2 = x[num];
				uint num3 = y[num];
				if (num2 < num3)
				{
					return false;
				}
				if (num2 > num3)
				{
					return true;
				}
			}
			return true;
		}

		public static uint Inc(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				if (++z[i] != 0)
				{
					return 0u;
				}
			}
			return 1u;
		}

		public static uint Inc(int len, uint[] x, uint[] z)
		{
			int i = 0;
			while (i < len)
			{
				uint num = (z[i] = x[i] + 1);
				i++;
				if (num != 0)
				{
					for (; i < len; i++)
					{
						z[i] = x[i];
					}
					return 0u;
				}
			}
			return 1u;
		}

		public static uint IncAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				if (++z[i] != 0)
				{
					return 0u;
				}
			}
			return 1u;
		}

		public static uint IncAt(int len, uint[] z, int zOff, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				if (++z[zOff + i] != 0)
				{
					return 0u;
				}
			}
			return 1u;
		}

		public static bool IsOne(int len, uint[] x)
		{
			if (x[0] != 1)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsZero(int len, uint[] x)
		{
			if (x[0] != 0)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public static int LessThan(int len, uint[] x, uint[] y)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)x[i] - (long)y[i];
				num >>= 32;
			}
			return (int)num;
		}

		public static uint Mul31BothAdd(int len, uint a, uint[] x, uint b, uint[] y, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = a;
			ulong num3 = b;
			int num4 = 0;
			do
			{
				num += num2 * x[num4] + num3 * y[num4] + z[zOff + num4];
				z[zOff + num4] = (uint)num;
				num >>= 32;
			}
			while (++num4 < len);
			return (uint)num;
		}

		public static uint MulWordAddTo(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = x;
			int num3 = 0;
			do
			{
				num += num2 * y[yOff + num3] + z[zOff + num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		public static uint ShiftDownBit(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> 1) | (c << 31);
				c = num2;
			}
			return c << 31;
		}

		public static uint ShiftDownBits(int len, uint[] z, int bits, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> bits) | (c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		public static uint ShiftDownBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[xOff + num];
				z[zOff + num] = (num2 >> bits) | (c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		public static uint ShiftDownWord(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = c;
				c = num2;
			}
			return c;
		}

		public static uint ShiftUpBit(int len, uint[] x, uint c, uint[] z)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				uint num2 = x[i];
				uint num3 = x[i + 1];
				uint num4 = x[i + 2];
				uint num5 = x[i + 3];
				z[i] = (num2 << 1) | (c >> 31);
				z[i + 1] = (num3 << 1) | (num2 >> 31);
				z[i + 2] = (num4 << 1) | (num3 >> 31);
				z[i + 3] = (num5 << 1) | (num4 >> 31);
				c = num5;
			}
			for (; i < len; i++)
			{
				uint num6 = x[i];
				z[i] = (num6 << 1) | (c >> 31);
				c = num6;
			}
			return c >> 31;
		}

		public static ulong ShiftUpBit64(int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				ulong num2 = x[xOff + i];
				ulong num3 = x[xOff + i + 1];
				ulong num4 = x[xOff + i + 2];
				ulong num5 = x[xOff + i + 3];
				z[zOff + i] = (num2 << 1) | (c >> 63);
				z[zOff + i + 1] = (num3 << 1) | (num2 >> 63);
				z[zOff + i + 2] = (num4 << 1) | (num3 >> 63);
				z[zOff + i + 3] = (num5 << 1) | (num4 >> 63);
				c = num5;
			}
			for (; i < len; i++)
			{
				ulong num6 = x[xOff + i];
				z[zOff + i] = (num6 << 1) | (c >> 63);
				c = num6;
			}
			return c >> 63;
		}

		public static uint ShiftUpBits(int len, uint[] z, int bits, uint c)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				uint num2 = z[i];
				uint num3 = z[i + 1];
				uint num4 = z[i + 2];
				uint num5 = z[i + 3];
				z[i] = (num2 << bits) | (c >> -bits);
				z[i + 1] = (num3 << bits) | (num2 >> -bits);
				z[i + 2] = (num4 << bits) | (num3 >> -bits);
				z[i + 3] = (num5 << bits) | (num4 >> -bits);
				c = num5;
			}
			for (; i < len; i++)
			{
				uint num6 = z[i];
				z[i] = (num6 << bits) | (c >> -bits);
				c = num6;
			}
			return c >> -bits;
		}

		public static uint ShiftUpBits(int len, uint[] x, int bits, uint c, uint[] z)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				uint num2 = x[i];
				uint num3 = x[i + 1];
				uint num4 = x[i + 2];
				uint num5 = x[i + 3];
				z[i] = (num2 << bits) | (c >> -bits);
				z[i + 1] = (num3 << bits) | (num2 >> -bits);
				z[i + 2] = (num4 << bits) | (num3 >> -bits);
				z[i + 3] = (num5 << bits) | (num4 >> -bits);
				c = num5;
			}
			for (; i < len; i++)
			{
				uint num6 = x[i];
				z[i] = (num6 << bits) | (c >> -bits);
				c = num6;
			}
			return c >> -bits;
		}

		public static ulong ShiftUpBits64(int len, ulong[] z, int zOff, int bits, ulong c)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				ulong num2 = z[zOff + i];
				ulong num3 = z[zOff + i + 1];
				ulong num4 = z[zOff + i + 2];
				ulong num5 = z[zOff + i + 3];
				z[zOff + i] = (num2 << bits) | (c >> -bits);
				z[zOff + i + 1] = (num3 << bits) | (num2 >> -bits);
				z[zOff + i + 2] = (num4 << bits) | (num3 >> -bits);
				z[zOff + i + 3] = (num5 << bits) | (num4 >> -bits);
				c = num5;
			}
			for (; i < len; i++)
			{
				ulong num6 = z[zOff + i];
				z[zOff + i] = (num6 << bits) | (c >> -bits);
				c = num6;
			}
			return c >> -bits;
		}

		public static ulong ShiftUpBits64(int len, ulong[] x, int xOff, int bits, ulong c, ulong[] z, int zOff)
		{
			int i = 0;
			for (int num = len - 4; i <= num; i += 4)
			{
				ulong num2 = x[xOff + i];
				ulong num3 = x[xOff + i + 1];
				ulong num4 = x[xOff + i + 2];
				ulong num5 = x[xOff + i + 3];
				z[zOff + i] = (num2 << bits) | (c >> -bits);
				z[zOff + i + 1] = (num3 << bits) | (num2 >> -bits);
				z[zOff + i + 2] = (num4 << bits) | (num3 >> -bits);
				z[zOff + i + 3] = (num5 << bits) | (num4 >> -bits);
				c = num5;
			}
			for (; i < len; i++)
			{
				ulong num6 = x[xOff + i];
				z[zOff + i] = (num6 << bits) | (c >> -bits);
				c = num6;
			}
			return c >> -bits;
		}

		public static int Sub(int len, uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)x[i] - (long)y[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		public static int Sub33From(int len, uint x, uint[] z)
		{
			long num = (long)z[0] - (long)x;
			z[0] = (uint)num;
			num >>= 32;
			num += (long)z[1] - 1L;
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return DecAt(len, z, 2);
			}
			return 0;
		}

		public static int SubFrom(int len, uint[] x, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)z[i] - (long)x[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		public static int SubFrom(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)z[zOff + i] - (long)x[xOff + i];
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		public static int SubWordFrom(int len, uint x, uint[] z)
		{
			long num = (long)z[0] - (long)x;
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return DecAt(len, z, 1);
			}
			return 0;
		}

		public static BigInteger ToBigInteger(int len, uint[] x)
		{
			byte[] array = new byte[len << 2];
			int num = len;
			int num2 = 0;
			while (--num >= 0)
			{
				Pack.UInt32_To_BE(x[num], array, num2);
				num2 += 4;
			}
			return new BigInteger(1, array);
		}

		public static void Xor64(int len, ulong[] x, ulong[] y, ulong[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] = x[i] ^ y[i];
			}
		}

		public static void Xor64(int len, ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				z[zOff + i] = x[xOff + i] ^ y[yOff + i];
			}
		}

		public static void XorTo64(int len, ulong[] x, ulong[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] ^= x[i];
			}
		}

		public static void XorTo64(int len, ulong[] x, int xOff, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				z[zOff + i] ^= x[xOff + i];
			}
		}

		public static void Zero(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] = 0u;
			}
		}
	}
}
