using System;
using Unity.Collections;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct PlayerNameEntry : INetworkSerializable, IEquatable<PlayerNameEntry>
	{
		public ulong ClientId;

		public FixedString64Bytes Name;

		public ulong SteamId;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref ClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref Name, default(FastBufferWriter.ForFixedStrings));
			serializer.SerializeValue(ref SteamId, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(PlayerNameEntry other)
		{
			if (ClientId == other.ClientId && Name.Equals(other.Name))
			{
				return SteamId == other.SteamId;
			}
			return false;
		}
	}
}
