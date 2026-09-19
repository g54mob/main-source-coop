using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	[NetworkStructWeaved(3)]
	public struct Vector3Compressed : INetworkStruct, IEquatable<Vector3Compressed>
	{
		[FieldOffset(0)]
		public int xEncoded;

		[FieldOffset(4)]
		public int yEncoded;

		[FieldOffset(8)]
		public int zEncoded;

		public const int SIZE = 12;

		public const int WORD_COUNT = 3;

		internal const int BYTE_OF_X_ENCODED = 0;

		internal const int BYTE_COUNT_OF_X_ENCODED = 4;

		internal const int BYTE_OF_Y_ENCODED = 4;

		internal const int BYTE_COUNT_OF_Y_ENCODED = 4;

		internal const int BYTE_OF_Z_ENCODED = 8;

		internal const int BYTE_COUNT_OF_Z_ENCODED = 4;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3Compressed(Vector3 v)
		{
			Vector3Compressed result = default(Vector3Compressed);
			result.xEncoded = FloatUtils.Compress(v.x);
			result.yEncoded = FloatUtils.Compress(v.y);
			result.zEncoded = FloatUtils.Compress(v.z);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3Compressed q)
		{
			Vector3 result = default(Vector3);
			result.x = FloatUtils.Decompress(q.xEncoded);
			result.y = FloatUtils.Decompress(q.yEncoded);
			result.z = FloatUtils.Decompress(q.zEncoded);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3Compressed(Vector2 v)
		{
			Vector3Compressed result = default(Vector3Compressed);
			result.xEncoded = FloatUtils.Compress(v.x);
			result.yEncoded = FloatUtils.Compress(v.y);
			result.zEncoded = 0;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector3Compressed q)
		{
			Vector2 result = default(Vector2);
			result.x = FloatUtils.Decompress(q.xEncoded);
			result.y = FloatUtils.Decompress(q.yEncoded);
			return result;
		}

		public readonly bool Equals(Vector3Compressed other)
		{
			return xEncoded == other.xEncoded && yEncoded == other.yEncoded && zEncoded == other.zEncoded;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is Vector3Compressed other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			int num = xEncoded;
			num = (num * 397) ^ yEncoded;
			return (num * 397) ^ zEncoded;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3Compressed left, Vector3Compressed right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3Compressed left, Vector3Compressed right)
		{
			return !left.Equals(right);
		}
	}
}
