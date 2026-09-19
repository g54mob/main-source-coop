namespace Fusion.Sockets.V2
{
	internal struct Peer
	{
		public Timer Timer;

		public NetSocket Socket;

		public Packet.Pool SendPool;

		public Packet.Pool RecvPool;

		public unsafe byte* DeliverBuffer;

		public readonly double Clock => Timer.ElapsedInSeconds;

		public unsafe static Peer* Alloc()
		{
			Peer* ptr = FusionUnsafe.AllocAndClear<Peer>(8, "Fusion\\Fusion.Sockets\\_V2\\Peer.cs", 13);
			ptr->Timer = Timer.StartNew();
			ptr->DeliverBuffer = (byte*)FusionUnsafe.AllocAndClear(1048576, 8, "Fusion\\Fusion.Sockets\\_V2\\Peer.cs", 16);
			return ptr;
		}

		public unsafe static void Free(Peer* peer)
		{
			if (peer != null)
			{
				Packet.Pool.FreeAll(&peer->SendPool);
				Packet.Pool.FreeAll(&peer->RecvPool);
				FusionUnsafe.Free(ref peer->DeliverBuffer);
				FusionUnsafe.Free(ref peer);
			}
		}

		public unsafe static void Send(INetSocket socket, Peer* peer, Packet* packet)
		{
			socket.Send(peer->Socket, &packet->State.Address, packet->State.Buffer, packet->State.Size);
		}
	}
}
