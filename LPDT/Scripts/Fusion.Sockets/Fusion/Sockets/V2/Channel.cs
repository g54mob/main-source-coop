using System.Runtime.CompilerServices;

namespace Fusion.Sockets.V2
{
	internal struct Channel
	{
		public byte Id;

		public ChannelFlags Flags;

		public uint NotifyGroup;

		public PacketGroup.List NotifyQueue;

		public Packet.List SendQueue;

		public Packet.List ResendQueue;

		public uint SendGroup;

		public uint RecvGroup;

		public Packet.List RecvQueue;

		public readonly int MtuData => 1104;

		public readonly bool Reliable
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Flags.Has(ChannelFlags.Reliable);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool CheckFlag(ChannelFlags flag)
		{
			return Flags.Has(flag);
		}

		public override readonly string ToString()
		{
			return string.Format("[Channel: {0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}, {14}: {15}, {16}: {17}, {18}: {19}, {20}: {21}]", "Id", Id, "Flags", Flags, "NotifyGroup", NotifyGroup, "NotifyQueue", NotifyQueue.Count, "SendQueue", SendQueue.Count, "ResendQueue", ResendQueue.Count, "SendGroup", SendGroup, "RecvGroup", RecvGroup, "RecvQueue", RecvQueue.Count, "MtuData", MtuData, "Reliable", Reliable);
		}
	}
}
