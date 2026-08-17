namespace Mirror.BouncyCastle.Math.Raw
{
	internal static class Interleave
	{
		internal static uint Expand8to16(byte x)
		{
			int num = (x | (x << 4)) & 0xF0F;
			int num2 = (num | (num << 2)) & 0x3333;
			return (uint)((num2 | (num2 << 1)) & 0x5555);
		}

		internal static ulong Expand32to64(uint x)
		{
			x = Bits.BitPermuteStep(x, 65280u, 8);
			x = Bits.BitPermuteStep(x, 15728880u, 4);
			x = Bits.BitPermuteStep(x, 202116108u, 2);
			x = Bits.BitPermuteStep(x, 572662306u, 1);
			return (((ulong)(x >> 1) & 0x55555555uL) << 32) | ((ulong)x & 0x55555555uL);
		}

		internal static void Expand64To128(ulong x, ulong[] z, int zOff)
		{
			x = Bits.BitPermuteStep(x, 4294901760uL, 16);
			x = Bits.BitPermuteStep(x, 280375465148160uL, 8);
			x = Bits.BitPermuteStep(x, 67555025218437360uL, 4);
			x = Bits.BitPermuteStep(x, 868082074056920076uL, 2);
			x = Bits.BitPermuteStep(x, 2459565876494606882uL, 1);
			z[zOff] = x & 0x5555555555555555L;
			z[zOff + 1] = (x >> 1) & 0x5555555555555555L;
		}

		internal static void Expand64To128(ulong[] xs, int xsOff, int xsLen, ulong[] zs, int zsOff)
		{
			int num = xsLen;
			int num2 = zsOff + (xsLen << 1);
			while (--num >= 0)
			{
				num2 -= 2;
				Expand64To128(xs[xsOff + num], zs, num2);
			}
		}

		internal static ulong Expand64To128Rev(ulong x, out ulong low)
		{
			x = Bits.BitPermuteStep(x, 4294901760uL, 16);
			x = Bits.BitPermuteStep(x, 280375465148160uL, 8);
			x = Bits.BitPermuteStep(x, 67555025218437360uL, 4);
			x = Bits.BitPermuteStep(x, 868082074056920076uL, 2);
			x = Bits.BitPermuteStep(x, 2459565876494606882uL, 1);
			low = x & 0xAAAAAAAAAAAAAAAAuL;
			return (x << 1) & 0xAAAAAAAAAAAAAAAAuL;
		}

		internal static uint Shuffle2(uint x)
		{
			x = Bits.BitPermuteStep(x, 61680u, 12);
			x = Bits.BitPermuteStep(x, 13369548u, 6);
			x = Bits.BitPermuteStep(x, 572662306u, 1);
			x = Bits.BitPermuteStep(x, 202116108u, 2);
			return x;
		}

		internal static ulong Unshuffle(ulong x)
		{
			x = Bits.BitPermuteStep(x, 2459565876494606882uL, 1);
			x = Bits.BitPermuteStep(x, 868082074056920076uL, 2);
			x = Bits.BitPermuteStep(x, 67555025218437360uL, 4);
			x = Bits.BitPermuteStep(x, 280375465148160uL, 8);
			x = Bits.BitPermuteStep(x, 4294901760uL, 16);
			return x;
		}

		internal static ulong Unshuffle(ulong x, out ulong even)
		{
			ulong num = Unshuffle(x);
			even = num & 0xFFFFFFFFu;
			return num >> 32;
		}

		internal static ulong Unshuffle(ulong x0, ulong x1, out ulong even)
		{
			ulong num = Unshuffle(x0);
			ulong num2 = Unshuffle(x1);
			even = (num2 << 32) | (num & 0xFFFFFFFFu);
			return (num >> 32) | (num2 & 0xFFFFFFFF00000000uL);
		}
	}
}
