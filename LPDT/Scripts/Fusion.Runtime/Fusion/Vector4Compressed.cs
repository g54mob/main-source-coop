using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	[NetworkStructWeaved(4)]
	public struct Vector4Compressed : INetworkStruct, IEquatable<Vector4Compressed>
	{
		[FieldOffset(0)]
		public int xEncoded;

		[FieldOffset(4)]
		public int yEncoded;

		[FieldOffset(8)]
		public int zEncoded;

		[FieldOffset(12)]
		public int wEncoded;

		public const int SIZE = 16;

		public const int WORD_COUNT = 4;

		internal const int BYTE_OF_X_ENCODED = 0;

		internal const int BYTE_COUNT_OF_X_ENCODED = 4;

		internal const int BYTE_OF_Y_ENCODED = 4;

		internal const int BYTE_COUNT_OF_Y_ENCODED = 4;

		internal const int BYTE_OF_Z_ENCODED = 8;

		internal const int BYTE_COUNT_OF_Z_ENCODED = 4;

		internal const int BYTE_OF_W_ENCODED = 12;

		internal const int BYTE_COUNT_OF_W_ENCODED = 4;

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

		public float Z
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FloatUtils.Decompress(zEncoded);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				zEncoded = FloatUtils.Compress(value);
			}
		}

		public float W
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FloatUtils.Decompress(wEncoded);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				wEncoded = FloatUtils.Compress(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4Compressed(Vector4 v)
		{
			Vector4Compressed result = default(Vector4Compressed);
			result.xEncoded = FloatUtils.Compress(v.x);
			result.yEncoded = FloatUtils.Compress(v.y);
			result.zEncoded = FloatUtils.Compress(v.z);
			result.wEncoded = FloatUtils.Compress(v.w);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector4Compressed q)
		{
			Vector4 result = default(Vector4);
			result.x = FloatUtils.Decompress(q.xEncoded);
			result.y = FloatUtils.Decompress(q.yEncoded);
			result.z = FloatUtils.Decompress(q.zEncoded);
			result.w = FloatUtils.Decompress(q.wEncoded);
			return result;
		}

		public readonly bool Equals(Vector4Compressed other)
		{
			return xEncoded == other.xEncoded && yEncoded == other.yEncoded && zEncoded == other.zEncoded && wEncoded == other.wEncoded;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is Vector4Compressed other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			int num = xEncoded;
			num = (num * 397) ^ yEncoded;
			num = (num * 397) ^ zEncoded;
			return (num * 397) ^ wEncoded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4Compressed left, Vector4Compressed right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4Compressed left, Vector4Compressed right)
		{
			return !left.Equals(right);
		}
	}
}
