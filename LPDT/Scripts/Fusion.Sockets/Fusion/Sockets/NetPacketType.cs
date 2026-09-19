namespace Fusion.Sockets
{
	internal enum NetPacketType : byte
	{
		V2 = 250,
		Command = 1,
		UnreliableData = 2,
		Unconnected = 5,
		MtuDiscoveryReq = 6,
		MtuDiscoveryRep = 7,
		NotifyReliableData = 8
	}
}
