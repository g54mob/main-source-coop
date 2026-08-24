using System.Net;
using System.Net.Sockets;

namespace LiteNetLib
{
	internal sealed class NetEvent
	{
		public enum EType
		{
			Connect = 0,
			Disconnect = 1,
			Receive = 2,
			ReceiveUnconnected = 3,
			Error = 4,
			ConnectionLatencyUpdated = 5,
			Broadcast = 6,
			ConnectionRequest = 7,
			MessageDelivered = 8,
			PeerAddressChanged = 9
		}

		public NetEvent Next;

		public EType Type;

		public NetPeer Peer;

		public IPEndPoint RemoteEndPoint;

		public object UserData;

		public int Latency;

		public SocketError ErrorCode;

		public DisconnectReason DisconnectReason;

		public ConnectionRequest ConnectionRequest;

		public DeliveryMethod DeliveryMethod;

		public byte ChannelNumber;

		public readonly NetPacketReader DataReader;

		public NetEvent(NetManager manager)
		{
			DataReader = new NetPacketReader(manager, this);
		}
	}
}
