using System;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct VoteKickState : INetworkSerializable, IEquatable<VoteKickState>
	{
		public bool Active;

		public ulong TargetClientId;

		public ulong StartedByClientId;

		public double EndsAtServerTime;

		public int Yes;

		public int No;

		public int Eligible;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref Active, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref TargetClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref StartedByClientId, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref EndsAtServerTime, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref Yes, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref No, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref Eligible, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(VoteKickState other)
		{
			if (Active == other.Active && TargetClientId == other.TargetClientId && StartedByClientId == other.StartedByClientId && EndsAtServerTime.Equals(other.EndsAtServerTime) && Yes == other.Yes && No == other.No)
			{
				return Eligible == other.Eligible;
			}
			return false;
		}
	}
}
