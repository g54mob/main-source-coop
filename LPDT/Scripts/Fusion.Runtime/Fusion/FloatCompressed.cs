using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct FloatCompressed : INetworkStruct, IEquatable<FloatCompressed>
	{
		[FieldOffset(0)]
		public int valueEncoded;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_VALUE_ENCODED = 0;

		internal const int BYTE_COUNT_OF_VALUE_ENCODED = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator FloatCompressed(float v)
		{
			FloatCompressed result = default(FloatCompressed);
			result.valueEncoded = FloatUtils.Compress(v);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float(FloatCompressed q)
		{
			return FloatUtils.Decompress(q.valueEncoded);
		}

		public readonly bool Equals(FloatCompressed other)
		{
			return valueEncoded == other.valueEncoded;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is FloatCompressed other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return valueEncoded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(FloatCompressed left, FloatCompressed right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(FloatCompressed left, FloatCompressed right)
		{
			return !left.Equals(right);
		}
	}
}
