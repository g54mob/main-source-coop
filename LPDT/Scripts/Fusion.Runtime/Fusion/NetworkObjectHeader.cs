using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 80)]
	[InlineHelp]
	[NetworkStructWeaved(20)]
	public struct NetworkObjectHeader : INetworkStruct, IEquatable<NetworkObjectHeader>
	{
		public const int WORDS = 20;

		public const int PLAYER_FLAGS_WORD = 9;

		public const int STATE_AUTHORITY_WORD = 8;

		[FieldOffset(0)]
		public NetworkId Id;

		[FieldOffset(4)]
		public readonly short WordCount;

		[FieldOffset(6)]
		public readonly short BehaviourCount;

		[FieldOffset(8)]
		public readonly NetworkObjectTypeId Type;

		[FieldOffset(16)]
		public readonly NetworkId NestingRoot;

		[FieldOffset(20)]
		public readonly NetworkObjectNestingKey NestingKey;

		[FieldOffset(24)]
		public readonly NetworkObjectHeaderFlags Flags;

		internal const int READ_ONLY_WORD_COUNT = 7;

		[FieldOffset(28)]
		public PlayerRef InputAuthority;

		[FieldOffset(32)]
		public PlayerRef StateAuthority;

		[FieldOffset(36)]
		internal NetworkObjectHeaderPlayerDataFlags PlayerFlags;

		public const int PLUGIN_VERSION_WORD = 10;

		[FieldOffset(40)]
		internal uint PluginVersion;

		[FieldOffset(44)]
		private unsafe fixed int _padding[9];

		public const int SIZE = 80;

		public const int WORD_COUNT = 20;

		internal const int BYTE_OF_ID = 0;

		internal const int BYTE_COUNT_OF_ID = 4;

		internal const int BYTE_OF_WORD_COUNT = 4;

		internal const int BYTE_COUNT_OF_WORD_COUNT = 2;

		internal const int BYTE_OF_BEHAVIOUR_COUNT = 6;

		internal const int BYTE_COUNT_OF_BEHAVIOUR_COUNT = 2;

		internal const int BYTE_OF_TYPE = 8;

		internal const int BYTE_COUNT_OF_TYPE = 8;

		internal const int BYTE_OF_NESTING_ROOT = 16;

		internal const int BYTE_COUNT_OF_NESTING_ROOT = 4;

		internal const int BYTE_OF_NESTING_KEY = 20;

		internal const int BYTE_COUNT_OF_NESTING_KEY = 4;

		internal const int BYTE_OF_FLAGS = 24;

		internal const int BYTE_COUNT_OF_FLAGS = 4;

		internal const int BYTE_OF_INPUT_AUTHORITY = 28;

		internal const int BYTE_COUNT_OF_INPUT_AUTHORITY = 4;

		internal const int BYTE_OF_STATE_AUTHORITY = 32;

		internal const int BYTE_COUNT_OF_STATE_AUTHORITY = 4;

		internal const int BYTE_OF_PLAYER_FLAGS = 36;

		internal const int BYTE_COUNT_OF_PLAYER_FLAGS = 4;

		internal const int BYTE_OF_PLUGIN_VERSION = 40;

		internal const int BYTE_COUNT_OF_PLUGIN_VERSION = 4;

		internal const int BYTE_OF__PADDING = 44;

		internal const int BYTE_COUNT_OF__PADDING = 36;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public readonly int ByteCount => WordCount * 4;

		public NetworkObjectHeader(NetworkId id, short wordCount, short behaviourCount, NetworkObjectTypeId type, NetworkId nestingRoot = default(NetworkId), NetworkObjectNestingKey nestingKey = default(NetworkObjectNestingKey), NetworkObjectHeaderFlags flags = (NetworkObjectHeaderFlags)0)
		{
			InputAuthority = default(PlayerRef);
			StateAuthority = default(PlayerRef);
			PlayerFlags = (NetworkObjectHeaderPlayerDataFlags)0;
			PluginVersion = 0u;
			Id = id;
			WordCount = wordCount;
			BehaviourCount = behaviourCount;
			Type = type;
			NestingRoot = nestingRoot;
			NestingKey = nestingKey;
			Flags = flags;
		}

		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			stringBuilder.Append("Id").Append(": ").Append(Id.ToString());
			stringBuilder.Append(", ").Append("WordCount").Append(": ")
				.Append(WordCount);
			stringBuilder.Append(", ").Append("BehaviourCount").Append(": ")
				.Append(BehaviourCount);
			if (Type.IsValid)
			{
				stringBuilder.Append(", ").Append("Type").Append(": ")
					.Append(Type.ToString());
			}
			if (NestingRoot.IsValid)
			{
				stringBuilder.Append(", ").Append("NestingRoot").Append(": ")
					.Append(NestingRoot.ToString());
			}
			if (NestingKey.IsValid)
			{
				stringBuilder.Append(", ").Append("NestingKey").Append(": ")
					.Append(NestingKey.ToString());
			}
			if (WordCount != 0)
			{
				stringBuilder.Append(", ").Append("WordCount").Append(": ")
					.Append(WordCount);
			}
			if (Flags != 0)
			{
				stringBuilder.Append(", ").Append("Flags").Append(": ")
					.Append(Flags.ToString());
			}
			if (InputAuthority != default(PlayerRef))
			{
				stringBuilder.Append(", ").Append("InputAuthority").Append(": ")
					.Append(InputAuthority.ToString());
			}
			if (StateAuthority != default(PlayerRef))
			{
				stringBuilder.Append(", ").Append("StateAuthority").Append(": ")
					.Append(StateAuthority.ToString());
			}
			if (PlayerFlags != 0)
			{
				stringBuilder.Append(", ").Append("PlayerFlags").Append(": ")
					.Append(PlayerFlags.ToString());
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		public readonly bool Equals(NetworkObjectHeader other)
		{
			return this == other;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkObjectHeader other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			int hashCode = Id.GetHashCode();
			hashCode = (hashCode * 397) ^ WordCount;
			hashCode = (hashCode * 397) ^ BehaviourCount;
			hashCode = (hashCode * 397) ^ Type.GetHashCode();
			hashCode = (hashCode * 397) ^ NestingRoot.GetHashCode();
			hashCode = (hashCode * 397) ^ NestingKey.GetHashCode();
			hashCode = (hashCode * 397) ^ (int)Flags;
			hashCode = (hashCode * 397) ^ InputAuthority.GetHashCode();
			hashCode = (hashCode * 397) ^ StateAuthority.GetHashCode();
			return (hashCode * 397) ^ PlayerFlags.GetHashCode();
		}

		public unsafe static bool operator ==(NetworkObjectHeader left, NetworkObjectHeader right)
		{
			return FusionUnsafe.Compare(&left, &right, 80) == 0;
		}

		public unsafe static bool operator !=(NetworkObjectHeader left, NetworkObjectHeader right)
		{
			return FusionUnsafe.Compare(&left, &right, 80) != 0;
		}
	}
}
