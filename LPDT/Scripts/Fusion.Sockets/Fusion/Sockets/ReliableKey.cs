using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.Sockets
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct ReliableKey : IEquatable<ReliableKey>
	{
		[FieldOffset(0)]
		public unsafe fixed byte Data[16];

		public const int SIZE = 16;

		public const int WORD_COUNT = 4;

		internal const int BYTE_OF_DATA = 0;

		internal const int BYTE_COUNT_OF_DATA = 16;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public unsafe readonly void GetInts(out int key0, out int key1, out int key2, out int key3)
		{
			ReliableKey reliableKey = this;
			key0 = *(int*)reliableKey.Data;
			key1 = *(int*)Unsafe.AsPointer(ref reliableKey.Data[4]);
			key2 = *(int*)Unsafe.AsPointer(ref reliableKey.Data[8]);
			key3 = *(int*)Unsafe.AsPointer(ref reliableKey.Data[12]);
		}

		public unsafe readonly void GetUlongs(out ulong key0, out ulong key1)
		{
			ReliableKey reliableKey = this;
			key0 = *(ulong*)reliableKey.Data;
			key1 = *(ulong*)Unsafe.AsPointer(ref reliableKey.Data[8]);
		}

		public unsafe static ReliableKey FromInts(int key0 = 0, int key1 = 0, int key2 = 0, int key3 = 0)
		{
			ReliableKey result = default(ReliableKey);
			*(int*)result.Data = key0;
			*(int*)Unsafe.AsPointer(ref result.Data[4]) = key1;
			*(int*)Unsafe.AsPointer(ref result.Data[8]) = key2;
			*(int*)Unsafe.AsPointer(ref result.Data[12]) = key3;
			return result;
		}

		public unsafe static ReliableKey FromULongs(ulong key0 = 0uL, ulong key1 = 0uL)
		{
			ReliableKey result = default(ReliableKey);
			*(ulong*)result.Data = key0;
			*(ulong*)Unsafe.AsPointer(ref result.Data[8]) = key1;
			return result;
		}

		public readonly bool Equals(ReliableKey other)
		{
			GetInts(out var key, out var key2, out var key3, out var key4);
			other.GetInts(out var key5, out var key6, out var key7, out var key8);
			return key == key5 && key2 == key6 && key3 == key7 && key4 == key8;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is ReliableKey other && Equals(other);
		}

		public static bool operator ==(ReliableKey a, ReliableKey b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(ReliableKey a, ReliableKey b)
		{
			return !a.Equals(b);
		}

		public override readonly int GetHashCode()
		{
			GetInts(out var key, out var key2, out var key3, out var key4);
			int num = 17;
			num = num * 31 + key;
			num = num * 31 + key2;
			num = num * 31 + key3;
			return num * 31 + key4;
		}
	}
}
