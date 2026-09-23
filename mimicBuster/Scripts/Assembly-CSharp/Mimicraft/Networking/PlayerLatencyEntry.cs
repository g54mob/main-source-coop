using System;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct PlayerLatencyEntry : INetworkSerializable, IEquatable<PlayerLatencyEntry>
	{
		public ulong ClientId;

		public ushort PingMs;

		public bool IsLocalHost;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref ClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref PingMs, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref IsLocalHost, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(PlayerLatencyEntry other)
		{
			if (ClientId == other.ClientId && PingMs == other.PingMs)
			{
				return IsLocalHost == other.IsLocalHost;
			}
			return false;
		}
	}
}
