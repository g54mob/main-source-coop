using System.Runtime.InteropServices;

namespace Fusion.Sockets
{
	[StructLayout(LayoutKind.Explicit, Size = 1)]
	internal struct NetUnreliableHeader
	{
		public const int SIZE_IN_BITS = 8;

		[FieldOffset(0)]
		public NetPacketType PacketType;

		public const int SIZE = 1;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_PACKET_TYPE = 0;

		internal const int BYTE_COUNT_OF_PACKET_TYPE = 1;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static NetUnreliableHeader Create()
		{
			NetUnreliableHeader result = default(NetUnreliableHeader);
			result.PacketType = NetPacketType.UnreliableData;
			return result;
		}
	}
}
