using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit)]
	[NetworkStructWeaved(4)]
	public struct NetworkRNG : INetworkStruct
	{
		public const int SIZE = 16;

		public const uint MAX = uint.MaxValue;

		[FieldOffset(0)]
		private ulong state;

		[FieldOffset(8)]
		private ulong inc;

		internal const double FP_32_32_ToUnitDoubleInclusive = 2.3283064370807974E-10;

		internal const double FP_32_32_ToUnitDoubleExclusive = 2.3283064365386963E-10;

		internal const float FP_8_24_ToUnitSingleInclusive = 5.960465E-08f;

		internal const float FP_8_24_ToUnitSingleExclusive = 5.9604645E-08f;

		public readonly NetworkRNG Peek
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this;
			}
		}

		public uint Next(uint maxExclusive)
		{
			uint num = (0 - maxExclusive) % maxExclusive;
			uint num2;
			for (num2 = NextUInt32(); num2 < num; num2 = NextUInt32())
			{
			}
			return num2 % maxExclusive;
		}

		public uint NextInclusive(uint maxInclusive)
		{
			if (maxInclusive == uint.MaxValue)
			{
				return NextUInt32();
			}
			return Next(maxInclusive + 1);
		}

		public ulong Next(ulong maxExclusive)
		{
			if (maxExclusive <= uint.MaxValue)
			{
				return Next((uint)maxExclusive);
			}
			ulong num = (0 - maxExclusive) % maxExclusive;
			ulong num2;
			for (num2 = ((ulong)NextUInt32() << 32) | NextUInt32(); num2 < num; num2 = ((ulong)NextUInt32() << 32) | NextUInt32())
			{
			}
			return num2 % maxExclusive;
		}

		public ulong NextInclusive(ulong maxInclusive)
		{
			if (maxInclusive == ulong.MaxValue)
			{
				return NextUInt64();
			}
			return Next(maxInclusive + 1);
		}

		public ulong NextUInt64()
		{
			return ((ulong)NextUInt32() << 32) | NextUInt32();
		}

		public int NextInt32()
		{
			return (int)NextUInt32();
		}

		public long NextInt64()
		{
			return (long)NextUInt64();
		}

		public NetworkRNG(int seed)
		{
			ulong x = (ulong)seed;
			state = NextSplitMix64(ref x);
			inc = NextSplitMix64(ref x);
		}

		public uint NextUInt32()
		{
			ulong num = state;
			state = num * 6364136223846793005L + (inc | 1);
			uint num2 = (uint)(((num >> 18) ^ num) >> 27);
			int num3 = (int)(num >> 59);
			return (num2 >> num3) | (num2 << (-num3 & 0x1F));
		}

		public override readonly int GetHashCode()
		{
			int num = 17;
			num = num * 31 + state.GetHashCode();
			return num * 31 + inc.GetHashCode();
		}

		private static ulong NextSplitMix64(ref ulong x)
		{
			ulong num = (x += 11400714819323198485uL);
			num = (num ^ (num >> 30)) * 13787848793156543929uL;
			num = (num ^ (num >> 27)) * 10723151780598845931uL;
			return num ^ (num >> 31);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble()
		{
			return (double)NextUInt32() * 2.3283064370807974E-10;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDoubleExclusive()
		{
			return (double)NextUInt32() * 2.3283064365386963E-10;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextSingle()
		{
			return (float)(NextUInt32() >> 8) * 5.960465E-08f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextSingleExclusive()
		{
			return (float)(NextUInt32() >> 8) * 5.9604645E-08f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Next()
		{
			return NextSingle();
		}

		public int Next(int minInclusive, int maxExclusive)
		{
			if (minInclusive == maxExclusive)
			{
				return minInclusive;
			}
			if (minInclusive > maxExclusive)
			{
				int num = maxExclusive;
				maxExclusive = minInclusive;
				minInclusive = num;
			}
			uint maxExclusive2 = (uint)(maxExclusive - minInclusive);
			uint num2 = Next(maxExclusive2);
			return minInclusive + (int)num2;
		}

		public int NextInclusive(int minInclusive, int maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				int num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			uint num2 = (uint)(maxInclusive - minInclusive + 1);
			if (num2 == 0)
			{
				return NextInt32();
			}
			uint num3 = Next(num2);
			return minInclusive + (int)num3;
		}

		public uint Next(uint minInclusive, uint maxExclusive)
		{
			if (minInclusive == maxExclusive)
			{
				return minInclusive;
			}
			if (minInclusive > maxExclusive)
			{
				uint num = maxExclusive;
				maxExclusive = minInclusive;
				minInclusive = num;
			}
			uint maxExclusive2 = maxExclusive - minInclusive;
			uint num2 = Next(maxExclusive2);
			return minInclusive + num2;
		}

		public uint NextInclusive(uint minInclusive, uint maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				uint num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			uint num2 = maxInclusive - minInclusive + 1;
			if (num2 == 0)
			{
				return NextUInt32();
			}
			uint num3 = Next(num2);
			return minInclusive + num3;
		}

		public long Next(long minInclusive, long maxExclusive)
		{
			if (minInclusive == maxExclusive)
			{
				return minInclusive;
			}
			if (minInclusive > maxExclusive)
			{
				long num = maxExclusive;
				maxExclusive = minInclusive;
				minInclusive = num;
			}
			ulong maxExclusive2 = (ulong)(maxExclusive - minInclusive);
			ulong num2 = Next(maxExclusive2);
			return minInclusive + (long)num2;
		}

		public long NextInclusive(long minInclusive, long maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				long num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			ulong num2 = (ulong)(maxInclusive - minInclusive + 1);
			if (num2 == 0)
			{
				return NextInt64();
			}
			ulong num3 = Next(num2);
			return minInclusive + (long)num3;
		}

		public ulong Next(ulong minInclusive, ulong maxExclusive)
		{
			if (minInclusive == maxExclusive)
			{
				return minInclusive;
			}
			if (minInclusive > maxExclusive)
			{
				ulong num = maxExclusive;
				maxExclusive = minInclusive;
				minInclusive = num;
			}
			ulong maxExclusive2 = maxExclusive - minInclusive;
			ulong num2 = Next(maxExclusive2);
			return minInclusive + num2;
		}

		public ulong NextInclusive(ulong minInclusive, ulong maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				ulong num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			ulong num2 = maxInclusive - minInclusive + 1;
			if (num2 == 0)
			{
				return NextUInt64();
			}
			ulong num3 = Next(num2);
			return minInclusive + num3;
		}

		public float NextInclusive(float minInclusive, float maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				float num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			return minInclusive + NextSingle() * (maxInclusive - minInclusive);
		}

		public double NextInclusive(double minInclusive, double maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				double num = maxInclusive;
				maxInclusive = minInclusive;
				minInclusive = num;
			}
			return minInclusive + NextDouble() * (maxInclusive - minInclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using NextInclusive")]
		public double RangeInclusive(double minInclusive, double maxInclusive)
		{
			return NextInclusive(minInclusive, maxInclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using NextInclusive")]
		public float RangeInclusive(float minInclusive, float maxInclusive)
		{
			return NextInclusive(minInclusive, maxInclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using NextInclusive")]
		public int RangeInclusive(int minInclusive, int maxInclusive)
		{
			return NextInclusive(minInclusive, maxInclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using NextInclusive")]
		public uint RangeInclusive(uint minInclusive, uint maxInclusive)
		{
			return NextInclusive(minInclusive, maxInclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using Next")]
		public int RangeExclusive(int minExclusive, int maxExclusive)
		{
			return Next(minExclusive, maxExclusive);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Prefer using Next")]
		public uint RangeExclusive(uint minExclusive, uint maxExclusive)
		{
			return Next(minExclusive, maxExclusive);
		}
	}
}
