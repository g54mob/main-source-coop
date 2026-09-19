using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct Tick : INetworkStruct, IComparable<Tick>, IEquatable<Tick>
	{
		public sealed class RelationalComparer : IComparer<Tick>
		{
			public int Compare(Tick x, Tick y)
			{
				return x.Raw.CompareTo(y.Raw);
			}
		}

		public sealed class EqualityComparer : IEqualityComparer<Tick>
		{
			public bool Equals(Tick x, Tick y)
			{
				return x.Raw == y.Raw;
			}

			public int GetHashCode(Tick obj)
			{
				return obj.Raw;
			}
		}

		public const int ALIGNMENT = 4;

		[FieldOffset(0)]
		public int Raw;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW = 0;

		internal const int BYTE_COUNT_OF_RAW = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public Tick Next(int increment)
		{
			Tick result = default(Tick);
			result.Raw = Raw + increment;
			return result;
		}

		public readonly bool Equals(Tick other)
		{
			return Raw == other.Raw;
		}

		public int CompareTo(Tick other)
		{
			return Raw.CompareTo(other.Raw);
		}

		public override readonly bool Equals(object obj)
		{
			return obj is Tick other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return Raw;
		}

		public override readonly string ToString()
		{
			return $"[Tick:{(int)this}]";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Tick a, Tick b)
		{
			return a.Raw > b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Tick a, Tick b)
		{
			return a.Raw >= b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Tick a, Tick b)
		{
			return a.Raw < b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Tick a, Tick b)
		{
			return a.Raw <= b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Tick a, Tick b)
		{
			return a.Raw == b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Tick a, Tick b)
		{
			return a.Raw != b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Tick(int value)
		{
			Tick result = default(Tick);
			result.Raw = ((value >= 0) ? value : 0);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(Tick value)
		{
			return value.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint(Tick value)
		{
			return (uint)value.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool(Tick value)
		{
			return value.Raw > 0;
		}
	}
}
