using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct NetworkBool : INetworkStruct, IEquatable<NetworkBool>
	{
		[FieldOffset(0)]
		[SerializeField]
		[FormerlySerializedAs("_value")]
		public int RawValue;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW_VALUE = 0;

		internal const int BYTE_COUNT_OF_RAW_VALUE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public NetworkBool(bool value)
		{
			RawValue = (value ? 1 : 0);
		}

		public readonly bool Equals(NetworkBool other)
		{
			return RawValue == other.RawValue;
		}

		public override readonly string ToString()
		{
			return (RawValue == 0) ? "false" : "true";
		}

		public override readonly bool Equals(object obj)
		{
			return (obj is NetworkBool other && Equals(other)) || (obj is bool b && Equals(b));
		}

		public readonly bool Equals(bool b)
		{
			return RawValue == 0 == b;
		}

		public override readonly int GetHashCode()
		{
			return RawValue;
		}

		public static implicit operator bool(NetworkBool val)
		{
			return val.RawValue == 1;
		}

		public static implicit operator NetworkBool(bool val)
		{
			NetworkBool result = default(NetworkBool);
			result.RawValue = (val ? 1 : 0);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(bool a, NetworkBool b)
		{
			return a == (bool)b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(bool a, NetworkBool b)
		{
			return !(a == b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkBool a, bool b)
		{
			return (bool)a == b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkBool a, bool b)
		{
			return !(a == b);
		}
	}
}
