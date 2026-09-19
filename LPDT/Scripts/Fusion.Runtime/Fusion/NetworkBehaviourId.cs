using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	[NetworkStructWeaved(2)]
	public struct NetworkBehaviourId : INetworkStruct, IEquatable<NetworkBehaviourId>
	{
		[FieldOffset(0)]
		public NetworkId Object;

		[FieldOffset(4)]
		public int Behaviour;

		public const int SIZE = 8;

		public const int WORD_COUNT = 2;

		internal const int BYTE_OF_OBJECT = 0;

		internal const int BYTE_COUNT_OF_OBJECT = 4;

		internal const int BYTE_OF_BEHAVIOUR = 4;

		internal const int BYTE_COUNT_OF_BEHAVIOUR = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public readonly bool IsValid => Object.IsValid && Behaviour >= 0;

		public static NetworkBehaviourId None => default(NetworkBehaviourId);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(NetworkBehaviourId other)
		{
			return Object.Equals(other.Object) && Behaviour == other.Behaviour;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkBehaviourId other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return (Object.GetHashCode() * 397) ^ Behaviour;
		}

		public override readonly string ToString()
		{
			return $"[Object:{Object}, Behaviour:{Behaviour}]";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkBehaviourId a, NetworkBehaviourId b)
		{
			return a.Equals(b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkBehaviourId a, NetworkBehaviourId b)
		{
			return !a.Equals(b);
		}
	}
}
