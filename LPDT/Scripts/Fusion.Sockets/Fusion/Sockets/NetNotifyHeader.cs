using System.Runtime.InteropServices;

namespace Fusion.Sockets
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct NetNotifyHeader
	{
		public const int SIZE_IN_BYTES = 14;

		public const int SIZE_IN_BITS = 112;

		[FieldOffset(0)]
		public NetPacketType PacketType;

		[FieldOffset(1)]
		public byte Fragment;

		[FieldOffset(2)]
		public ushort Sequence;

		[FieldOffset(4)]
		public ushort AckSequence;

		[FieldOffset(6)]
		public ulong AckMask;

		public unsafe override readonly string ToString()
		{
			ulong ackMask = AckMask;
			return $"[Type: {PacketType} Frag:{Fragment} Seq:{Sequence}, AckSeq:{AckSequence}, AckMask:{Maths.PrintBits((byte*)(&ackMask), 8)}]";
		}
	}
}
