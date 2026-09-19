namespace Fusion.Sockets
{
	internal struct NetSendEnvelope
	{
		public UniquePtr UserData;

		public readonly double SendTime;

		public readonly ushort Sequence;

		internal readonly NetPacketType PacketType;

		public NetSendEnvelope(double sendTime, ushort sequence, NetPacketType packetType, ref UniquePtr userData)
		{
			UserData = UniquePtr.MoveUniquePtr(ref userData);
			SendTime = sendTime;
			Sequence = sequence;
			PacketType = packetType;
		}
	}
}
