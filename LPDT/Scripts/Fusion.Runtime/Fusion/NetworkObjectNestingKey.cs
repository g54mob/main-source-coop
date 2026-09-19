using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct NetworkObjectNestingKey : INetworkStruct, IEquatable<NetworkObjectNestingKey>
	{
		public sealed class EqualityComparer : IEqualityComparer<NetworkObjectNestingKey>
		{
			public bool Equals(NetworkObjectNestingKey x, NetworkObjectNestingKey y)
			{
				return x.Value == y.Value;
			}

			public int GetHashCode(NetworkObjectNestingKey obj)
			{
				return obj.Value;
			}
		}

		public const int ALIGNMENT = 4;

		[FieldOffset(0)]
		public int Value;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_VALUE = 0;

		internal const int BYTE_COUNT_OF_VALUE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public readonly bool IsNone => Value == 0;

		public readonly bool IsValid => Value > 0;

		public NetworkObjectNestingKey(int value)
		{
			Value = value;
		}

		public readonly bool Equals(NetworkObjectNestingKey other)
		{
			return Value == other.Value;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkObjectNestingKey other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return Value;
		}

		public override readonly string ToString()
		{
			return IsNone ? "[NestingKey:None]" : $"[NestingKey:{Value}]";
		}
	}
}
