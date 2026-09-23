using System;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct VoxelModelPayload : INetworkSerializable, IEquatable<VoxelModelPayload>
	{
		public byte[] Data;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			if (Data == null)
			{
				Data = Array.Empty<byte>();
			}
			serializer.SerializeValue(ref Data, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(VoxelModelPayload other)
		{
			if (Data == other.Data)
			{
				return true;
			}
			if (Data == null || other.Data == null)
			{
				return false;
			}
			if (Data.Length != other.Data.Length)
			{
				return false;
			}
			for (int i = 0; i < Data.Length; i++)
			{
				if (Data[i] != other.Data[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
