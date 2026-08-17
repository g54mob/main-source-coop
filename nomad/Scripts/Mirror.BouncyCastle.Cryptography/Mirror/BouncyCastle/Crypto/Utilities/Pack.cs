namespace Mirror.BouncyCastle.Crypto.Utilities
{
	internal static class Pack
	{
		internal static void UInt32_To_BE(uint n, byte[] bs)
		{
			bs[0] = (byte)(n >> 24);
			bs[1] = (byte)(n >> 16);
			bs[2] = (byte)(n >> 8);
			bs[3] = (byte)n;
		}

		internal static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[off + 1] = (byte)(n >> 16);
			bs[off + 2] = (byte)(n >> 8);
			bs[off + 3] = (byte)n;
		}

		internal static void UInt32_To_BE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				UInt32_To_BE(ns[i], bs, off);
				off += 4;
			}
		}

		internal static uint BE_To_UInt32(byte[] bs)
		{
			return (uint)((bs[0] << 24) | (bs[1] << 16) | (bs[2] << 8) | bs[3]);
		}

		internal static uint BE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((bs[off] << 24) | (bs[off + 1] << 16) | (bs[off + 2] << 8) | bs[off + 3]);
		}

		internal static uint BE_To_UInt32_Low(byte[] bs, int off, int len)
		{
			uint num = bs[off];
			for (int i = 1; i < len; i++)
			{
				num <<= 8;
				num |= bs[off + i];
			}
			return num;
		}

		internal static void BE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = BE_To_UInt32(bs, off);
				off += 4;
			}
		}

		internal static void BE_To_UInt32(byte[] bs, int bsOff, uint[] ns, int nsOff, int nsLen)
		{
			for (int i = 0; i < nsLen; i++)
			{
				ns[nsOff + i] = BE_To_UInt32(bs, bsOff);
				bsOff += 4;
			}
		}

		internal static byte[] UInt64_To_BE(ulong n)
		{
			byte[] array = new byte[8];
			UInt64_To_BE(n, array);
			return array;
		}

		internal static void UInt64_To_BE(ulong n, byte[] bs)
		{
			UInt32_To_BE((uint)(n >> 32), bs);
			UInt32_To_BE((uint)n, bs, 4);
		}

		internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			UInt32_To_BE((uint)(n >> 32), bs, off);
			UInt32_To_BE((uint)n, bs, off + 4);
		}

		internal static void UInt64_To_BE(ulong[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				UInt64_To_BE(ns[i], bs, off);
				off += 8;
			}
		}

		internal static ulong BE_To_UInt64(byte[] bs)
		{
			uint num = BE_To_UInt32(bs);
			uint num2 = BE_To_UInt32(bs, 4);
			return ((ulong)num << 32) | num2;
		}

		internal static ulong BE_To_UInt64(byte[] bs, int off)
		{
			uint num = BE_To_UInt32(bs, off);
			uint num2 = BE_To_UInt32(bs, off + 4);
			return ((ulong)num << 32) | num2;
		}

		internal static void BE_To_UInt64(byte[] bs, int off, ulong[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = BE_To_UInt64(bs, off);
				off += 8;
			}
		}

		internal static void UInt32_To_LE(uint n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
			bs[2] = (byte)(n >> 16);
			bs[3] = (byte)(n >> 24);
		}

		internal static void UInt32_To_LE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[off + 1] = (byte)(n >> 8);
			bs[off + 2] = (byte)(n >> 16);
			bs[off + 3] = (byte)(n >> 24);
		}

		internal static void UInt32_To_LE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				UInt32_To_LE(ns[i], bs, off);
				off += 4;
			}
		}

		internal static void UInt32_To_LE(uint[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
		{
			for (int i = 0; i < nsLen; i++)
			{
				UInt32_To_LE(ns[nsOff + i], bs, bsOff);
				bsOff += 4;
			}
		}

		internal static uint LE_To_UInt32(byte[] bs, int off)
		{
			return (uint)(bs[off] | (bs[off + 1] << 8) | (bs[off + 2] << 16) | (bs[off + 3] << 24));
		}

		internal static uint LE_To_UInt32_Low(byte[] bs, int off, int len)
		{
			uint num = bs[off];
			int num2 = 0;
			for (int i = 1; i < len; i++)
			{
				num2 += 8;
				num |= (uint)(bs[off + i] << num2);
			}
			return num;
		}

		internal static void LE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = LE_To_UInt32(bs, off);
				off += 4;
			}
		}

		internal static void LE_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ns[nOff + i] = LE_To_UInt32(bs, bOff);
				bOff += 4;
			}
		}

		internal static void UInt64_To_LE(ulong n, byte[] bs)
		{
			UInt32_To_LE((uint)n, bs);
			UInt32_To_LE((uint)(n >> 32), bs, 4);
		}

		internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
		{
			UInt32_To_LE((uint)n, bs, off);
			UInt32_To_LE((uint)(n >> 32), bs, off + 4);
		}

		internal static void UInt64_To_LE(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
		{
			for (int i = 0; i < nsLen; i++)
			{
				UInt64_To_LE(ns[nsOff + i], bs, bsOff);
				bsOff += 8;
			}
		}

		internal static ulong LE_To_UInt64(byte[] bs, int off)
		{
			uint num = LE_To_UInt32(bs, off);
			return ((ulong)LE_To_UInt32(bs, off + 4) << 32) | num;
		}

		internal static void LE_To_UInt64(byte[] bs, int off, ulong[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = LE_To_UInt64(bs, off);
				off += 8;
			}
		}
	}
}
