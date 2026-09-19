using System;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1)]
	public readonly struct NetworkSceneLoadId : IEquatable<NetworkSceneLoadId>
	{
		[FieldOffset(0)]
		public readonly byte Value;

		public const int SIZE = 1;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_VALUE = 0;

		internal const int BYTE_COUNT_OF_VALUE = 1;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public NetworkSceneLoadId(byte value)
		{
			Value = value;
		}

		public bool Equals(NetworkSceneLoadId other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			return obj is NetworkSceneLoadId other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator ==(NetworkSceneLoadId left, NetworkSceneLoadId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(NetworkSceneLoadId left, NetworkSceneLoadId right)
		{
			return !left.Equals(right);
		}

		public static implicit operator NetworkSceneLoadId(byte value)
		{
			return new NetworkSceneLoadId(value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
