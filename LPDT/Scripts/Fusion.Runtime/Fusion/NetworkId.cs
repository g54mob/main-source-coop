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
	public struct NetworkId : INetworkStruct, IEquatable<NetworkId>, IComparable, IComparable<NetworkId>
	{
		public sealed class EqualityComparer : IEqualityComparer<NetworkId>
		{
			public bool Equals(NetworkId a, NetworkId b)
			{
				return a.Raw == b.Raw;
			}

			public int GetHashCode(NetworkId id)
			{
				return (int)id.Raw;
			}
		}

		public const int BLOCK_SIZE = 8;

		public const int ALIGNMENT = 4;

		[FieldOffset(0)]
		public uint Raw;

		internal const int MAX_RESERVED_ID = 1023;

		private const uint RAW_RUNTIME_CONFIG = 1u;

		private const uint RAW_PLAYER_REF_DATA_ARRAY = 2u;

		private const uint RAW_SCENE_INFO = 3u;

		private const uint RAW_PHYSICS_INFO = 4u;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW = 0;

		internal const int BYTE_COUNT_OF_RAW = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static EqualityComparer Comparer { get; } = new EqualityComparer();

		public readonly bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Raw != 0;
			}
		}

		public readonly bool IsReserved
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Raw != 0 && Raw <= 1023;
			}
		}

		internal static NetworkId RuntimeConfig => new NetworkId(1u);

		internal static NetworkId SceneInfo => new NetworkId(3u);

		internal static NetworkId PhysicsInfo => new NetworkId(4u);

		public static NetworkId None => default(NetworkId);

		internal NetworkId(uint raw)
		{
			Raw = raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(NetworkId other)
		{
			return Raw == other.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(NetworkId other)
		{
			return (int)(Raw - other.Raw);
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkId networkId && Raw == networkId.Raw;
		}

		int IComparable.CompareTo(object obj)
		{
			return (obj is NetworkId other) ? CompareTo(other) : 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkId a, NetworkId b)
		{
			return a.Raw == b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkId a, NetworkId b)
		{
			return a.Raw != b.Raw;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool(NetworkId id)
		{
			return id.Raw != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Write(NetBitBuffer* buffer, NetworkId id)
		{
			buffer->WriteUInt32VarLength(id.Raw, 8);
		}

		public unsafe static NetworkId Read(NetBitBuffer* buffer)
		{
			NetworkId result = default(NetworkId);
			result.Raw = buffer->ReadUInt32VarLength(8);
			return result;
		}

		internal static NetworkId? TryRead(ref NetBitBufferManaged buffer)
		{
			uint? num = buffer.TryReadUInt32VarLength(8);
			if (num.HasValue)
			{
				NetworkId value = default(NetworkId);
				value.Raw = num.Value;
				return value;
			}
			return null;
		}

		internal unsafe static NetworkId? TryRead(NetBitBuffer* buffer)
		{
			uint? num = buffer->TryReadUInt32VarLength(8);
			if (num.HasValue)
			{
				NetworkId value = default(NetworkId);
				value.Raw = num.Value;
				return value;
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write(NetBitBuffer* buffer)
		{
			Write(buffer, this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Write(ref NetBitBufferManaged buffer, NetworkId id)
		{
			buffer.WriteUInt32VarLength(id.Raw, 8);
		}

		internal void Write(ref NetBitBufferManaged buffer)
		{
			Write(ref buffer, this);
		}

		public override readonly int GetHashCode()
		{
			return (int)Raw;
		}

		public override readonly string ToString()
		{
			if (IsValid)
			{
				return Raw switch
				{
					1u => "[Id:RuntimeConfig]", 
					3u => "[Id:SceneInfo]", 
					4u => "[Id:Physics]", 
					2u => "[Id:PlayerDataArray]", 
					_ => $"[Id:{Raw}]", 
				};
			}
			return "[Id:None]";
		}

		public string ToNamePrefixString()
		{
			return IsValid ? $"[{Raw}] " : "[Invalid] ";
		}

		public static NetworkId FromRaw(uint value)
		{
			NetworkId result = default(NetworkId);
			result.Raw = value;
			return result;
		}
	}
}
