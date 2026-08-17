using FishNet.Managing;
using FishNet.Transporting;
using GameKit.Utilities;

namespace FishNet.Serializing.Helping
{
	internal static class Broadcasts
	{
		internal static PooledWriter WriteBroadcast<T>(NetworkManager networkManager, PooledWriter writer, T message, ref Channel channel)
		{
			writer.WritePacketId(PacketId.Broadcast);
			writer.WriteUInt16(typeof(T).FullName.GetStableHashU16());
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.Write(message);
			writer.WriteLength(pooledWriter.Length);
			writer.WriteArraySegment(pooledWriter.GetArraySegment());
			networkManager.TransportManager.CheckSetReliableChannel(writer.Length, ref channel);
			pooledWriter.Store();
			return writer;
		}
	}
}
