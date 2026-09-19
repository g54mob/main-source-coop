#define DEBUG
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	internal struct ChangeTick : INetworkStruct, IComparable<ChangeTick>, IEquatable<ChangeTick>
	{
		public const int ALIGNMENT = 4;

		[FieldOffset(0)]
		public uint Raw;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW = 0;

		internal const int BYTE_COUNT_OF_RAW = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static ChangeTick MaxValue
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				ChangeTick result = default(ChangeTick);
				result.Raw = uint.MaxValue;
				return result;
			}
		}

		public readonly Tick AsPrevSimulationTick
		{
			get
			{
				Tick result = default(Tick);
				result.Raw = Math.Max(0, (int)(Raw / 2 - 1));
				return result;
			}
		}

		public readonly bool IsSimulationTick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (Raw & 1) == 0;
			}
		}

		public static ChangeTick FromSimulationTick(Tick tick)
		{
			Assert.Check(tick.Raw >= 0, "tick.Raw >= 0");
			ChangeTick result = default(ChangeTick);
			result.Raw = (uint)(tick.Raw * 2);
			return result;
		}

		public static ChangeTick FromRaw(uint tick)
		{
			ChangeTick result = default(ChangeTick);
			result.Raw = tick;
			return result;
		}

		public static ChangeTick FromRecvTick(Tick tick)
		{
			Assert.Check(tick.Raw >= 0, "tick.Raw >= 0");
			ChangeTick result = default(ChangeTick);
			result.Raw = (uint)(tick.Raw * 2 + 1);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(ChangeTick other)
		{
			return Raw == other.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(ChangeTick other)
		{
			return Raw.CompareTo(other.Raw);
		}

		public override readonly bool Equals(object obj)
		{
			return obj is Tick tick && Equals(tick);
		}

		public override readonly int GetHashCode()
		{
			return Raw.GetHashCode();
		}

		public override readonly string ToString()
		{
			return string.Format("[ChangeTick:{0}{1}]", Raw / 2, (Raw % 2 == 0) ? "" : "+");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(ChangeTick a, ChangeTick b)
		{
			return a.Raw == b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(ChangeTick a, ChangeTick b)
		{
			return a.Raw != b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(ChangeTick a, ChangeTick b)
		{
			return a.Raw > b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(ChangeTick a, ChangeTick b)
		{
			return a.Raw < b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(ChangeTick a, ChangeTick b)
		{
			return a.Raw >= b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(ChangeTick a, ChangeTick b)
		{
			return a.Raw <= b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ChangeTick operator +(ChangeTick a, ChangeTick b)
		{
			return FromRaw(a.Raw + b.Raw);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ChangeTick operator -(ChangeTick a, ChangeTick b)
		{
			return FromRaw(a.Raw - b.Raw);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ChangeTick Max(ChangeTick a, ChangeTick b)
		{
			return (a.Raw > b.Raw) ? a : b;
		}
	}
}
