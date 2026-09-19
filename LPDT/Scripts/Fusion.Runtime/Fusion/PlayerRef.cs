#define DEBUG
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion.Sockets;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct PlayerRef : INetworkStruct, IEquatable<PlayerRef>
	{
		private sealed class IndexEqualityComparer : IEqualityComparer<PlayerRef>
		{
			public bool Equals(PlayerRef x, PlayerRef y)
			{
				return x.Raw == y.Raw;
			}

			public int GetHashCode(PlayerRef obj)
			{
				return obj.Raw;
			}
		}

		private const int MASTER_CLIENT_RAW = -1;

		private const uint SERIALIZATION_OFFSET = 2u;

		internal const int INVALID_RAW = -10;

		[FieldOffset(0)]
		internal int Raw;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW = 0;

		internal const int BYTE_COUNT_OF_RAW = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static IEqualityComparer<PlayerRef> Comparer { get; } = new IndexEqualityComparer();

		public static PlayerRef Invalid
		{
			get
			{
				PlayerRef result = default(PlayerRef);
				result.Raw = -10;
				return result;
			}
		}

		public static PlayerRef None => default(PlayerRef);

		public static PlayerRef InternalMasterClientIdentifier
		{
			get
			{
				PlayerRef result = default(PlayerRef);
				result.Raw = -1;
				return result;
			}
		}

		[Obsolete("Use InternalMasterClientIdentifier instead.")]
		public static PlayerRef MasterClient => InternalMasterClientIdentifier;

		public readonly bool IsRealPlayer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Raw > 0;
			}
		}

		public readonly bool IsNone
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Raw == 0;
			}
		}

		public readonly bool IsInternalMasterClientIdentifier
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Raw == -1;
			}
		}

		[Obsolete]
		public readonly bool IsMasterClient => IsInternalMasterClientIdentifier;

		public readonly int RawEncoded => Raw;

		public readonly int AsIndex => Raw - 1;

		public readonly int PlayerId => Raw - 1;

		public readonly bool IsInvalid => !IsValid;

		public readonly bool IsValid => Raw >= -1;

		public override readonly bool Equals(object obj)
		{
			return obj is PlayerRef other && Equals(other);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override readonly int GetHashCode()
		{
			return Raw;
		}

		public override readonly string ToString()
		{
			switch (Raw)
			{
			case 0:
				return "[Player:None]";
			case -1:
				return "[Player:MasterClient]";
			default:
				if (Raw > 0)
				{
					return $"[Player:{Raw - 1}]";
				}
				return $"[Player:Invalid({Raw})]";
			}
		}

		public static PlayerRef FromEncoded(int encoded)
		{
			PlayerRef result = default(PlayerRef);
			result.Raw = encoded;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PlayerRef FromRaw(int raw)
		{
			return FromEncoded(raw);
		}

		public static PlayerRef FromIndex(int index)
		{
			PlayerRef result = default(PlayerRef);
			result.Raw = index + 1;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(PlayerRef a, PlayerRef b)
		{
			return a.Raw == b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(PlayerRef a, PlayerRef b)
		{
			return a.Raw != b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Write(NetBitBuffer* buffer, PlayerRef playerRef)
		{
			if (buffer->WriteBoolean(playerRef.IsRealPlayer))
			{
				buffer->WriteInt32VarLength(playerRef.AsIndex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Write<T>(T* buffer, PlayerRef playerRef) where T : unmanaged, INetBitWriteStream
		{
			if (buffer->WriteBoolean(playerRef.IsRealPlayer))
			{
				buffer->WriteInt32VarLength(playerRef.AsIndex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static PlayerRef Read(NetBitBuffer* buffer)
		{
			if (buffer->ReadBoolean())
			{
				PlayerRef result = FromIndex(buffer->ReadInt32VarLength());
				Assert.Check(!result.IsNone, "result.IsNone == false");
				return result;
			}
			return default(PlayerRef);
		}

		public readonly bool Equals(PlayerRef other)
		{
			return Raw == other.Raw;
		}

		internal static bool TryRead(ReadBuffer buffer, out PlayerRef result)
		{
			uint num = buffer.UIntVar();
			if (num == 0)
			{
				result = Invalid;
				return false;
			}
			num--;
			num += uint.MaxValue;
			result.Raw = (int)num;
			if (result.Raw < -1)
			{
				result = Invalid;
				return false;
			}
			return true;
		}

		internal static void Write(WriteBuffer buffer, PlayerRef value)
		{
			if (value.Raw < -1)
			{
				Assert.Check(!value.IsValid, "!value.IsValid");
				buffer.UIntVar(0u);
				return;
			}
			uint raw = (uint)value.Raw;
			raw -= uint.MaxValue;
			raw++;
			buffer.UIntVar(raw);
		}
	}
}
