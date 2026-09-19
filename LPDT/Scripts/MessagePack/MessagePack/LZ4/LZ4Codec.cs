using System;

namespace MessagePack.LZ4
{
	internal static class LZ4Codec
	{
		internal static class HashTablePool
		{
			[ThreadStatic]
			private static ushort[]? ushortPool;

			[ThreadStatic]
			private static uint[]? uintPool;

			[ThreadStatic]
			private static int[]? intPool;

			public static ushort[] GetUShortHashTablePool()
			{
				if (ushortPool == null)
				{
					ushortPool = new ushort[2048];
				}
				else
				{
					Array.Clear(ushortPool, 0, ushortPool.Length);
				}
				return ushortPool;
			}

			public static uint[] GetUIntHashTablePool()
			{
				if (uintPool == null)
				{
					uintPool = new uint[1024];
				}
				else
				{
					Array.Clear(uintPool, 0, uintPool.Length);
				}
				return uintPool;
			}

			public static int[] GetIntHashTablePool()
			{
				if (intPool == null)
				{
					intPool = new int[1024];
				}
				else
				{
					Array.Clear(intPool, 0, intPool.Length);
				}
				return intPool;
			}
		}

		private const int MEMORY_USAGE = 12;

		private const int NOTCOMPRESSIBLE_DETECTIONLEVEL = 6;

		private const int MINMATCH = 4;

		private const int SKIPSTRENGTH = 6;

		private const int COPYLENGTH = 8;

		private const int LASTLITERALS = 5;

		private const int MFLIMIT = 12;

		private const int MINLENGTH = 13;

		private const int MAXD_LOG = 16;

		private const int MAXD = 65536;

		private const int MAXD_MASK = 65535;

		private const int MAX_DISTANCE = 65535;

		private const int ML_BITS = 4;

		private const int ML_MASK = 15;

		private const int RUN_BITS = 4;

		private const int RUN_MASK = 15;

		private const int STEPSIZE_64 = 8;

		private const int STEPSIZE_32 = 4;

		private const int LZ4_64KLIMIT = 65547;

		private const int HASH_LOG = 10;

		private const int HASH_TABLESIZE = 1024;

		private const int HASH_ADJUST = 22;

		private const int HASH64K_LOG = 11;

		private const int HASH64K_TABLESIZE = 2048;

		private const int HASH64K_ADJUST = 21;

		private const int HASHHC_LOG = 15;

		private const int HASHHC_TABLESIZE = 32768;

		private const int HASHHC_ADJUST = 17;

		private const int MAX_NB_ATTEMPTS = 256;

		private const int OPTIMAL_ML = 18;

		private const int BLOCK_COPY_LIMIT = 16;

		private static readonly int[] DECODER_TABLE_32 = new int[8] { 0, 3, 2, 3, 0, 0, 0, 0 };

		private static readonly int[] DECODER_TABLE_64 = new int[8] { 0, 0, 0, -1, 0, 1, 2, 3 };

		private static readonly int[] DEBRUIJN_TABLE_32 = new int[32]
		{
			0, 0, 3, 0, 3, 1, 3, 0, 3, 2,
			2, 1, 3, 2, 0, 1, 3, 3, 1, 2,
			2, 2, 2, 0, 3, 1, 2, 0, 1, 0,
			1, 1
		};

		private static readonly int[] DEBRUIJN_TABLE_64 = new int[64]
		{
			0, 0, 0, 0, 0, 1, 1, 2, 0, 3,
			1, 3, 1, 4, 2, 7, 0, 2, 3, 6,
			1, 5, 3, 5, 1, 3, 4, 4, 2, 5,
			6, 7, 7, 0, 1, 2, 3, 3, 4, 6,
			2, 6, 5, 5, 3, 4, 5, 6, 7, 1,
			2, 4, 6, 4, 4, 5, 7, 2, 6, 5,
			7, 6, 7, 7
		};

		public static int MaximumOutputLength(int inputLength)
		{
			checked
			{
				return inputLength + unchecked(inputLength / 255) + 16;
			}
		}

		internal static void CheckArguments(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength)
		{
			if (inputLength == 0)
			{
				outputLength = 0;
				return;
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			checked
			{
				if ((uint)inputOffset > (uint)input.Length)
				{
					throw new ArgumentOutOfRangeException("inputOffset");
				}
				if ((uint)inputLength > (uint)input.Length - (uint)inputOffset)
				{
					throw new ArgumentOutOfRangeException("inputLength");
				}
				if (output == null)
				{
					throw new ArgumentNullException("output");
				}
				if ((uint)outputOffset > (uint)output.Length)
				{
					throw new ArgumentOutOfRangeException("outputOffset");
				}
				if ((uint)outputLength <= (uint)output.Length - (uint)outputOffset)
				{
					return;
				}
				throw new ArgumentOutOfRangeException("outputLength");
			}
		}

		public unsafe static int Encode(ReadOnlySpan<byte> input, Span<byte> output)
		{
			if (output.Length == 0)
			{
				throw new MessagePackSerializationException("Output is empty.");
			}
			fixed (byte* src = input)
			{
				fixed (byte* dst = output)
				{
					if (input.Length < 65547)
					{
						fixed (ushort* hash_table = &HashTablePool.GetUShortHashTablePool()[0])
						{
							if (IntPtr.Size == 4)
							{
								return LZ4_compress64kCtx_32(hash_table, src, dst, input.Length, output.Length);
							}
							return LZ4_compress64kCtx_64(hash_table, src, dst, input.Length, output.Length);
						}
					}
					fixed (uint* hash_table2 = &HashTablePool.GetUIntHashTablePool()[0])
					{
						if (IntPtr.Size == 4)
						{
							return LZ4_compressCtx_32(hash_table2, src, dst, input.Length, output.Length);
						}
						return LZ4_compressCtx_64(hash_table2, src, dst, input.Length, output.Length);
					}
				}
			}
		}

		public unsafe static int Decode(ReadOnlySpan<byte> input, Span<byte> output)
		{
			if (output.Length == 0)
			{
				throw new MessagePackSerializationException("Output is empty.");
			}
			fixed (byte* src = input)
			{
				fixed (byte* dst = output)
				{
					int num = ((IntPtr.Size != 4) ? LZ4_uncompress_64(src, dst, output.Length) : LZ4_uncompress_32(src, dst, output.Length));
					if (num != input.Length)
					{
						throw new MessagePackSerializationException("LZ4 block is corrupted, or invalid length has been given.");
					}
					return output.Length;
				}
			}
		}

		private unsafe static int LZ4_compressCtx_32(uint* hash_table, byte* src, byte* dst, int src_len, int dst_maxlen)
		{
			fixed (int* ptr = &DEBRUIJN_TABLE_32[0])
			{
				byte* ptr2 = src;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + src_len;
				byte* ptr6 = ptr5 - 12;
				byte* ptr7 = dst;
				byte* ptr8 = ptr7 + dst_maxlen;
				byte* ptr9 = ptr5 - 5;
				byte* ptr10 = ptr9 - 1;
				byte* ptr11 = ptr9 - 3;
				byte* ptr12 = ptr8 - 6;
				byte* ptr13 = ptr8 - 8;
				if (src_len >= 13)
				{
					hash_table[(uint)((int)(*(uint*)ptr2) * -1640531535) >> 22] = (uint)(ptr2 - ptr3);
					ptr2++;
					uint num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
					while (true)
					{
						int num2 = 67;
						byte* ptr14 = ptr2;
						byte* ptr15;
						while (true)
						{
							uint num3 = num;
							int num4 = num2++ >> 6;
							ptr2 = ptr14;
							ptr14 = ptr2 + num4;
							if (ptr14 > ptr6)
							{
								break;
							}
							num = (uint)((int)(*(uint*)ptr14) * -1640531535) >> 22;
							ptr15 = ptr3 + hash_table[num3];
							hash_table[num3] = (uint)(ptr2 - ptr3);
							if (ptr15 < ptr2 - 65535 || *(uint*)ptr15 != *(uint*)ptr2)
							{
								continue;
							}
							goto IL_00f5;
						}
						break;
						IL_01d1:
						byte* ptr16;
						int num6;
						while (true)
						{
							*(ushort*)ptr7 = (ushort)(ptr2 - ptr15);
							ptr7 += 2;
							ptr2 += 4;
							ptr15 += 4;
							ptr4 = ptr2;
							while (true)
							{
								if (ptr2 < ptr11)
								{
									int num5 = *(int*)ptr15 ^ *(int*)ptr2;
									if (num5 == 0)
									{
										ptr2 += 4;
										ptr15 += 4;
										continue;
									}
									ptr2 += ptr[(uint)((num5 & -num5) * 125613361) >> 27];
									break;
								}
								if (ptr2 < ptr10 && *(ushort*)ptr15 == *(ushort*)ptr2)
								{
									ptr2 += 2;
									ptr15 += 2;
								}
								if (ptr2 < ptr9 && *ptr15 == *ptr2)
								{
									ptr2++;
								}
								break;
							}
							num6 = (int)(ptr2 - ptr4);
							if (ptr7 + (num6 >> 8) > ptr12)
							{
								return 0;
							}
							if (num6 >= 15)
							{
								byte* intPtr = ptr16;
								*intPtr += 15;
								for (num6 -= 15; num6 > 509; num6 -= 510)
								{
									*(ptr7++) = byte.MaxValue;
									*(ptr7++) = byte.MaxValue;
								}
								if (num6 > 254)
								{
									num6 -= 255;
									*(ptr7++) = byte.MaxValue;
								}
								*(ptr7++) = (byte)num6;
							}
							else
							{
								byte* intPtr2 = ptr16;
								*intPtr2 += (byte)num6;
							}
							if (ptr2 > ptr6)
							{
								break;
							}
							hash_table[(uint)((int)(*(uint*)(ptr2 - 2)) * -1640531535) >> 22] = (uint)(ptr2 - 2 - ptr3);
							uint num3 = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
							ptr15 = ptr3 + hash_table[num3];
							hash_table[num3] = (uint)(ptr2 - ptr3);
							if (ptr15 > ptr2 - 65536 && *(uint*)ptr15 == *(uint*)ptr2)
							{
								ptr16 = ptr7++;
								*ptr16 = 0;
								continue;
							}
							goto IL_0362;
						}
						ptr4 = ptr2;
						break;
						IL_00f5:
						while (ptr2 > ptr4 && ptr15 > src && ptr2[-1] == ptr15[-1])
						{
							ptr2--;
							ptr15--;
						}
						num6 = (int)(ptr2 - ptr4);
						ptr16 = ptr7++;
						if (ptr7 + num6 + (num6 >> 8) > ptr13)
						{
							return 0;
						}
						if (num6 >= 15)
						{
							int num7 = num6 - 15;
							*ptr16 = 240;
							if (num7 > 254)
							{
								do
								{
									*(ptr7++) = byte.MaxValue;
									num7 -= 255;
								}
								while (num7 > 254);
								*(ptr7++) = (byte)num7;
								BlockCopy32(ptr4, ptr7, num6);
								ptr7 += num6;
								goto IL_01d1;
							}
							*(ptr7++) = (byte)num7;
						}
						else
						{
							*ptr16 = (byte)(num6 << 4);
						}
						byte* ptr17 = ptr7 + num6;
						do
						{
							*(int*)ptr7 = *(int*)ptr4;
							ptr7 += 4;
							ptr4 += 4;
							*(int*)ptr7 = *(int*)ptr4;
							ptr7 += 4;
							ptr4 += 4;
						}
						while (ptr7 < ptr17);
						ptr7 = ptr17;
						goto IL_01d1;
						IL_0362:
						ptr4 = ptr2++;
						num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
					}
				}
				int num8 = (int)(ptr5 - ptr4);
				if (ptr7 + num8 + 1 + (num8 + 255 - 15) / 255 > ptr8)
				{
					return 0;
				}
				if (num8 >= 15)
				{
					*(ptr7++) = 240;
					for (num8 -= 15; num8 > 254; num8 -= 255)
					{
						*(ptr7++) = byte.MaxValue;
					}
					*(ptr7++) = (byte)num8;
				}
				else
				{
					*(ptr7++) = (byte)(num8 << 4);
				}
				BlockCopy32(ptr4, ptr7, (int)(ptr5 - ptr4));
				ptr7 += ptr5 - ptr4;
				return (int)(ptr7 - dst);
			}
		}

		private unsafe static int LZ4_compress64kCtx_32(ushort* hash_table, byte* src, byte* dst, int src_len, int dst_maxlen)
		{
			fixed (int* ptr = &DEBRUIJN_TABLE_32[0])
			{
				byte* ptr2 = src;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + src_len;
				byte* ptr6 = ptr5 - 12;
				byte* ptr7 = dst;
				byte* ptr8 = ptr7 + dst_maxlen;
				byte* ptr9 = ptr5 - 5;
				byte* ptr10 = ptr9 - 1;
				byte* ptr11 = ptr9 - 3;
				byte* ptr12 = ptr8 - 6;
				byte* ptr13 = ptr8 - 8;
				if (src_len >= 13)
				{
					ptr2++;
					uint num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
					while (true)
					{
						int num2 = 67;
						byte* ptr14 = ptr2;
						byte* ptr15;
						while (true)
						{
							uint num3 = num;
							int num4 = num2++ >> 6;
							ptr2 = ptr14;
							ptr14 = ptr2 + num4;
							if (ptr14 > ptr6)
							{
								break;
							}
							num = (uint)((int)(*(uint*)ptr14) * -1640531535) >> 21;
							ptr15 = ptr4 + (int)hash_table[num3];
							hash_table[num3] = (ushort)(ptr2 - ptr4);
							if (*(uint*)ptr15 != *(uint*)ptr2)
							{
								continue;
							}
							goto IL_00ce;
						}
						break;
						IL_01aa:
						byte* ptr16;
						while (true)
						{
							*(ushort*)ptr7 = (ushort)(ptr2 - ptr15);
							ptr7 += 2;
							ptr2 += 4;
							ptr15 += 4;
							ptr3 = ptr2;
							while (true)
							{
								if (ptr2 < ptr11)
								{
									int num5 = *(int*)ptr15 ^ *(int*)ptr2;
									if (num5 == 0)
									{
										ptr2 += 4;
										ptr15 += 4;
										continue;
									}
									ptr2 += ptr[(uint)((num5 & -num5) * 125613361) >> 27];
									break;
								}
								if (ptr2 < ptr10 && *(ushort*)ptr15 == *(ushort*)ptr2)
								{
									ptr2 += 2;
									ptr15 += 2;
								}
								if (ptr2 < ptr9 && *ptr15 == *ptr2)
								{
									ptr2++;
								}
								break;
							}
							int num6 = (int)(ptr2 - ptr3);
							if (ptr7 + (num6 >> 8) > ptr12)
							{
								return 0;
							}
							if (num6 >= 15)
							{
								byte* intPtr = ptr16;
								*intPtr += 15;
								for (num6 -= 15; num6 > 509; num6 -= 510)
								{
									*(ptr7++) = byte.MaxValue;
									*(ptr7++) = byte.MaxValue;
								}
								if (num6 > 254)
								{
									num6 -= 255;
									*(ptr7++) = byte.MaxValue;
								}
								*(ptr7++) = (byte)num6;
							}
							else
							{
								byte* intPtr2 = ptr16;
								*intPtr2 += (byte)num6;
							}
							if (ptr2 > ptr6)
							{
								break;
							}
							hash_table[(uint)((int)(*(uint*)(ptr2 - 2)) * -1640531535) >> 21] = (ushort)(ptr2 - 2 - ptr4);
							uint num3 = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
							ptr15 = ptr4 + (int)hash_table[num3];
							hash_table[num3] = (ushort)(ptr2 - ptr4);
							if (*(uint*)ptr15 == *(uint*)ptr2)
							{
								ptr16 = ptr7++;
								*ptr16 = 0;
								continue;
							}
							goto IL_032c;
						}
						ptr3 = ptr2;
						break;
						IL_00ce:
						while (ptr2 > ptr3 && ptr15 > src && ptr2[-1] == ptr15[-1])
						{
							ptr2--;
							ptr15--;
						}
						int num7 = (int)(ptr2 - ptr3);
						ptr16 = ptr7++;
						if (ptr7 + num7 + (num7 >> 8) > ptr13)
						{
							return 0;
						}
						if (num7 >= 15)
						{
							int num6 = num7 - 15;
							*ptr16 = 240;
							if (num6 > 254)
							{
								do
								{
									*(ptr7++) = byte.MaxValue;
									num6 -= 255;
								}
								while (num6 > 254);
								*(ptr7++) = (byte)num6;
								BlockCopy32(ptr3, ptr7, num7);
								ptr7 += num7;
								goto IL_01aa;
							}
							*(ptr7++) = (byte)num6;
						}
						else
						{
							*ptr16 = (byte)(num7 << 4);
						}
						byte* ptr17 = ptr7 + num7;
						do
						{
							*(int*)ptr7 = *(int*)ptr3;
							ptr7 += 4;
							ptr3 += 4;
							*(int*)ptr7 = *(int*)ptr3;
							ptr7 += 4;
							ptr3 += 4;
						}
						while (ptr7 < ptr17);
						ptr7 = ptr17;
						goto IL_01aa;
						IL_032c:
						ptr3 = ptr2++;
						num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
					}
				}
				int num8 = (int)(ptr5 - ptr3);
				if (ptr7 + num8 + 1 + (num8 - 15 + 255) / 255 > ptr8)
				{
					return 0;
				}
				if (num8 >= 15)
				{
					*(ptr7++) = 240;
					for (num8 -= 15; num8 > 254; num8 -= 255)
					{
						*(ptr7++) = byte.MaxValue;
					}
					*(ptr7++) = (byte)num8;
				}
				else
				{
					*(ptr7++) = (byte)(num8 << 4);
				}
				BlockCopy32(ptr3, ptr7, (int)(ptr5 - ptr3));
				ptr7 += ptr5 - ptr3;
				return (int)(ptr7 - dst);
			}
		}

		private unsafe static int LZ4_uncompress_32(byte* src, byte* dst, int dst_len)
		{
			fixed (int* ptr = &DECODER_TABLE_32[0])
			{
				byte* ptr2 = src;
				byte* ptr3 = dst;
				byte* ptr4 = ptr3 + dst_len;
				byte* ptr5 = ptr4 - 5;
				byte* ptr6 = ptr4 - 8;
				byte* ptr7 = ptr4 - 8;
				while (true)
				{
					uint num = *(ptr2++);
					int num2;
					if ((num2 = (int)(num >> 4)) == 15)
					{
						int num3;
						while ((num3 = *(ptr2++)) == 255)
						{
							num2 += 255;
						}
						num2 += num3;
					}
					byte* ptr8 = ptr3 + num2;
					if (ptr8 > ptr6)
					{
						if (ptr8 != ptr4)
						{
							break;
						}
						BlockCopy32(ptr2, ptr3, num2);
						ptr2 += num2;
						return (int)(ptr2 - src);
					}
					do
					{
						*(int*)ptr3 = *(int*)ptr2;
						ptr3 += 4;
						ptr2 += 4;
						*(int*)ptr3 = *(int*)ptr2;
						ptr3 += 4;
						ptr2 += 4;
					}
					while (ptr3 < ptr8);
					ptr2 -= ptr3 - ptr8;
					ptr3 = ptr8;
					byte* ptr9 = ptr8 - (int)(*(ushort*)ptr2);
					ptr2 += 2;
					if (ptr9 < dst)
					{
						break;
					}
					if ((num2 = (int)(num & 0xF)) == 15)
					{
						while (*ptr2 == byte.MaxValue)
						{
							ptr2++;
							num2 += 255;
						}
						num2 += *(ptr2++);
					}
					if (ptr3 - ptr9 < 4)
					{
						*ptr3 = *ptr9;
						ptr3[1] = ptr9[1];
						ptr3[2] = ptr9[2];
						ptr3[3] = ptr9[3];
						ptr3 += 4;
						ptr9 += 4;
						ptr9 -= ptr[ptr3 - ptr9];
						*(int*)ptr3 = *(int*)ptr9;
						ptr3 = ptr3;
						ptr9 = ptr9;
					}
					else
					{
						*(int*)ptr3 = *(int*)ptr9;
						ptr3 += 4;
						ptr9 += 4;
					}
					ptr8 = ptr3 + num2;
					if (ptr8 > ptr7)
					{
						if (ptr8 > ptr5)
						{
							break;
						}
						do
						{
							*(int*)ptr3 = *(int*)ptr9;
							ptr3 += 4;
							ptr9 += 4;
							*(int*)ptr3 = *(int*)ptr9;
							ptr3 += 4;
							ptr9 += 4;
						}
						while (ptr3 < ptr6);
						while (ptr3 < ptr8)
						{
							*(ptr3++) = *(ptr9++);
						}
						ptr3 = ptr8;
					}
					else
					{
						do
						{
							*(int*)ptr3 = *(int*)ptr9;
							ptr3 += 4;
							ptr9 += 4;
							*(int*)ptr3 = *(int*)ptr9;
							ptr3 += 4;
							ptr9 += 4;
						}
						while (ptr3 < ptr8);
						ptr3 = ptr8;
					}
				}
				return (int)(-(ptr2 - src));
			}
		}

		private unsafe static void BlockCopy32(byte* src, byte* dst, int len)
		{
			while (len >= 4)
			{
				*(int*)dst = *(int*)src;
				dst = (byte*)checked(unchecked((nuint)dst) + (nuint)4u);
				src = (byte*)checked(unchecked((nuint)src) + (nuint)4u);
				len = checked(len - 4);
			}
			if (len >= 2)
			{
				*(short*)dst = *(short*)src;
				dst = (byte*)checked(unchecked((nuint)dst) + (nuint)2u);
				src = (byte*)checked(unchecked((nuint)src) + (nuint)2u);
				len = checked(len - 2);
			}
			if (len >= 1)
			{
				*dst = *src;
			}
		}

		private unsafe static int LZ4_compressCtx_64(uint* hash_table, byte* src, byte* dst, int src_len, int dst_maxlen)
		{
			fixed (int* ptr = &DEBRUIJN_TABLE_64[0])
			{
				byte* ptr2 = src;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + src_len;
				byte* ptr6 = ptr5 - 12;
				byte* ptr7 = dst;
				byte* ptr8 = ptr7 + dst_maxlen;
				byte* ptr9 = ptr5 - 5;
				byte* ptr10 = ptr9 - 1;
				byte* ptr11 = ptr9 - 3;
				byte* ptr12 = ptr9 - 7;
				byte* ptr13 = ptr8 - 6;
				byte* ptr14 = ptr8 - 8;
				if (src_len >= 13)
				{
					hash_table[(uint)((int)(*(uint*)ptr2) * -1640531535) >> 22] = (uint)(ptr2 - ptr3);
					ptr2++;
					uint num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
					while (true)
					{
						int num2 = 67;
						byte* ptr15 = ptr2;
						byte* ptr16;
						while (true)
						{
							uint num3 = num;
							int num4 = num2++ >> 6;
							ptr2 = ptr15;
							ptr15 = ptr2 + num4;
							if (ptr15 > ptr6)
							{
								break;
							}
							num = (uint)((int)(*(uint*)ptr15) * -1640531535) >> 22;
							ptr16 = ptr3 + hash_table[num3];
							hash_table[num3] = (uint)(ptr2 - ptr3);
							if (ptr16 < ptr2 - 65535 || *(uint*)ptr16 != *(uint*)ptr2)
							{
								continue;
							}
							goto IL_00fb;
						}
						break;
						IL_01c5:
						byte* ptr17;
						int num6;
						while (true)
						{
							*(ushort*)ptr7 = (ushort)(ptr2 - ptr16);
							ptr7 += 2;
							ptr2 += 4;
							ptr16 += 4;
							ptr4 = ptr2;
							while (true)
							{
								if (ptr2 < ptr12)
								{
									long num5 = *(long*)ptr16 ^ *(long*)ptr2;
									if (num5 == 0L)
									{
										ptr2 += 8;
										ptr16 += 8;
										continue;
									}
									ptr2 += ptr[(ulong)((num5 & -num5) * 151050438428048703L) >> 58];
									break;
								}
								if (ptr2 < ptr11 && *(uint*)ptr16 == *(uint*)ptr2)
								{
									ptr2 += 4;
									ptr16 += 4;
								}
								if (ptr2 < ptr10 && *(ushort*)ptr16 == *(ushort*)ptr2)
								{
									ptr2 += 2;
									ptr16 += 2;
								}
								if (ptr2 < ptr9 && *ptr16 == *ptr2)
								{
									ptr2++;
								}
								break;
							}
							num6 = (int)(ptr2 - ptr4);
							if (ptr7 + (num6 >> 8) > ptr13)
							{
								return 0;
							}
							if (num6 >= 15)
							{
								byte* intPtr = ptr17;
								*intPtr += 15;
								for (num6 -= 15; num6 > 509; num6 -= 510)
								{
									*(ptr7++) = byte.MaxValue;
									*(ptr7++) = byte.MaxValue;
								}
								if (num6 > 254)
								{
									num6 -= 255;
									*(ptr7++) = byte.MaxValue;
								}
								*(ptr7++) = (byte)num6;
							}
							else
							{
								byte* intPtr2 = ptr17;
								*intPtr2 += (byte)num6;
							}
							if (ptr2 > ptr6)
							{
								break;
							}
							hash_table[(uint)((int)(*(uint*)(ptr2 - 2)) * -1640531535) >> 22] = (uint)(ptr2 - 2 - ptr3);
							uint num3 = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
							ptr16 = ptr3 + hash_table[num3];
							hash_table[num3] = (uint)(ptr2 - ptr3);
							if (ptr16 > ptr2 - 65536 && *(uint*)ptr16 == *(uint*)ptr2)
							{
								ptr17 = ptr7++;
								*ptr17 = 0;
								continue;
							}
							goto IL_036f;
						}
						ptr4 = ptr2;
						break;
						IL_00fb:
						while (ptr2 > ptr4 && ptr16 > src && ptr2[-1] == ptr16[-1])
						{
							ptr2--;
							ptr16--;
						}
						num6 = (int)(ptr2 - ptr4);
						ptr17 = ptr7++;
						if (ptr7 + num6 + (num6 >> 8) > ptr14)
						{
							return 0;
						}
						if (num6 >= 15)
						{
							int num7 = num6 - 15;
							*ptr17 = 240;
							if (num7 > 254)
							{
								do
								{
									*(ptr7++) = byte.MaxValue;
									num7 -= 255;
								}
								while (num7 > 254);
								*(ptr7++) = (byte)num7;
								BlockCopy64(ptr4, ptr7, num6);
								ptr7 += num6;
								goto IL_01c5;
							}
							*(ptr7++) = (byte)num7;
						}
						else
						{
							*ptr17 = (byte)(num6 << 4);
						}
						byte* ptr18 = ptr7 + num6;
						do
						{
							*(long*)ptr7 = *(long*)ptr4;
							ptr7 += 8;
							ptr4 += 8;
						}
						while (ptr7 < ptr18);
						ptr7 = ptr18;
						goto IL_01c5;
						IL_036f:
						ptr4 = ptr2++;
						num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 22;
					}
				}
				int num8 = (int)(ptr5 - ptr4);
				if (ptr7 + num8 + 1 + (num8 + 255 - 15) / 255 > ptr8)
				{
					return 0;
				}
				if (num8 >= 15)
				{
					*(ptr7++) = 240;
					for (num8 -= 15; num8 > 254; num8 -= 255)
					{
						*(ptr7++) = byte.MaxValue;
					}
					*(ptr7++) = (byte)num8;
				}
				else
				{
					*(ptr7++) = (byte)(num8 << 4);
				}
				BlockCopy64(ptr4, ptr7, (int)(ptr5 - ptr4));
				ptr7 += ptr5 - ptr4;
				return (int)(ptr7 - dst);
			}
		}

		private unsafe static int LZ4_compress64kCtx_64(ushort* hash_table, byte* src, byte* dst, int src_len, int dst_maxlen)
		{
			fixed (int* ptr = &DEBRUIJN_TABLE_64[0])
			{
				byte* ptr2 = src;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + src_len;
				byte* ptr6 = ptr5 - 12;
				byte* ptr7 = dst;
				byte* ptr8 = ptr7 + dst_maxlen;
				byte* ptr9 = ptr5 - 5;
				byte* ptr10 = ptr9 - 1;
				byte* ptr11 = ptr9 - 3;
				byte* ptr12 = ptr9 - 7;
				byte* ptr13 = ptr8 - 6;
				byte* ptr14 = ptr8 - 8;
				if (src_len >= 13)
				{
					ptr2++;
					uint num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
					while (true)
					{
						int num2 = 67;
						byte* ptr15 = ptr2;
						byte* ptr16;
						while (true)
						{
							uint num3 = num;
							int num4 = num2++ >> 6;
							ptr2 = ptr15;
							ptr15 = ptr2 + num4;
							if (ptr15 > ptr6)
							{
								break;
							}
							num = (uint)((int)(*(uint*)ptr15) * -1640531535) >> 21;
							ptr16 = ptr4 + (int)hash_table[num3];
							hash_table[num3] = (ushort)(ptr2 - ptr4);
							if (*(uint*)ptr16 != *(uint*)ptr2)
							{
								continue;
							}
							goto IL_00d4;
						}
						break;
						IL_019e:
						byte* ptr17;
						while (true)
						{
							*(ushort*)ptr7 = (ushort)(ptr2 - ptr16);
							ptr7 += 2;
							ptr2 += 4;
							ptr16 += 4;
							ptr3 = ptr2;
							while (true)
							{
								if (ptr2 < ptr12)
								{
									long num5 = *(long*)ptr16 ^ *(long*)ptr2;
									if (num5 == 0L)
									{
										ptr2 += 8;
										ptr16 += 8;
										continue;
									}
									ptr2 += ptr[(ulong)((num5 & -num5) * 151050438428048703L) >> 58];
									break;
								}
								if (ptr2 < ptr11 && *(uint*)ptr16 == *(uint*)ptr2)
								{
									ptr2 += 4;
									ptr16 += 4;
								}
								if (ptr2 < ptr10 && *(ushort*)ptr16 == *(ushort*)ptr2)
								{
									ptr2 += 2;
									ptr16 += 2;
								}
								if (ptr2 < ptr9 && *ptr16 == *ptr2)
								{
									ptr2++;
								}
								break;
							}
							int num6 = (int)(ptr2 - ptr3);
							if (ptr7 + (num6 >> 8) > ptr13)
							{
								return 0;
							}
							if (num6 >= 15)
							{
								byte* intPtr = ptr17;
								*intPtr += 15;
								for (num6 -= 15; num6 > 509; num6 -= 510)
								{
									*(ptr7++) = byte.MaxValue;
									*(ptr7++) = byte.MaxValue;
								}
								if (num6 > 254)
								{
									num6 -= 255;
									*(ptr7++) = byte.MaxValue;
								}
								*(ptr7++) = (byte)num6;
							}
							else
							{
								byte* intPtr2 = ptr17;
								*intPtr2 += (byte)num6;
							}
							if (ptr2 > ptr6)
							{
								break;
							}
							hash_table[(uint)((int)(*(uint*)(ptr2 - 2)) * -1640531535) >> 21] = (ushort)(ptr2 - 2 - ptr4);
							uint num3 = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
							ptr16 = ptr4 + (int)hash_table[num3];
							hash_table[num3] = (ushort)(ptr2 - ptr4);
							if (*(uint*)ptr16 == *(uint*)ptr2)
							{
								ptr17 = ptr7++;
								*ptr17 = 0;
								continue;
							}
							goto IL_0339;
						}
						ptr3 = ptr2;
						break;
						IL_00d4:
						while (ptr2 > ptr3 && ptr16 > src && ptr2[-1] == ptr16[-1])
						{
							ptr2--;
							ptr16--;
						}
						int num7 = (int)(ptr2 - ptr3);
						ptr17 = ptr7++;
						if (ptr7 + num7 + (num7 >> 8) > ptr14)
						{
							return 0;
						}
						if (num7 >= 15)
						{
							int num6 = num7 - 15;
							*ptr17 = 240;
							if (num6 > 254)
							{
								do
								{
									*(ptr7++) = byte.MaxValue;
									num6 -= 255;
								}
								while (num6 > 254);
								*(ptr7++) = (byte)num6;
								BlockCopy64(ptr3, ptr7, num7);
								ptr7 += num7;
								goto IL_019e;
							}
							*(ptr7++) = (byte)num6;
						}
						else
						{
							*ptr17 = (byte)(num7 << 4);
						}
						byte* ptr18 = ptr7 + num7;
						do
						{
							*(long*)ptr7 = *(long*)ptr3;
							ptr7 += 8;
							ptr3 += 8;
						}
						while (ptr7 < ptr18);
						ptr7 = ptr18;
						goto IL_019e;
						IL_0339:
						ptr3 = ptr2++;
						num = (uint)((int)(*(uint*)ptr2) * -1640531535) >> 21;
					}
				}
				int num8 = (int)(ptr5 - ptr3);
				if (ptr7 + num8 + 1 + (num8 - 15 + 255) / 255 > ptr8)
				{
					return 0;
				}
				if (num8 >= 15)
				{
					*(ptr7++) = 240;
					for (num8 -= 15; num8 > 254; num8 -= 255)
					{
						*(ptr7++) = byte.MaxValue;
					}
					*(ptr7++) = (byte)num8;
				}
				else
				{
					*(ptr7++) = (byte)(num8 << 4);
				}
				BlockCopy64(ptr3, ptr7, (int)(ptr5 - ptr3));
				ptr7 += ptr5 - ptr3;
				return (int)(ptr7 - dst);
			}
		}

		private unsafe static int LZ4_uncompress_64(byte* src, byte* dst, int dst_len)
		{
			fixed (int* ptr = &DECODER_TABLE_32[0])
			{
				fixed (int* ptr2 = &DECODER_TABLE_64[0])
				{
					byte* ptr3 = src;
					byte* ptr4 = dst;
					byte* ptr5 = ptr4 + dst_len;
					byte* ptr6 = ptr5 - 5;
					byte* ptr7 = ptr5 - 8;
					byte* ptr8 = ptr5 - 8 - 4;
					while (true)
					{
						byte b = *(ptr3++);
						int num;
						if ((num = b >> 4) == 15)
						{
							int num2;
							while ((num2 = *(ptr3++)) == 255)
							{
								num += 255;
							}
							num += num2;
						}
						byte* ptr9 = ptr4 + num;
						if (ptr9 > ptr7)
						{
							if (ptr9 != ptr5)
							{
								break;
							}
							BlockCopy64(ptr3, ptr4, num);
							ptr3 += num;
							return (int)(ptr3 - src);
						}
						do
						{
							*(long*)ptr4 = *(long*)ptr3;
							ptr4 += 8;
							ptr3 += 8;
						}
						while (ptr4 < ptr9);
						ptr3 -= ptr4 - ptr9;
						ptr4 = ptr9;
						byte* ptr10 = ptr9 - (int)(*(ushort*)ptr3);
						ptr3 += 2;
						if (ptr10 < dst)
						{
							break;
						}
						if ((num = b & 0xF) == 15)
						{
							while (*ptr3 == byte.MaxValue)
							{
								ptr3++;
								num += 255;
							}
							num += *(ptr3++);
						}
						if (ptr4 - ptr10 < 8)
						{
							int num3 = ptr2[ptr4 - ptr10];
							*ptr4 = *ptr10;
							ptr4[1] = ptr10[1];
							ptr4[2] = ptr10[2];
							ptr4[3] = ptr10[3];
							ptr4 += 4;
							ptr10 += 4;
							ptr10 -= ptr[ptr4 - ptr10];
							*(int*)ptr4 = *(int*)ptr10;
							ptr4 += 4;
							ptr10 -= num3;
						}
						else
						{
							*(long*)ptr4 = *(long*)ptr10;
							ptr4 += 8;
							ptr10 += 8;
						}
						ptr9 = ptr4 + num - 4;
						if (ptr9 > ptr8)
						{
							if (ptr9 > ptr6)
							{
								break;
							}
							while (ptr4 < ptr7)
							{
								*(long*)ptr4 = *(long*)ptr10;
								ptr4 += 8;
								ptr10 += 8;
							}
							while (ptr4 < ptr9)
							{
								*(ptr4++) = *(ptr10++);
							}
							ptr4 = ptr9;
						}
						else
						{
							do
							{
								*(long*)ptr4 = *(long*)ptr10;
								ptr4 += 8;
								ptr10 += 8;
							}
							while (ptr4 < ptr9);
							ptr4 = ptr9;
						}
					}
					return (int)(-(ptr3 - src));
				}
			}
		}

		private unsafe static void BlockCopy64(byte* src, byte* dst, int len)
		{
			while (len >= 8)
			{
				*(long*)dst = *(long*)src;
				dst = (byte*)checked(unchecked((nuint)dst) + (nuint)8u);
				src = (byte*)checked(unchecked((nuint)src) + (nuint)8u);
				len = checked(len - 8);
			}
			if (len >= 4)
			{
				*(int*)dst = *(int*)src;
				dst = (byte*)checked(unchecked((nuint)dst) + (nuint)4u);
				src = (byte*)checked(unchecked((nuint)src) + (nuint)4u);
				len = checked(len - 4);
			}
			if (len >= 2)
			{
				*(short*)dst = *(short*)src;
				dst = (byte*)checked(unchecked((nuint)dst) + (nuint)2u);
				src = (byte*)checked(unchecked((nuint)src) + (nuint)2u);
				len = checked(len - 2);
			}
			if (len >= 1)
			{
				*dst = *src;
			}
		}
	}
}
