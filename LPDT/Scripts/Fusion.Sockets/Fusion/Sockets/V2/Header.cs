using System.Runtime.InteropServices;

namespace Fusion.Sockets.V2
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	internal struct Header
	{
		[FieldOffset(0)]
		public NetPacketType PacketType;

		[FieldOffset(1)]
		public PacketTypeV2 PacketTypeV2;

		[FieldOffset(2)]
		public byte Channel;

		[FieldOffset(3)]
		private byte _reserved_0;

		[FieldOffset(4)]
		public ushort Sequence;

		[FieldOffset(6)]
		public ushort AckSequence;

		[FieldOffset(8)]
		public ulong AckMask;

		[FieldOffset(16)]
		public uint FragGroup;

		[FieldOffset(20)]
		public uint FragIndex;

		[FieldOffset(24)]
		public uint FragCount;

		[FieldOffset(28)]
		private uint _reserved_1;

		public const int SIZE = 32;

		public const int WORD_COUNT = 8;

		internal const int BYTE_OF_PACKET_TYPE = 0;

		internal const int BYTE_COUNT_OF_PACKET_TYPE = 1;

		internal const int BYTE_OF_PACKET_TYPE_V2 = 1;

		internal const int BYTE_COUNT_OF_PACKET_TYPE_V2 = 1;

		internal const int BYTE_OF_CHANNEL = 2;

		internal const int BYTE_COUNT_OF_CHANNEL = 1;

		internal const int BYTE_OF__RESERVED_0 = 3;

		internal const int BYTE_COUNT_OF__RESERVED_0 = 1;

		internal const int BYTE_OF_SEQUENCE = 4;

		internal const int BYTE_COUNT_OF_SEQUENCE = 2;

		internal const int BYTE_OF_ACK_SEQUENCE = 6;

		internal const int BYTE_COUNT_OF_ACK_SEQUENCE = 2;

		internal const int BYTE_OF_ACK_MASK = 8;

		internal const int BYTE_COUNT_OF_ACK_MASK = 8;

		internal const int BYTE_OF_FRAG_GROUP = 16;

		internal const int BYTE_COUNT_OF_FRAG_GROUP = 4;

		internal const int BYTE_OF_FRAG_INDEX = 20;

		internal const int BYTE_COUNT_OF_FRAG_INDEX = 4;

		internal const int BYTE_OF_FRAG_COUNT = 24;

		internal const int BYTE_COUNT_OF_FRAG_COUNT = 4;

		internal const int BYTE_OF__RESERVED_1 = 28;

		internal const int BYTE_COUNT_OF__RESERVED_1 = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public readonly bool IsLastFrag => PacketTypeV2.Has(PacketTypeV2.LastFrag_Bit);

		public bool Is(PacketTypeV2 type)
		{
			return PacketTypeV2.Has(type);
		}

		public unsafe override readonly string ToString()
		{
			ulong ackMask = AckMask;
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}, {14}: {15}, {16}: {17}, {18}: {19}, {20}: {21}; {22}: {23}", "PacketType", PacketType, "PacketTypeV2", PacketTypeV2, "Channel", Channel, "_reserved_0", _reserved_0, "Sequence", Sequence, "AckSequence", AckSequence, "AckMask", Maths.PrintBits((byte*)(&ackMask), 8), "FragGroup", FragGroup, "FragIndex", FragIndex, "FragCount", FragCount, "IsLastFrag", IsLastFrag, "_reserved_1", _reserved_1);
		}
	}
}
