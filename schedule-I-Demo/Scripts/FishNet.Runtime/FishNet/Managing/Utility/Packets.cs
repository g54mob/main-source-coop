using FishNet.Serializing;
using FishNet.Transporting;

namespace FishNet.Managing.Utility
{
	public class Packets
	{
		internal static int GetPacketLength(ushort packetId, PooledReader reader, Channel channel)
		{
			PacketId packetId2 = (PacketId)packetId;
			if (channel == Channel.Reliable || packetId2 == PacketId.Broadcast || packetId2 == PacketId.SyncVar)
			{
				return reader.ReadInt32();
			}
			if (channel == Channel.Unreliable)
			{
				return -2;
			}
			LogError($"Operation is unhandled for packetId {(PacketId)packetId} on channel {channel}.");
			return -2;
			void LogError(string message)
			{
				if (reader.NetworkManager != null)
				{
					reader.NetworkManager.LogError(message);
				}
				else
				{
					NetworkManager.StaticLogError(message);
				}
			}
		}
	}
}
