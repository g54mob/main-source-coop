using System;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct PlayerRevealEntry : INetworkSerializable, IEquatable<PlayerRevealEntry>
	{
		public ulong ClientId;

		public PlayerRole Role;

		public int Health;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref ClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref Role, default(FastBufferWriter.ForEnums));
			serializer.SerializeValue(ref Health, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(PlayerRevealEntry other)
		{
			if (ClientId == other.ClientId && Role == other.Role)
			{
				return Health == other.Health;
			}
			return false;
		}
	}
}
