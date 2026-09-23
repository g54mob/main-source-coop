using System;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct PlayerScoreEntry : INetworkSerializable, IEquatable<PlayerScoreEntry>
	{
		public ulong ClientId;

		public int Score;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref ClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref Score, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(PlayerScoreEntry other)
		{
			if (ClientId == other.ClientId)
			{
				return Score == other.Score;
			}
			return false;
		}
	}
}
