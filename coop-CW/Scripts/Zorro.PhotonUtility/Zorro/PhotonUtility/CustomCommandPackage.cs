using System;
using ExitGames.Client.Photon;
using Unity.Collections;
using Zorro.Core.Serizalization;

namespace Zorro.PhotonUtility
{
	public abstract class CustomCommandPackage<CommandEnum> where CommandEnum : struct, IConvertible
	{
		public BinarySerializer Serialize()
		{
			BinarySerializer binarySerializer = new BinarySerializer(32, Allocator.Temp);
			SerializeData(binarySerializer);
			return binarySerializer;
		}

		protected abstract void SerializeData(BinarySerializer binarySerializer);

		public abstract void DeserializeData(BinaryDeserializer binaryDeserializer);

		public byte GetCommandEventCode()
		{
			return (byte)(int)(object)GetCommandType();
		}

		public abstract CommandEnum GetCommandType();

		public abstract SendOptions GetSendOptions();
	}
}
