using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	[NetworkStructWeaved(2)]
	public struct Vector2Compressed : INetworkStruct, IEquatable<Vector2Compressed>
	{
		[FieldOffset(0)]
		public int xEncoded;

		[FieldOffset(4)]
		public int yEncoded;

		public const int SIZE = 8;

		public const int WORD_COUNT = 2;

		internal const int BYTE_OF_X_ENCODED = 0;

		internal const int BYTE_COUNT_OF_X_ENCODED = 4;

		internal const int BYTE_OF_Y_ENCODED = 4;

		internal const int BYTE_COUNT_OF_Y_ENCODED = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public float X
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FloatUtils.Decompress(xEncoded);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				xEncoded = FloatUtils.Compress(value);
			}
		}

		public float Y
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FloatUtils.Decompress(yEncoded);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				yEncoded = FloatUtils.Compress(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2Compressed(Vector2 v)
		{
			Vector2Compressed result = default(Vector2Compressed);
			result.xEncoded = FloatUtils.Compress(v.x);
			result.yEncoded = FloatUtils.Compress(v.y);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2Compressed q)
		{
			Vector2 result = default(Vector2);
			result.x = FloatUtils.Decompress(q.xEncoded);
			result.y = FloatUtils.Decompress(q.yEncoded);
			return result;
		}

		public readonly bool Equals(Vector2Compressed other)
		{
			return xEncoded == other.xEncoded && yEncoded == other.yEncoded;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is Vector2Compressed other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return (xEncoded * 397) ^ yEncoded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2Compressed left, Vector2Compressed right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2Compressed left, Vector2Compressed right)
		{
			return !left.Equals(right);
		}
	}
}
