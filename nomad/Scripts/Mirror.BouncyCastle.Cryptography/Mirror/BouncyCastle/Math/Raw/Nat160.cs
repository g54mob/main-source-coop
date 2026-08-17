using Mirror.BouncyCastle.Crypto.Utilities;

namespace Mirror.BouncyCastle.Math.Raw
{
	internal static class Nat160
	{
		public static uint Add(uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0uL;
			num += (ulong)((long)x[0] + (long)y[0]);
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[1] + (long)y[1]);
			z[1] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[2] + (long)y[2]);
			z[2] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[3] + (long)y[3]);
			z[3] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[4] + (long)y[4]);
			z[4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		public static uint AddBothTo(uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0uL;
			num += (ulong)((long)x[0] + (long)y[0] + z[0]);
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[1] + (long)y[1] + z[1]);
			z[1] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[2] + (long)y[2] + z[2]);
			z[2] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[3] + (long)y[3] + z[3]);
			z[3] = (uint)num;
			num >>= 32;
			num += (ulong)((long)x[4] + (long)y[4] + z[4]);
			z[4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		public static void Copy(uint[] x, int xOff, uint[] z, int zOff)
		{
			z[zOff] = x[xOff];
			z[zOff + 1] = x[xOff + 1];
			z[zOff + 2] = x[xOff + 2];
			z[zOff + 3] = x[xOff + 3];
			z[zOff + 4] = x[xOff + 4];
		}

		public static uint[] Create()
		{
			return new uint[5];
		}

		public static uint[] CreateExt()
		{
			return new uint[10];
		}

		public static bool Eq(uint[] x, uint[] y)
		{
			for (int num = 4; num >= 0; num--)
			{
				if (x[num] != y[num])
				{
					return false;
				}
			}
			return true;
		}

		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1;
			}
			int num = bit >> 5;
			if (num < 0 || num >= 5)
			{
				return 0u;
			}
			int num2 = bit & 0x1F;
			return (x[num] >> num2) & 1;
		}

		public static bool Gte(uint[] x, uint[] y)
		{
			for (int num = 4; num >= 0; num--)
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

		public static bool IsOne(uint[] x)
		{
			if (x[0] != 1)
			{
				return false;
			}
			for (int i = 1; i < 5; i++)
			{
				if (x[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsZero(uint[] x)
		{
			for (int i = 0; i < 5; i++)
			{
				if (x[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public static void Mul(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = y[0];
			ulong num2 = y[1];
			ulong num3 = y[2];
			ulong num4 = y[3];
			ulong num5 = y[4];
			ulong num6 = 0uL;
			ulong num7 = x[0];
			num6 += num7 * num;
			zz[0] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num2;
			zz[1] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num3;
			zz[2] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num4;
			zz[3] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num5;
			zz[4] = (uint)num6;
			num6 >>= 32;
			zz[5] = (uint)num6;
			for (int i = 1; i < 5; i++)
			{
				ulong num8 = 0uL;
				ulong num9 = x[i];
				num8 += num9 * num + zz[i];
				zz[i] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num2 + zz[i + 1];
				zz[i + 1] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num3 + zz[i + 2];
				zz[i + 2] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num4 + zz[i + 3];
				zz[i + 3] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num5 + zz[i + 4];
				zz[i + 4] = (uint)num8;
				num8 >>= 32;
				zz[i + 5] = (uint)num8;
			}
		}

		public static uint MulAddTo(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = y[0];
			ulong num2 = y[1];
			ulong num3 = y[2];
			ulong num4 = y[3];
			ulong num5 = y[4];
			ulong num6 = 0uL;
			for (int i = 0; i < 5; i++)
			{
				ulong num7 = 0uL;
				ulong num8 = x[i];
				num7 += num8 * num + zz[i];
				zz[i] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num2 + zz[i + 1];
				zz[i + 1] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num3 + zz[i + 2];
				zz[i + 2] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num4 + zz[i + 3];
				zz[i + 3] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num5 + zz[i + 4];
				zz[i + 4] = (uint)num7;
				num7 >>= 32;
				num6 += num7 + zz[i + 5];
				zz[i + 5] = (uint)num6;
				num6 >>= 32;
			}
			return (uint)num6;
		}

		public static ulong Mul33Add(uint w, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = w;
			ulong num3 = x[xOff];
			num += num2 * num3 + y[yOff];
			z[zOff] = (uint)num;
			num >>= 32;
			ulong num4 = x[xOff + 1];
			num += num2 * num4 + num3 + y[yOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			ulong num5 = x[xOff + 2];
			num += num2 * num5 + num4 + y[yOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			ulong num6 = x[xOff + 3];
			num += num2 * num6 + num5 + y[yOff + 3];
			z[zOff + 3] = (uint)num;
			num >>= 32;
			ulong num7 = x[xOff + 4];
			num += num2 * num7 + num6 + y[yOff + 4];
			z[zOff + 4] = (uint)num;
			num >>= 32;
			return num + num7;
		}

		public static uint Mul33DWordAdd(uint x, ulong y, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = x;
			ulong num3 = y & 0xFFFFFFFFu;
			num += num2 * num3 + z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			ulong num4 = y >> 32;
			num += num2 * num4 + num3 + z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += num4 + z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			num += z[zOff + 3];
			z[zOff + 3] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.IncAt(5, z, zOff, 4);
			}
			return 0u;
		}

		public static uint Mul33WordAdd(uint x, uint y, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = y;
			num += num2 * x + z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += num2 + z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.IncAt(5, z, zOff, 3);
			}
			return 0u;
		}

		public static uint MulWordsAdd(uint x, uint y, uint[] z, int zOff)
		{
			ulong num = 0uL;
			ulong num2 = x;
			ulong num3 = y;
			num += num3 * num2 + z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.IncAt(5, z, zOff, 2);
			}
			return 0u;
		}

		public static void Square(uint[] x, uint[] zz)
		{
			ulong num = x[0];
			uint num2 = 0u;
			int num3 = 4;
			int num4 = 10;
			do
			{
				long num5 = x[num3--];
				ulong num6 = (ulong)(num5 * num5);
				zz[--num4] = (num2 << 31) | (uint)(int)(num6 >> 33);
				zz[--num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			ulong num7 = num * num;
			ulong num8 = (num2 << 31) | (num7 >> 33);
			zz[0] = (uint)num7;
			num2 = (uint)((int)(num7 >> 32) & 1);
			ulong num9 = x[1];
			ulong num10 = zz[2];
			num8 += num9 * num;
			uint num11 = (uint)num8;
			zz[1] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num10 += num8 >> 32;
			ulong num12 = x[2];
			ulong num13 = zz[3];
			ulong num14 = zz[4];
			num10 += num12 * num;
			num11 = (uint)num10;
			zz[2] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num13 += (num10 >> 32) + num12 * num9;
			num14 += num13 >> 32;
			num13 &= 0xFFFFFFFFu;
			ulong num15 = x[3];
			ulong num16 = zz[5] + (num14 >> 32);
			num14 &= 0xFFFFFFFFu;
			ulong num17 = zz[6] + (num16 >> 32);
			num16 &= 0xFFFFFFFFu;
			num13 += num15 * num;
			num11 = (uint)num13;
			zz[3] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num14 += (num13 >> 32) + num15 * num9;
			num16 += (num14 >> 32) + num15 * num12;
			num14 &= 0xFFFFFFFFu;
			num17 += num16 >> 32;
			num16 &= 0xFFFFFFFFu;
			ulong num18 = x[4];
			ulong num19 = zz[7] + (num17 >> 32);
			num17 &= 0xFFFFFFFFu;
			ulong num20 = zz[8] + (num19 >> 32);
			num19 &= 0xFFFFFFFFu;
			num14 += num18 * num;
			num11 = (uint)num14;
			zz[4] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num16 += (num14 >> 32) + num18 * num9;
			num17 += (num16 >> 32) + num18 * num12;
			num19 += (num17 >> 32) + num18 * num15;
			num20 += num19 >> 32;
			num11 = (uint)num16;
			zz[5] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num11 = (uint)num17;
			zz[6] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num11 = (uint)num19;
			zz[7] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num11 = (uint)num20;
			zz[8] = (num11 << 1) | num2;
			num2 = num11 >> 31;
			num11 = zz[9] + (uint)(int)(num20 >> 32);
			zz[9] = (num11 << 1) | num2;
		}

		public static int Sub(uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			num += (long)x[0] - (long)y[0];
			z[0] = (uint)num;
			num >>= 32;
			num += (long)x[1] - (long)y[1];
			z[1] = (uint)num;
			num >>= 32;
			num += (long)x[2] - (long)y[2];
			z[2] = (uint)num;
			num >>= 32;
			num += (long)x[3] - (long)y[3];
			z[3] = (uint)num;
			num >>= 32;
			num += (long)x[4] - (long)y[4];
			z[4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		public static int SubFrom(uint[] x, uint[] z)
		{
			long num = 0L;
			num += (long)z[0] - (long)x[0];
			z[0] = (uint)num;
			num >>= 32;
			num += (long)z[1] - (long)x[1];
			z[1] = (uint)num;
			num >>= 32;
			num += (long)z[2] - (long)x[2];
			z[2] = (uint)num;
			num >>= 32;
			num += (long)z[3] - (long)x[3];
			z[3] = (uint)num;
			num >>= 32;
			num += (long)z[4] - (long)x[4];
			z[4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		public static BigInteger ToBigInteger(uint[] x)
		{
			byte[] array = new byte[20];
			for (int i = 0; i < 5; i++)
			{
				uint num = x[i];
				if (num != 0)
				{
					Pack.UInt32_To_BE(num, array, 4 - i << 2);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
